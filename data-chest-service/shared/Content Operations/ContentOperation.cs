using domain.Models;

namespace shared.Content_Operations
{
    public class ContentOperation : IContentOperation
    {
        public Task CreateContent(Content content,string parentPath = "")
        {
            throw new NotImplementedException();
        }

        public Task DeleteContent(string path,bool includeSubItems = true)
        {
            throw new NotImplementedException();
        }
    }
}
