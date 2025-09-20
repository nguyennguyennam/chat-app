using chat_service.domain.Entity;
namespace chat_service.domain.Repository;
using System;
using System.Text.RegularExpressions;


/*
   Methods for sending, receiving, editing, deleting, and searching messages within chat groups.
*/
public interface IChatRepository
{
    //Core Methods
    Task<Groups_Message> SendMessage(Groups_Message message);
    Task<Groups_Message>  EditMessage(Guid GroupId, Guid messageId,  string newContent);
    Task<Groups_Message> GetMessageById(Guid GroupId, Guid MessageId);
    Task DeleteMessage(Guid GroupId, Guid MessageId);

    //Query methods
    Task<List<Groups_Message>> GetMessagesByGroup(Guid GroupId, int limit, int offset);
    Task<List<Groups_Message>> SearchMessages(Guid GroupId, string keyword);

    //Status Methods
    Task<Groups_Message?> UpdateMessageStatus(Guid GroupId, Guid MessageId, string status);
    Task<Groups_Message> GetMessageStatus(Guid GroupId, Guid messageId);

}