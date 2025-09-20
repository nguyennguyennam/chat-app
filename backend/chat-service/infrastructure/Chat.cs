using chat_service.domain.Entity;
using chat_service.domain.Repository;
using chat_service.infrastructure.Socket;
namespace chat_service.infrastructure.Repository;


/*
    Implementation of IChatRepository for managing chat messages.
*/
// public class ChatRepository : IChatRepository
// {
//     /*
//         Params: message - The message content to be sent.
//                 GroupId - The ID of the group where the message is sent.
//                 SenderId - The ID of the user sending the message.
//         Returns: The sent message with metadata.
//         Description: Sends a message to a specified group from a sender.
//     */
//     // public Task<Message_Content> SendMessage(Message_Content message, Guid GroupId, Guid userId)
//     // {
        
//     // }
// }