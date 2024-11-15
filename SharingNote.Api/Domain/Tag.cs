namespace SharingNote.Api.Domain
{
    public sealed class Tag
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        private List<Post> _posts = [];
        public IReadOnlyCollection<Post> Posts => _posts;

        public Guid UserId { get; private set; }
        private Tag() { }
        public Tag(string name, Guid userId)
        {
            Id = Guid.NewGuid();
            Name = name;
            UserId = userId;
        }
    }
}
