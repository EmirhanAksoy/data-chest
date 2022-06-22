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
    public class ContentCreationUnitTest
    {
        IContentOperation contentOperation = new ContentOperation();

        public readonly FileStream contentStream;

        public ContentCreationUnitTest()
        {
            contentOperation.MainPath = @"C:\Contents";

            FileStream fs = File.OpenRead(@"C:\TestContent\test.jpg");
            contentStream = fs;
        }


        [Theory(DisplayName = "Create File Content - Valid Stream")]
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

        public async Task CreateContentWithValidStream(string parentPath, bool isFolder)
        {
            Content content = Contents.GetContent(isFolder);

            await contentOperation.CreateContent(content, contentStream, parentPath);

            bool isContentCreated = false;

            if (isFolder)
                isContentCreated = Directory.Exists(Path.Join(contentOperation.MainPath, content.CreatedUserId.ToString(), parentPath, content.Name));
            else
                isContentCreated = File.Exists(Path.Join(contentOperation.MainPath, content.CreatedUserId.ToString(), parentPath, content.Name));

            Assert.True(isContentCreated, $"{content.Name} created succesfully");
        }

        [Theory(DisplayName = "Create File Content - Invalid Stream")]
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

        public async Task CreateContentWithInvalidStream(string parentPath, bool isFolder)
        {
            Content content = Contents.GetContent(isFolder);

            if (isFolder)
            {
                await contentOperation.CreateContent(content, null, parentPath);

                bool isContentCreated = Directory.Exists(Path.Join(contentOperation.MainPath, content.CreatedUserId.ToString(), parentPath, content.Name));

                Assert.True(isContentCreated, $"{content.Name} created succesfully");
            }
            else
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await contentOperation.CreateContent(content, null, parentPath));

        }

    }
}