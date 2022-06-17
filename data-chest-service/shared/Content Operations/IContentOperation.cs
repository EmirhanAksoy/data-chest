using domain.Models;

namespace shared.Content_Operations
{
    public interface IContentOperation
    {
        Task CreateContent(Content content, string parentPath = "");

        Task DeleteContent(string path,bool includeSubItems = true);

    }
}
