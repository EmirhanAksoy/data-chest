using domain.Models;
using System;

namespace unit_test.Mock.Theory;

public static class ContentCreator
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
