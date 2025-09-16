namespace chat_service.domain.Entity;
using chat_service.domain.ValueObject;

public class Permission
{
    public Guid PermissionId { get; private set; }
    public PermissionType PermissionType { get; private set; }

    public Permission(Guid permissionId, PermissionType permissionType)
    {
        PermissionId = permissionId;
        PermissionType = permissionType;
    }

    public override string ToString() => PermissionType.ToString();
}
