using domain.Models;

namespace shared.Content_Operations
{
    public class ContentOperation : IContentOperation
    {
        public string MainPath { get; set; }

        public async Task CreateContent(Content content, Stream stream, string parentPath = "")
        {
            string userRootDirectory = Path.Join(MainPath, content.CreatedUserId.ToString(), parentPath);

            Directory.CreateDirectory(userRootDirectory);

            string contentPath = Path.Join(userRootDirectory, content.Name);

            if (content.IsFolder)
                Directory.CreateDirectory(contentPath);
            else
            {
                if (stream is null)
                    throw new ArgumentNullException("Stream");

                using FileStream fileStream = new(contentPath, FileMode.Create, FileAccess.Write);
                await stream.CopyToAsync(fileStream);
            }

            content.Path = contentPath;
        }

        public void DeleteContent(string path, bool isFolder, bool includeSubItems = true)
        {
            if (isFolder)
            {
                if (!Directory.Exists(path))
                    throw new DirectoryNotFoundException(path);

                Directory.Delete(path, includeSubItems);
            }
            else
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException(path);

                File.Delete(path);
            }

        }
    }
}
