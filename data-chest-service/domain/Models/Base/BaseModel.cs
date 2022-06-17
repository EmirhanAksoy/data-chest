namespace domain.Models.Base
{
    public class BaseModel
    {
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; } = false;

        public bool IsHidden { get; set; } = false;

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public Guid CreatedUserId { get; set; }

        public Guid UpdatedUserId { get; set; }
    }
}
