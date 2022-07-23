using domain.Models;
using shared.Content_Operations;
using System.IO;
using System.Threading.Tasks;
using unit_test.Mock.Theory;
using Xunit;

namespace unit_test
{
    public class ContentDeleteUnitTest
    {
        private readonly IContentOperation contentOperation = new ContentOperation();

        public readonly FileStream contentStream;

        public ContentDeleteUnitTest()
        {
            contentOperation.MainPath = @"C:\Contents";

            FileStream fs = File.OpenRead(@"C:\TestContent\test.jpg");
            contentStream = fs;
        }


        [Theory(DisplayName = "Delete File Content - Valid Path")]
        [InlineData(false)]
        [InlineData(true)]
        public async Task DeleteFileContentValidPath(bool includeSubItems)
        {
            Content content = ContentCreator.GetContent(isFolder: false);

            await contentOperation.CreateContent(content, contentStream, string.Empty);

            string createdContentPath = content.Path;

            contentOperation.DeleteContent(createdContentPath, content.IsFolder, includeSubItems);

            bool isContentDeleted = !File.Exists(createdContentPath);

            Assert.True(isContentDeleted, $"{content.Name} deleted succesfully");
        }

        [Theory(DisplayName = "Delete Folder Content - Valid Path")]
        [InlineData(false)]
        [InlineData(true)]
        public async Task DeteleFolderContentValidPath(bool includeSubItems)
        {
            Content content = ContentCreator.GetContent(isFolder: false);

            string parentFolder = "Files";

            await contentOperation.CreateContent(content, contentStream, parentFolder);

            string createdContentPath = Path.Join(contentOperation.MainPath, content.CreatedUserId.ToString(), parentFolder);

            if (includeSubItems)
            {
                contentOperation.DeleteContent(createdContentPath, isFolder: true, includeSubItems);

                bool isContentDeleted = !File.Exists(createdContentPath);

                Assert.True(isContentDeleted, $"{content.Name} deleted succesfully");
            }
            else
                Assert.Throws<IOException>(() => contentOperation.DeleteContent(createdContentPath, isFolder: true , includeSubItems));
        }

        [Theory(DisplayName = "Delete File Content - Invalid Path")]
        [InlineData(false,null)]
        [InlineData(true,null)]
        [InlineData(false,"")]
        [InlineData(true,"")]
        [InlineData(false, "xxxxxxxxxxxxxx")]
        [InlineData(true, "xxxxxxxxxxxxxx")]
        public void DeleteFileContentInvalidPath(bool includeSubItems,string path)
        {
            Assert.Throws<FileNotFoundException>(() => contentOperation.DeleteContent(path, isFolder: false, includeSubItems));
        }

        [Theory(DisplayName = "Delete File Content - Invalid Path")]
        [InlineData(false, null)]
        [InlineData(true, null)]
        [InlineData(false, "")]
        [InlineData(true, "")]
        [InlineData(false, "xxxxxxxxxxxxxx")]
        [InlineData(true, "xxxxxxxxxxxxxx")]
        public void DeteleFolderContentInvalidPath(bool includeSubItems, string path)
        {
            Assert.Throws<DirectoryNotFoundException>(() => contentOperation.DeleteContent(path, isFolder: true, includeSubItems));
        }
    }



}
