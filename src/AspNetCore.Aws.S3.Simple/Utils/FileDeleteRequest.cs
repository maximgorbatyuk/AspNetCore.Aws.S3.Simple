using System.Net;
using Amazon.S3.Model;

namespace AspNetCore.Aws.S3.Simple.Utils;

public class FileDeleteRequest : DeleteObjectRequest
{
    public FileDeleteRequest(string uniqueStorageName, string bucketName)
    {
        BucketName = bucketName;
        Key = WebUtility.HtmlDecode(uniqueStorageName).ToLowerInvariant();
    }
}