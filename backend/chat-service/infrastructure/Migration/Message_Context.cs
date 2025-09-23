using chat_service.infrastructure.Utility.Database;
/*
    This file handles the migration of the Message_Content entity to the Cassandra database.
*/
using Cassandra.Mapping;

namespace chat_service.infrastructure.Migration;
public class Message_Context
{
    private readonly CassandraDb _db;
    private readonly IMapper _mapper;
    public Message_Context()
    {
        _db = new CassandraDb();
        _mapper = _db.GetMapper();
        CreateMessageTable();
    }

    //This table is for querying messages by group
    private void CreateMessageTable()
{
    // Create table 
    _db.GetSession().Execute(@"
        CREATE TABLE IF NOT EXISTS message_content_by_group (
            group_id UUID,
            sent_at TIMESTAMP,
            message_id TimeUUID,
            sender_id UUID,
            Content_Type TEXT,
            Content_Text TEXT,
            Content_URL TEXT,
            Mime_Type TEXT,
            Size_Bytes BIGINT,
            Metadata TEXT,
            status TEXT,
            PRIMARY KEY ((group_id), sent_at, message_id)
        ) WITH CLUSTERING ORDER BY (sent_at DESC);
    ");

    // Create index on message content for search functionality
    _db.GetSession().Execute(@"
        CREATE INDEX IF NOT EXISTS idx_content_text 
        ON message_content_by_group (Content_Text)
      ");
    }
}