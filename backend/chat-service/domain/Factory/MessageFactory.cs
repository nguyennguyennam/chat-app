using System.Text.Json;
using Cassandra;
using chat_service.domain.Entity;
using chat_service.domain.ValueObject;

namespace chat_service.domain.Factory
{
    public static class MessageFactory
    {
        /**
         * Create a new Groups_Message with its Message_Content.
         * Supports two cases:
         * 1. Text-only (no URL).
         * 2. Media/file message (with URL).
         */
        public static Groups_Message CreateNewMessage(
            Guid groupId,
            Guid senderId,
            string contentText,
            string contentType,
            string mimeType,
            string? contentUrl = null,
            long sizeBytes = 0,
            JsonDocument? metadata = null
        )
        {
            var messageContent = new Message_Content(
                TimeUuid.NewId(),
                new ContentType(contentType),
                contentText,
                contentUrl ?? string.Empty,
                mimeType,
                sizeBytes,
                metadata ?? JsonDocument.Parse("{}"),
                new MessageStatus("Sent")
            );

            return new Groups_Message(groupId,  messageContent, senderId);
        }
    }
}
