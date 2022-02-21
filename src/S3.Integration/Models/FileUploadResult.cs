using System;
using S3.Integration.Contracts;

namespace S3.Integration.Models;

public record FileUploadResult : Optional<string>
{
    public FileUploadResult(string uniqueStorageName, Exception ex)
        : base(uniqueStorageName, ex)
    {
    }

    public override bool Success => ThrownError is null && !string.IsNullOrEmpty(Result);
}