namespace chat_service.domain.Entity;

/*
    This class represents the abstract the information of a message sent in a group chat.
*/
public class Groups_Message
{
    public Guid GroupId { get; private set; }
    public Guid MessageId { get; private set; }
    public Guid SenderId { get; private set; }
    public DateTime SentAt { get; private set; }

    public Groups_Message (Guid groupId, Guid messageId, Guid senderId)
    {
        GroupId = groupId;
        MessageId = messageId;
        SenderId = senderId;
        SentAt = DateTime.UtcNow;
    }
}