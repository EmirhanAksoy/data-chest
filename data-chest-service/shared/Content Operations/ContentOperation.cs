using domain.Models;

namespace shared.Content_Operations
{
    public class ContentOperation : IContentOperation
    {
        public string MainPath { get; set; }

        public async Task CreateContent(Content content, Stream stream, string parentPath = "")
        {
            string userRootDirectory = Path.Join(MainPath, content.CreatedUserId.ToString());

            Directory.CreateDirectory(userRootDirectory);

            string contentPath = Path.Join(userRootDirectory, parentPath, content.Name);

            if (content.IsFolder)
                Directory.CreateDirectory(contentPath);
            else 
            {
                using FileStream fileStream = new(contentPath, FileMode.Create, FileAccess.Write);
                await stream.CopyToAsync(fileStream);
            }
        }

        public void DeleteContent(string path, bool isFolder, bool includeSubItems = true)
        {
            if (isFolder && Directory.Exists(path))
            {
                Directory.Delete(path, includeSubItems);
                return;
            }

            File.Delete(path);
        }
    }
}
