using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unit_test.Mock.Theory
{
    public static class Contents
    {
        public static Content GetContent(bool isFolder)
        {
            return new Content()
            {
                Id = Guid.NewGuid(),
                Name = isFolder ? "Docs Folder" :  "Test.png",
                CreatedTime = DateTime.Now,
                CreatedUserId = Guid.NewGuid(),
                Extension = ".png",
                IsDeleted = false,
                IsFolder = isFolder,
                IsActive = true,
                IsInTrash = false,
                IsPublic = false,
                ParentContentId = Guid.Empty,
                Path = String.Empty,
                Size = 0,
                ThumbnailPath = String.Empty,
                UpdatedUserId = Guid.Empty,
                UpdatedTime = DateTime.Now
            };
        }
    }
}
