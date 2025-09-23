using chat_service.application.DTOs;

namespace chat_service.application.Interface;


/*
    This interface lists all of the Task for persistance layer to work with
*/
public interface IChatUseCase
{
    Task<MessageDto> SendMessage(SendMessageRequestDto message);
    Task<MessageDto> EditMessage(Guid GroupId, Guid messageId, string newContent);
    Task DeleteMessage(Guid GroupId, Guid MessageId);
    Task<List<MessageDto>> GetMessagesByGroup(Guid groupId, DateTime? before, int pageSize = 15);
    Task<List<MessageDto>> SearchMessage(Guid groupId, string keyword);
    
}