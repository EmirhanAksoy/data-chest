using domain.Models;
using shared.Content_Operations;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using System;
using System.Diagnostics;

namespace unit_test
{
    public class ContentCreationTest
    {
        IContentOperation contentOperation = new ContentOperation();

        public readonly Content contentFile;
        public readonly Content contentFolder;
        public readonly FileStream contentStream;

        public ContentCreationTest()
        {
            contentOperation.MainPath = @"C:\Contents";

            contentFile = new Content()
            {
                Id = Guid.NewGuid(),
                Name = "Test.png",
                CreateTime = DateTime.Now,
                CreatedUserId = Guid.NewGuid(),
                Extension = ".png",
                IsDeleted = false,
                IsFolder = false,
                IsHidden = false,
                IsInBin = false,
                IsPublic = false,
                ParentContentId = Guid.Empty,
                Path = String.Empty,
                Size = 0,
                ThumbnailPath = String.Empty,
                UpdatedUserId = Guid.Empty,
                UpdateTime = DateTime.Now
            };

            contentFolder = new Content()
            {
                Id = Guid.NewGuid(),
                Name = "Docs Test Folder",
                CreateTime = DateTime.Now,
                CreatedUserId = Guid.NewGuid(),
                Extension = String.Empty,
                IsDeleted = false,
                IsFolder = true,
                IsHidden = false,
                IsInBin = false,
                IsPublic = false,
                ParentContentId = Guid.Empty,
                Path = String.Empty,
                Size = 0,
                ThumbnailPath = String.Empty,
                UpdatedUserId = Guid.Empty,
                UpdateTime = DateTime.Now
            };

            FileStream fs = File.OpenRead(@"C:\TestContent\test.jpg");
            contentStream = fs;

        }


        [Theory(DisplayName = "Create File Content")]
        [InlineData("")]
        [InlineData(@"\Files")]
        [InlineData(null)]
        [InlineData("0123456789")]
        [InlineData("c188d074-a4d8-4105-9bc3-9fb2dddf08dd")]
        public async Task CreateFileContent(string parentPath)
        {
            await contentOperation.CreateContent(contentFile, contentStream, parentPath);

            bool isFileCreated = File.Exists(Path.Join(contentOperation.MainPath, contentFile.CreatedUserId.ToString(), parentPath, contentFile.Name));

            Assert.True(isFileCreated, $"{contentFile.Name} created succesfully");
        }


        [Fact(DisplayName = "Create Folder Content")]
        public async Task CreateFolderContentInRoot()
        {
            string parentPath = string.Empty;

            await contentOperation.CreateContent(contentFolder, contentStream, parentPath);

            bool isFolderCreated = Directory.Exists(Path.Join(contentOperation.MainPath, contentFolder.CreatedUserId.ToString(), parentPath, contentFolder.Name));

            Assert.True(isFolderCreated, $"{contentFolder.Name} created succesfully");
        }



    }
}