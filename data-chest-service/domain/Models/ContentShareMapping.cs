namespace domain.Models
{
    public class ContentShareMapping
    {
        public Guid Id { get; set; }

        public Guid ContentId { get; set; }

        public Guid SharedUserId { get; set; }

        public DateTime ExpireDate { get; set; }

        public bool IsExpired { get; set; }
    }
}
