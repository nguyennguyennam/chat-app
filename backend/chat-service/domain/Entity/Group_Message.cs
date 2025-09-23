using Cassandra.Mapping.Attributes;

namespace chat_service.domain.Entity
{
    /*
        This class represents the abstract the information of a message sent in a group chat.
    */

    [Table("message_content_by_group")]
    public class Groups_Message
{
    [Column("group_id")]
    public Guid GroupId { get; private set; }

    [Column("sender_id")]
    public Guid SenderId { get; private set; }

    [Column("sent_at")]
    public DateTime SentAt { get; private set; }

    // Map the nested Message_Content properties to appropriate columns via its own mapping
    // The mapper will handle nested objects if configured; keep property non-nullable here.
    public Message_Content Message { get; private set; } = default!;

    public Groups_Message (Guid groupId, Message_Content message , Guid senderId)
    {
        GroupId = groupId;
        Message = message;
        SenderId = senderId;
        SentAt = DateTime.UtcNow;
    }
}
}