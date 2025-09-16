using user_service.domain.Entity;

namespace user_service.domain.Repository;
public interface IUserRepository
{
    Task<User>RegisterUser(User profile);
    Task<User?> LoginUser(string email, string password);
    Task LogoutUser(User profile);
    Task<User> ChangePassword(Guid ProfileId, string OldPassword, string newPassword);
}