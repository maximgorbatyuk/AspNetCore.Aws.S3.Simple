using System;

namespace AspNetCore.Aws.S3.Simple.Models;

public record S3File
{
    public S3File(
        string originalFileName,
        string uniqueStorageName,
        string contentType,
        byte[] content,
        DateTimeOffset lastModifiedAt)
    {
        Content = content;
        OriginalFileName = originalFileName;
        UniqueStorageName = uniqueStorageName;
        LastModifiedAt = lastModifiedAt;
        ContentType = contentType;
    }

    public byte[] Content { get; }

    public string ContentType { get; }

    public string OriginalFileName { get; }

    public string UniqueStorageName { get; }

    public DateTimeOffset LastModifiedAt { get; }
}