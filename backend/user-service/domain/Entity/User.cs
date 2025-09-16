namespace user_service.domain.Entity;
using user_service.domain.ValueObject;
public class User
{
    public Guid UserId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;

    public User(Guid userId, Email email, Password password)
    {
        UserId = userId;
        Email = email.Value;
        Password = password.Value;
    }
    
    public void ChangePassword (Password newPassword)
    {
        Password = newPassword.Value;
    }
}
