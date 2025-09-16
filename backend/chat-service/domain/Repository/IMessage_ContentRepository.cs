using chat_service.domain.Entity;
using System;


/*
   Methods for sending, receiving, editing, deleting, and searching messages within chat groups.
*/
public interface IChatRepository
{
    Task<Message_Content> SendMessage(Message_Content message, Guid GroupId, Guid SenderId);
    Task<Message_Content?> EditMessage(Guid MessageId, string newContent);
    Task<Message_Content> ReceiveMessage(Guid MessageId);
    Task DeleteMessage(Guid MessageId);
    Task<List<Message_Content>> GetMessagesByGroup(Guid GroupId, int limit, int offset);
    Task<Message_Content?> SearchMessages(Guid GroupId, string keyword);
    Task<Message_Content?> UpdateMessageStatus(Guid MessageId, string status);
}