using chat_service.domain.Entity;
namespace chat_service.domain.Repository;
using System;


/*
   Methods for sending, receiving, editing, deleting, and searching messages within chat groups.
*/
public interface IChatRepository
{
    //Core Methods
    Task<Message_Content> SendMessage(Message_Content message, Guid GroupId, Guid SenderId);
    Task<Message_Content?> EditMessage(Guid MessageId, string newContent);
    Task<Message_Content?> GetMessageById(Guid MessageId);
    Task DeleteMessage(Guid MessageId);

    //Query methods
    Task<List<Message_Content>> GetMessagesByGroup(Guid GroupId, int limit, int offset);
    Task<List<Message_Content?>> SearchMessages(Guid GroupId, string keyword);

    //Status Methods
    Task<Message_Content?> UpdateMessageStatus(Guid MessageId, string status, Guid userId);
    Task<Message_Content> GetMessageStatus(Guid messageId, Guid userId);

}