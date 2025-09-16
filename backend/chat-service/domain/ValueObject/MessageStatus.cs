namespace chat_service.domain.ValueObject;

public enum MessageStatusEnum
{
    Sent,
    Delivered,
    Seen
}

public class MessageStatus
{
    public MessageStatusEnum Value { get; private set; } 
    public MessageStatus(string status)
    {
        if (Enum.TryParse<MessageStatusEnum>(status, true, out var result))
        {
            Value = result;
        }
        else
        {
            throw new ArgumentException($"Invalid message status: {status}");
        }
    }

}
