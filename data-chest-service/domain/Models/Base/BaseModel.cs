namespace domain.Models.Base
{
    public class BaseModel
    {
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public Guid CreatedUserId { get; set; }

        public Guid UpdatedUserId { get; set; }
    }
}
