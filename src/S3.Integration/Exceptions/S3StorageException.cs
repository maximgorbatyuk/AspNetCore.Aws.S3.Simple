using System;
using System.Net;
using Amazon.Runtime;

namespace S3.Integration.Exceptions;

public class S3StorageException : InvalidOperationException
{
    public HttpStatusCode StatusCode { get; }

    public string ServiceErrorMessage { get; }

    public S3StorageException(
        AmazonServiceException amazonS3Exception)
        : this(
            "Error during request to Amazon S3",
            amazonS3Exception)
    {
    }

    public S3StorageException(
        string message,
        AmazonServiceException amazonS3Exception)
        : base(
            message,
            amazonS3Exception)
    {
        StatusCode = amazonS3Exception.StatusCode;
        ServiceErrorMessage = amazonS3Exception.Message;
    }
}