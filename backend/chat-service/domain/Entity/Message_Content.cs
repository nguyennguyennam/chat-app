using System.Text.Json;
using Cassandra;
using chat_service.domain.ValueObject;
namespace chat_service.domain.Entity;

/*
    This class represents the detailed content of a message sent in a chat
*/
public class Message_Content
{
    public TimeUuid MessageId { get; private set; } = TimeUuid.NewId(); //Enable time-based UUIDs for ordering messages by creation time
    public ContentType ContentType_ { get; private set; } = default!;
    public string ContentText { get; private set; } = string.Empty;
    public string ContentURL { get; private set; } = string.Empty;

    public string MimeType { get; private set; } = string.Empty;
    public long Size_Bytes { get; private set; } = 0;

    public JsonDocument Metadata { get; private set; } = JsonDocument.Parse("{}");
    public MessageStatus Status { get; private set; }

    public Message_Content(TimeUuid messageId, ContentType contentType, string contentText, string contentURL, string mimeType, long size_Bytes, JsonDocument metadata, MessageStatus status)
    {
        MessageId = messageId;
        ContentType_ = contentType;
        ContentText = contentText;
        ContentURL = contentURL;
        MimeType = mimeType;
        Size_Bytes = size_Bytes;
        Metadata = metadata;
        Status = status;
    }

}