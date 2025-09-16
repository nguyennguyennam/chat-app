namespace chat_service.domain_Entity;

public class Role_Permissions
{
    public Guid RoleId { get; private set; }
    public Guid PermissionId { get; private set; }

    public Role_Permissions (Guid roleId, Guid permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }
}