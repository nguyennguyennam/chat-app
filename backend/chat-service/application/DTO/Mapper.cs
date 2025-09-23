using chat_service.application.DTOs;
using chat_service.domain.Entity;

namespace chat_service.application.Mapper
{
    public static class MessageMapper
    {
        /** Map domain entity to DTO */
        public static MessageDto ToDto(Groups_Message message)
        {
            var msg = message.Message;
            if (msg is null)
            {
                throw new ArgumentNullException(nameof(message), "Groups_Message.Message must not be null");
            }

            return new MessageDto
            {
                MessageId = msg.MessageId,
                GroupId = message.GroupId,
                SenderId = message.SenderId,
                SentAt = message.SentAt,
                ContentType = msg.ContentType_?.Value.ToString() ?? string.Empty,
                ContentText = msg.ContentText,
                ContentUrl = msg.ContentURL,
                MimeType = msg.MimeType,
                SizeBytes = msg.Size_Bytes,
                Status = msg.Status.Value.ToString()
            };
        }

        /** Map request DTO to domain entity via Factory */
        public static Groups_Message FromRequest(SendMessageRequestDto dto)
        {
            return chat_service.domain.Factory.MessageFactory.CreateNewMessage(
                dto.GroupId,
                dto.SenderId,
                dto.ContentText,
                dto.ContentType,
                dto.MimeType,
                dto.ContentUrl,
                dto.SizeBytes
            );
        }
    }
}
