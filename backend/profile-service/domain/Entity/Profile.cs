namespace profile_service.domain.entity;

using System.Text.RegularExpressions;

/**
    Represens a user profile in the system, inclduing basic information.
**/

public class Profile
{
    public Guid ProfileId { get; private set; }
    public Guid UserId { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string AvatarUrl { get; private set; } = string.Empty;
    public string NickName { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdateAt { get; private set; }

    public Profile(Guid profileId, Guid userId, string username, string nickname, string avatarUrl)
    {
        ProfileId = profileId;
        UserId = userId;
        Username = username;
        NickName = nickname;
        CreatedAt = DateTime.UtcNow;
        UpdateAt = DateTime.UtcNow;
        AvatarUrl = avatarUrl;
    }

    public void UpdateProfile(string username, string nickname, string avatarUrl)
    {
        Username = username;
        NickName = nickname;
        AvatarUrl = avatarUrl;
        UpdateAt = DateTime.UtcNow;
    }
}