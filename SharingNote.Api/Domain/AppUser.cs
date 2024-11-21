namespace SharingNote.Api.Domain
{
    public class AppUser : IdentityUser<Guid>
    {
        public string DisplayName { get; private set; } = string.Empty;
        public string Avatar { get; private set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryDate { get; set; }

        private AppUser()
        {

        }

        public AppUser(string email)
        {
            Email = email;
            UserName = email;
            DisplayName = email;
        }
        public void UpdateInfo(string displayName, string avatar)
        {
            DisplayName = displayName;
            Avatar = avatar;
        }
    }
}
