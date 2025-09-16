namespace chat_service.domain.ValueObject;

/*
    This class repesents the role name of a user in a group chat.
*/
public enum RoleType
{
    Admin,
    Member,
    Default //For 1-1 chat, the other user is a guest.
}
public class RoleName
{
    public RoleType Value { get; private set; }
    public RoleName (string role) {
        if (Enum.TryParse<RoleType>(role, true, out var result))
        {
            Value = result;
        }
        else
        {
            throw new ArgumentException($"Invalid role name: {role}");
        }
    }
}