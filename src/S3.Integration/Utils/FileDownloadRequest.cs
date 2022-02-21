using System.Net;
using Amazon.S3.Model;

namespace S3.Integration.Utils;

public class FileDownloadRequest : GetObjectRequest
{
    public FileDownloadRequest(
        string uniqueStorageName,
        string bucketName)
    {
        BucketName = bucketName;
        Key = WebUtility.HtmlDecode(uniqueStorageName).ToLowerInvariant();
    }
}