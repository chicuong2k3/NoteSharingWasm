namespace SharingNote.Api.GraphQL.Types
{
    public class UserActivity
    {
        public Guid UserId { get; set; }
        [UseToUpper]
        public string Email { get; set; } = string.Empty;
        public int TotalPosts { get; set; }
        public int TotalComments { get; set; }
    }
}
