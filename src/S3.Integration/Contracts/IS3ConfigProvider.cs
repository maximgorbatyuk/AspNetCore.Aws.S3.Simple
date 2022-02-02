using Amazon.S3;

namespace S3.Integration.Contracts;

public interface IS3ConfigProvider
{
    AmazonS3Config Config();
}