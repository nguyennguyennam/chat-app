namespace chat_service.domain.ValueObject;
/*
    Identify the chat type: 1-1 or a group chat.
*/
public enum GroupTypeEnum
{
    Single,
    Group
}

public class Group_Type
{
    public GroupTypeEnum  Value { get; private set; }
    public Group_Type (string type) {
        if (Enum.TryParse<GroupTypeEnum>(type, true, out var result))
        {
            Value = result;
        }
        else
        {
            throw new ArgumentException($"Invalid group type: {type}");
        }
    }
}