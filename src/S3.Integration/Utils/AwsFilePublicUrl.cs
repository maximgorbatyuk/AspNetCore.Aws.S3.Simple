using S3.Integration.Settings;

namespace S3.Integration.Utils;

// URL patterns: Virtual hosted style and path style
// Virtual hosted style
// 1. https://[bucketName].[regionName].amazonaws.com/[key]
// 2. https://[bucketName].s3.amazonaws.com/[key]
public readonly struct AwsFilePublicUrl
{
    private readonly string _publicUrl;

    public AwsFilePublicUrl(
        S3StorageSettings settings, string uniqueStorageKey, bool useRegion = true)
    {
        _publicUrl = useRegion ? $"https://{settings.BucketName}.{settings.Region}.{settings.S3BaseDomain}/{uniqueStorageKey}" : $"https://{settings.BucketName}.{settings.S3BaseDomain}/{uniqueStorageKey}";
    }

    public override string ToString()
    {
        return _publicUrl;
    }
}