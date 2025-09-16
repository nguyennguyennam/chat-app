namespace chat_service.domain.Entity;

public class Group_Participants
{
    public Guid GroupId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid RoleId { get; private set; } // Role of the user in the group, relating to Group_Role
    public DateTime JoinedAt { get; private set; }
    
}