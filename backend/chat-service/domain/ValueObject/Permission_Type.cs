namespace chat_service.domain.ValueObject;

/// <summary>
/// List of permissions available in the chat system.
/// </summary>
public enum PermissionType
{
    // Messaging
    SendMessage,
    RemoveMessage,
    EditMessage,
    ReactMessage,        // add reactions (emoji, like, etc.)
    ReplyMessage,        // reply to a message
    ForwardMessage,      // forward a message

    // Group management
    AddMember,
    RemoveMember,
    InviteMember,        // invite via link
    BanMember,           // ban a user
    MuteMember,          // mute a user

    // Group settings
    PinMessage,
    ChangeGroupName,
    ChangeAvatar,
    ChangeGroupVisibility,  // toggle private/public
    ChangeGroupType,        // switch between 1-1 and group

    // Role & permission management
    AssignRole,
    ManagePermissions,

    // Miscellaneous
    ShareFile,
    ShareMedia,
    DeleteGroup,
    LeaveGroup
}

/// <summary>
/// Value Object for Permission. 
/// Wraps the PermissionType enum to protect domain invariants and ensure type safety.
/// </summary>
public class Permission
{
    public PermissionType Value { get; private set; }

    public Permission(PermissionType value)
    {
        Value = value;
    }

    public Permission(string permission)
    {
        if (Enum.TryParse<PermissionType>(permission, true, out var result))
        {
            Value = result;
        }
        else
        {
            throw new ArgumentException($"Invalid permission: {permission}");
        }
    }

    public override string ToString() => Value.ToString();

    public override bool Equals(object? obj)
    {
        if (obj is Permission other)
        {
            return Value == other.Value;
        }
        return false;
    }

    public override int GetHashCode() => Value.GetHashCode();
}
