using Amazon.S3;
using S3.Integration.Contracts;
using S3.Integration.Settings;

namespace S3.Integration.AmazonServices;

public class AmazonS3ConfigProvider : IS3ConfigProvider
{
    private readonly S3StorageSettings _s3StorageSettings;

    public AmazonS3ConfigProvider(S3StorageSettings s3StorageSettings)
    {
        _s3StorageSettings = s3StorageSettings;
    }

    public AmazonS3Config Config()
    {
        return _s3StorageSettings.AmazonS3Config();
    }
}