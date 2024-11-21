using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharingNote.Api.Domain
{
    public class Post
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Content { get; private set; } = string.Empty;
        [NotMapped]
        public string ShortDescription => Content.Length > 20 ? Content.Substring(0, 20) : Content;
        public DateTime PublicationDate { get; private set; }
        private List<Tag> _tags = [];
        public IReadOnlyCollection<Tag> Tags => _tags;

        public Guid UserId { get; private set; }

        private List<PostInteraction> _interactions = [];
        public IReadOnlyCollection<PostInteraction> Interactions => _interactions;


        [Timestamp]
        public byte[] RowVersion { get; set; } = [];
        private Post()
        {

        }

        public Post(string title, string content, List<Tag> tags, Guid userId)
        {
            Id = Guid.NewGuid();
            Title = title;
            Content = content;

            _tags.AddRange(tags);

            PublicationDate = DateTime.UtcNow;

            UserId = userId;
        }

        public void Update(string title, string content, List<Tag> tags)
        {
            Title = title;
            Content = content;
            _tags.Clear();
            _tags.AddRange(tags);
        }

    }
}
