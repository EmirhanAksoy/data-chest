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
        private readonly IContentOperation contentOperation = new ContentOperation();

        public readonly FileStream contentStream;

        public ContentCreationUnitTest()
        {
            contentOperation.MainPath = @"C:\Contents";

            FileStream fs = File.OpenRead(@"C:\TestContent\test.jpg");
            contentStream = fs;
        }


        [Theory(DisplayName = "Create File Content - Valid Stream")]
        [InlineData("")]
        [InlineData(@"\Files")]
        [InlineData(null)]
        [InlineData("0123456789")]
        [InlineData("c188d074-a4d8-4105-9bc3-9fb2dddf08dd")]
        [InlineData(@"\\\\\\Docs")]

        public async Task CreateFileContentWithValidStream(string parentPath)
        {
            Content content = Contents.GetContent(isFolder: false);

            await contentOperation.CreateContent(content, contentStream, parentPath);

            bool isContentCreated = File.Exists(Path.Join(contentOperation.MainPath, content.CreatedUserId.ToString(), parentPath, content.Name)); ;

            Assert.True(isContentCreated, $"{content.Name} created succesfully");
        }

        [Theory(DisplayName = "Create Folder Content - Valid Stream")]
        [InlineData("")]
        [InlineData(@"\Files")]
        [InlineData(null)]
        [InlineData("0123456789")]
        [InlineData("c188d074-a4d8-4105-9bc3-9fb2dddf08dd")]
        [InlineData(@"\\\\\\Docs")]

        public async Task CreateFolderContentWithValidStream(string parentPath)
        {
            Content content = Contents.GetContent(isFolder: true);

            await contentOperation.CreateContent(content, contentStream, parentPath);

            bool isContentCreated = Directory.Exists(Path.Join(contentOperation.MainPath, content.CreatedUserId.ToString(), parentPath, content.Name)); ;

            Assert.True(isContentCreated, $"{content.Name} created succesfully");
        }

        [Theory(DisplayName = "Create File Content - Invalid Stream")]
        [InlineData("")]
        [InlineData(@"\Files")]
        [InlineData(null)]
        [InlineData("0123456789")]
        [InlineData("c188d074-a4d8-4105-9bc3-9fb2dddf08dd")]
        [InlineData(@"\\\\\\Docs")]

        public async Task CreateFileContentWithInvalidStream(string parentPath)
        {
            Content content = Contents.GetContent(isFolder:false);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await contentOperation.CreateContent(content, null, parentPath));

        }

        [Theory(DisplayName = "Create Folder Content - Invalid Stream")]
        [InlineData("")]
        [InlineData(@"\Files")]
        [InlineData(null)]
        [InlineData("0123456789")]
        [InlineData("c188d074-a4d8-4105-9bc3-9fb2dddf08dd")]
        [InlineData(@"\\\\\\Docs")]

        public async Task CreateFolderContentWithInvalidStream(string parentPath)
        {
            Content content = Contents.GetContent(isFolder: true);

            await contentOperation.CreateContent(content, null, parentPath);

            bool isContentCreated = Directory.Exists(Path.Join(contentOperation.MainPath, content.CreatedUserId.ToString(), parentPath, content.Name));

            Assert.True(isContentCreated, $"{content.Name} created succesfully");
        }

    }
}