namespace chat_service.domain.Entity;
using chat_service.domain.ValueObject;
public class Role
{
    public Guid RoleId { get; private set; }
    public RoleName Rolename_ { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public Role (Guid roleId, RoleName role, string description)
    {
        RoleId = roleId;
        Rolename_ = role;
        Description = description;
    }
}