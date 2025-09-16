using System.Text.Json;
using chat_service.domain.ValueObject;
namespace chat_service.domain.Entity;

/*
    This class represents the detailed content of a message sent in a chat
*/
public class Message_Content
{
    public Guid MessageId { get; private set; }
    public ContentType ContentType_ { get; private set; }
    public string ContentText { get; private set; } = string.Empty;
    public string ContentURL { get; private set; } = string.Empty;

    public string MimeType { get; private set; } = string.Empty;
    public long Size_Bytes { get; private set; } = 0;

    public JsonDocument Metadata { get; private set; } = JsonDocument.Parse("{}");

    public Message_Content(Guid messageId, ContentType contentType, string contentText, string contentURL, string mimeType, long size_Bytes, JsonDocument metadata)
    {
        MessageId = messageId;
        ContentType_ = contentType;
        ContentText = contentText;
        ContentURL = contentURL;
        MimeType = mimeType;
        Size_Bytes = size_Bytes;
        Metadata = metadata;
    }

}