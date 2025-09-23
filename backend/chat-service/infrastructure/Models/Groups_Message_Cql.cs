using Cassandra.Mapping.Attributes;

namespace chat_service.infrastructure.Models;

/*
    Persistence model: map thẳng tới bảng Cassandra
    message_content_by_group
*/
[Table("message_content_by_group")]
public class Groups_Message_Cql
{
    [Column("group_id")] public Guid GroupId { get; set; }
    [Column("sender_id")] public Guid SenderId { get; set; }
    [Column("sent_at")] public DateTime SentAt { get; set; }

    [Column("message_id")] public Guid MessageId { get; set; }
    [Column("content_type")] public string ContentType { get; set; } = string.Empty;
    [Column("content_text")] public string ContentText { get; set; } = string.Empty;
    [Column("content_url")] public string ContentUrl { get; set; } = string.Empty;
    [Column("mime_type")] public string MimeType { get; set; } = string.Empty;
    [Column("size_bytes")] public long SizeBytes { get; set; }
    [Column("metadata")] public string Metadata { get; set; } = "{}";
    [Column("status")] public string Status { get; set; } = "Sent";
}
