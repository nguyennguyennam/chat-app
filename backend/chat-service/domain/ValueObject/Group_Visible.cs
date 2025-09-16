namespace chat_service.domain.ValueObject;

/*
    Group visibility value object
    Public: anyone can join with no keys
    Private: restricted, need a key to join
*/
public enum GroupVisibleEnum
{
    Public,
    Private
}

public class Group_Visible
{
    public GroupVisibleEnum Value { get; private set; } 
    public Group_Visible (string visibility)
    {
        if (Enum.TryParse<GroupVisibleEnum>(visibility, true, out var result))
        {
            Value = result;
        }
        else
        {
            // Default to Public if invalid input
            Value = GroupVisibleEnum.Public;
            throw new ArgumentException($"Invalid group visibility: {visibility}");
        }
    }
}