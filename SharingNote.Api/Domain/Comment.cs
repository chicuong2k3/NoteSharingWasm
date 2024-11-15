namespace SharingNote.Api.Domain
{
    public class Comment
    {
        public Guid Id { get; private set; }
        public Guid PostId { get; private set; }
        public Guid UserId { get; private set; }
        public string Content { get; private set; } = string.Empty;
        public DateTime PostedDate { get; private set; }
        public DateTime? LastModifiedDate { get; private set; }
        public Guid? ParentCommentId { get; set; }

        private Comment() { }

        public Comment(Guid postId, Guid userId, string content, Guid? parentId)
        {
            Id = Guid.NewGuid();
            PostId = postId;
            UserId = userId;
            Content = content;
            PostedDate = DateTime.UtcNow;
            ParentCommentId = parentId;
        }

        public void Update(string content)
        {
            Content = content;
            LastModifiedDate = DateTime.UtcNow;
        }
    }
}
