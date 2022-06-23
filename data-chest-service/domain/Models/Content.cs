using domain.Models.Base;

namespace domain.Models
{
    public class Content : BaseModel
    {
        public string Name { get; set; }
        public bool IsFolder { get; set; }
        public string Path { get; set; }
        public int Size { get; set; }
        public string ThumbnailPath { get; set; }
        public Guid ParentContentId { get; set; } = Guid.Empty;
        public string Extension { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = false;
        public bool IsInTrash { get; set; } = false;
    }
}
