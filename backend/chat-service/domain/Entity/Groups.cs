namespace chat_service.domain.Entity;
using chat_service.domain.ValueObject;
/*
    This class represents the information of a group chat.
*/

public class Groups
{
    public Guid GroupId { get; private set; }
    public Guid OwnerId { get; private set; } // UserId of the group creator, relating to Profile
    public string GroupName { get; private set; } = string.Empty;
    public DateTime CreateAt { get; private set; }
    public Group_Visible GroupVisible { get; private set; } 
    public Group_Type GroupType { get; private set; }
    public string Group_Key { get; private set; } = string.Empty; // For private group, the key to join the group
    public Groups (Guid groupId, Guid ownerId, string groupName, Group_Visible visibility,Group_Type type, string groupKey)
    {
        GroupId = groupId;
        OwnerId = ownerId;
        GroupName = groupName;
        CreateAt = DateTime.UtcNow;
        GroupVisible = visibility;
        GroupType = type;
        Group_Key = groupKey;
    }
    
}