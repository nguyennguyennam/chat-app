/*
    This file implements the IMessage_ContentRepository interface to provide data access methods for Message_Content entity.
*/

using chat_service.domain.Repository;
using chat_service.domain.Entity;
using chat_service.infrastructure.Utility.Database;
namespace chat_service.infrastructure.Repository;

public class MessageCore : IChatRepository
{
    private readonly CassandraDb _db;
    public MessageCore(CassandraDb db)
    {
        _db = db;
    }

    /*
        Send messages and store them in the database.
        Parameters: message - The message entity to be sent and stored.
        Returns: The sent message entity with updated information (e.g, timestamps, IDs).
    */

    public async Task<Groups_Message> SendMessage(Groups_Message message)
    {
        // Implementation for sending a message
        await _db.GetMapper().InsertAsync(message);
        return message;
    }

    /*
        Edit an existing message's content.
        Parameters: GroupId - The ID of the group where the message was sent.
                    messageId - The ID of the message to be edited.
                    newContent - The new content to replace the existing message content.
        Returns: The updated message entity after editing.
    */

    public async Task<Groups_Message> EditMessage(Guid groupId, Guid messageId, string newContent)
    {
        await _db.GetMapper().UpdateAsync<Groups_Message>($"SET Message.ContentText = {newContent} WHERE GroupId = {groupId} and MessageId = {messageId}");
        return await GetMessageById(groupId, messageId) ?? throw new Exception("Message not found after update");
    }

    /*
        Retrieve a message by its ID.
        Parameters: GroupId - The ID of the group where the message was sent.
                    MessageId - The ID of the message to be retrieved.
        Returns: The message entity if found, otherwise null.
    */
    public async Task<Groups_Message?> GetMessageById(Guid groupId, Guid messageId)
    {
        return await _db.GetMapper().FirstOrDefaultAsync<Groups_Message>($"WHERE GroupId ={groupId} and MessageId = {messageId}");
    }

    public async Task DeleteMessage(Guid groupId, Guid messageId)
    {
        await _db.GetMapper().DeleteAsync<Groups_Message>($"WHERE GroupId = {groupId} and MessageId = {messageId}");
    }

    /*
        Retrieve messages for a specific group with pagination.
        Parameters: GroupId - The ID of the group to retrieve messages from.
                    before - Cursor, the last message from the viewed page size
                    pageSize - A number of messages for every scroll
        Returns: A list of message entities for the specified group.
    */
    public async Task<List<Groups_Message>> GetMessagesByGroup(Guid groupId, DateTime? before, int pageSize)
    {
        string cql;
        object[] parameters;
        if (before.HasValue)
        {
            // Fetch older messages
            cql = "WHERE GroupId = ? AND SentAt < ? LIMIT ?";
            parameters = new object[] { groupId, before, pageSize };
        }
        else
        {
            cql = "WHERE GroupId = ? LIMIT ?";
            parameters = new object[] { groupId, pageSize };
        }
        var messages = await _db.GetMapper().FetchAsync<Groups_Message>(cql, parameters);
        return messages.ToList();
    }
    

    /*
        Search messages in a specific group by keyword.
        Parameters: GroupId - The ID of the group to search messages in.
                    keyword - The keyword to search for within message contents.
        Returns: A list of message entities that match the search criteria.
    */
    public async Task<List<Groups_Message>> SearchMessages(Guid groupId, string keyword)
    {
        
    }
    

}