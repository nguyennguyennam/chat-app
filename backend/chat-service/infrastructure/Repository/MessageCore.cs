/*
    This file implements the IChatRepository interface to provide 
    data access methods for Message_Content entity.
*/

using chat_service.domain.Repository;
using chat_service.domain.Entity;
using chat_service.infrastructure.Models;
using chat_service.infrastructure.Utility.Database;
using Cassandra;

namespace chat_service.infrastructure.Repository
{
    /*
        Repository implementation for handling message operations in Cassandra.
        This class interacts with the `message_content_by_group` table to perform CRUD 
        and query operations for chat messages.
    */
    public class MessageRepository : IChatRepository
    {
        private readonly CassandraDb _db;

        public MessageRepository(CassandraDb db)
        {
            _db = db;
        }

        /*
            Insert a new message into the database.
        */
        public async Task<Groups_Message> SendMessage(Groups_Message message)
        {
            if (message.Message == null)
                throw new ArgumentNullException(nameof(message.Message), "Message content cannot be null when sending a message");

            var cqlMsg = new Groups_Message_Cql
            {
                GroupId = message.GroupId,
                SenderId = message.SenderId,
                SentAt = message.SentAt,
                MessageId = message.Message.MessageId,
                ContentType = message.Message.ContentType_?.Value.ToString() ?? string.Empty,
                ContentText = message.Message.ContentText,
                ContentUrl = message.Message.ContentURL,
                MimeType = message.Message.MimeType,
                SizeBytes = message.Message.Size_Bytes,
                Metadata = message.Message.Metadata?.RootElement.GetRawText() ?? "{}",
                Status = message.Message.Status?.Value.ToString() ?? "Sent"
            };

            await _db.GetMapper().InsertAsync(cqlMsg);
            return message;
        }

        /*
            Edit an existing message content.
        */
        public async Task<Groups_Message> EditMessage(Guid groupId, Guid messageId, string newContent)
        {
            var cql = "UPDATE message_content_by_group SET content_text = ?, status = 'Sent' " +
                      "WHERE group_id = ? AND message_id = ?";
            await _db.GetSession().ExecuteAsync(new SimpleStatement(cql, newContent, groupId, messageId));
            return await GetMessageById(groupId, messageId) ?? throw new Exception("Message not found");
        }

        /*
            Retrieve a specific message by its groupId and messageId.
        */
        public async Task<Groups_Message> GetMessageById(Guid groupId, Guid messageId)
        {
            var cql = "SELECT * FROM message_content_by_group WHERE group_id = ? AND message_id = ?";
            var result = await _db.GetMapper().FirstOrDefaultAsync<Groups_Message>(cql, groupId, messageId);
            return result ?? throw new Exception("Message not found");
        }

        /*
            Delete a message from the database.
        */
        public async Task DeleteMessage(Guid groupId, Guid messageId)
        {
            var cql = "DELETE FROM message_content_by_group WHERE group_id = ? AND message_id = ?";
            await _db.GetSession().ExecuteAsync(new SimpleStatement(cql, groupId, messageId));
        }

        /*
            Retrieve messages for a group with cursor-based pagination.
        */
        public async Task<List<Groups_Message>> GetMessagesByGroup(Guid groupId, DateTime? before, int pageSize = 15)
        {
            string cql;
            object[] parameters;

            if (before.HasValue)
            {
                cql = "SELECT * FROM message_content_by_group WHERE group_id = ? AND sent_at < ? LIMIT ?";
                parameters = new object[] { groupId, before.Value, pageSize };
            }
            else
            {
                cql = "SELECT * FROM message_content_by_group WHERE group_id = ? LIMIT ?";
                parameters = new object[] { groupId, pageSize };
            }

            var messages = await _db.GetMapper().FetchAsync<Groups_Message>(cql, parameters);
            return messages.ToList();
        }

        /*
            Search messages within a group by keyword.
            ⚠ Requires SASI index on `content_text` (disabled by default in Cassandra 4.1).
        */
        public async Task<List<Groups_Message>> SearchMessages(Guid groupId, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<Groups_Message>();

            var cql = "SELECT * FROM message_content_by_group WHERE group_id = ? AND content_text LIKE ? LIMIT 50";
            var results = await _db.GetMapper().FetchAsync<Groups_Message>(
                cql,
                groupId,
                "%" + keyword + "%"
            );

            return results.ToList();
        }

        /*
            Update the status of a message (e.g., Sent, Delivered, Seen).
        */
        public async Task<Groups_Message> UpdateMessageStatus(Guid groupId, Guid messageId, string status)
        {
            var cql = "UPDATE message_content_by_group SET status = ? WHERE group_id = ? AND message_id = ?";
            await _db.GetSession().ExecuteAsync(new SimpleStatement(cql, status, groupId, messageId));
            return await GetMessageById(groupId, messageId) ?? throw new Exception("Message not found");
        }
    }
}
