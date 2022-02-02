using System;
using Amazon.Runtime;

namespace S3.Integration.Exceptions;

public class FileDoesNotExistException : InvalidOperationException
{
    public FileDoesNotExistException(
        string fileName, string uniqueStorageName, string bucket, AmazonServiceException exception)
        : base($"File {fileName} ({uniqueStorageName}) does not exist in bucket {bucket}", exception)
    {
    }
}