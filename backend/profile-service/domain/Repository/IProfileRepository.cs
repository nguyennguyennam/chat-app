using profile_service.domain.entity;
namespace profile_service.domain.repository;

public interface IProfileRepository
{
    
    Task<Profile> DisplayProfile(string email, string password);
    Task<Profile> EditProfile(Profile profile);
    Task<List<Profile?>> FriendsNetwork(Guid ProfileId);
}