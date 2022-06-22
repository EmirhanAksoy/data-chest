using domain.Models;
using shared.Content_Operations;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using System;
using System.Diagnostics;
using unit_test.Mock.Theory;

namespace unit_test
{
    public class ContentCreationTest
    {
        IContentOperation contentOperation = new ContentOperation();

        public readonly FileStream contentStream;

        public ContentCreationTest()
        {
            contentOperation.MainPath = @"C:\Contents";

            FileStream fs = File.OpenRead(@"C:\TestContent\test.jpg");
            contentStream = fs;
        }


        [Theory(DisplayName = "Create File Content")]
        [InlineData("", false)]
        [InlineData(@"\Files", false)]
        [InlineData(null, false)]
        [InlineData("0123456789", false)]
        [InlineData("c188d074-a4d8-4105-9bc3-9fb2dddf08dd", false)]
        [InlineData(@"\\\\\\Docs", false)]
        [InlineData("", true)]
        [InlineData(@"\Files", true)]
        [InlineData(null, true)]
        [InlineData("0123456789", true)]
        [InlineData("c188d074-a4d8-4105-9bc3-9fb2dddf08dd", true)]
        [InlineData(@"\\\\\\Docs", true)]
        [InlineData("", false, true)]
        [InlineData(@"\Files", false, true)]
        [InlineData(null, false, true)]
        [InlineData("0123456789", false, true)]
        [InlineData("c188d074-a4d8-4105-9bc3-9fb2dddf08dd", false, true)]
        [InlineData(@"\\\\\\Docs", false, true)]
        [InlineData("", true, true)]
        [InlineData(@"\Files", true, true)]
        [InlineData(null, true, true)]
        [InlineData("0123456789", true, true)]
        [InlineData("c188d074-a4d8-4105-9bc3-9fb2dddf08dd", true, true)]
        [InlineData(@"\\\\\\Docs", true, true)]
        public async Task CreateContent(string parentPath, bool isFolder, bool isStreamNull = false)
        {
            Content content = Contents.GetContent(isFolder);

            if (isStreamNull && !isFolder)
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await contentOperation.CreateContent(content, null, parentPath));
            else
            {
                await contentOperation.CreateContent(content, isStreamNull ? null : contentStream, parentPath);

                bool isContentCreated = false;

                if (isFolder)
                    isContentCreated = Directory.Exists(Path.Join(contentOperation.MainPath, content.CreatedUserId.ToString(), parentPath, content.Name));
                else
                    isContentCreated = File.Exists(Path.Join(contentOperation.MainPath, content.CreatedUserId.ToString(), parentPath, content.Name));

                Assert.True(isContentCreated, $"{content.Name} created succesfully");
            }
        }

    }
}