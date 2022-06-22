using domain.Models;

namespace shared.Content_Operations
{
    public interface IContentOperation
    {
        string MainPath {get; set;}

        Task CreateContent(Content content, Stream stream, string parentPath = "");

        void DeleteContent(string path, bool isFolder, bool includeSubItems = true);

    }
}

