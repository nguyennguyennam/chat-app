/*
    Wrap input from Presentation Layer to a Data Object for better control
*/
namespace chat_service.application.DTOs
{
    public class MessageDto
    {
        public Guid MessageId { get; set; }
        public Guid GroupId { get; set; }
        public Guid SenderId { get; set; }
        public DateTime SentAt { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string ContentText { get; set; } = string.Empty;
        public string ContentUrl { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public long SizeBytes { get; set; }
        public string Status { get; set; } = "Sent";
    }

    public class SendMessageRequestDto
    {
        public Guid GroupId { get; set; }
        public Guid SenderId { get; set; }
        public string ContentText { get; set; } = string.Empty;
        public string ContentType { get; set; } = "Text";
        public string MimeType { get; set; } = "text/plain";
        public string? ContentUrl { get; set; }
        public long SizeBytes { get; set; } = 0;
    }
}
