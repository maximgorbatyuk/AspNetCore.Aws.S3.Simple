using System.Net;
using Amazon.S3.Model;

namespace S3.Integration.Utils;

public class FileDeleteRequest : DeleteObjectRequest
{
    public FileDeleteRequest(string uniqueStorageName, string bucketName)
    {
        BucketName = bucketName;
        Key = WebUtility.HtmlDecode(uniqueStorageName).ToLowerInvariant();
    }
}