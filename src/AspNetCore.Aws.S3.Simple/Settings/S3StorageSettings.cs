using System;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using HealthChecks.Aws.S3;
using Microsoft.Extensions.Configuration;

namespace AspNetCore.Aws.S3.Simple.Settings;

public class S3StorageSettings
{
    public const string BucketNameConfigKey = "S3:BucketName";
    public const string AccessKeyConfigKey = "S3:AccessKey";
    public const string SecretKeyConfigKey = "S3:AccessSecret";
    public const string RegionConfigKey = "S3:Region";
    public const string BaseDomainConfigKey = "S3:S3BaseDomain";
    public const string AllowedFileExtensionsConfigKey = "S3:AllowedFileExtensions";
    public const string AllowedFileSizeInMbConfigKey = "S3:AllowedFileSizeInMb";
    public const string CloudStorageUrlConfigKey = "S3:CloudStorageUrl";

    public S3StorageSettings(IConfiguration configuration)
    {
        BucketName = GetConfigValueOrFail(configuration, BucketNameConfigKey);
        AllowedFileExtensions = GetConfigValueOrFail(configuration, AllowedFileExtensionsConfigKey);
        AllowedFileSizeInMb = int.Parse(GetConfigValueOrFail(configuration, AllowedFileSizeInMbConfigKey));
        AccessKey = GetConfigValueOrFail(configuration, AccessKeyConfigKey);
        AccessSecret = GetConfigValueOrFail(configuration, SecretKeyConfigKey);
        Region = GetConfigValueOrFail(configuration, RegionConfigKey, true);
        S3BaseDomain = GetConfigValueOrFail(configuration, BaseDomainConfigKey);
        CloudStorageUrl = GetConfigValueOrFail(configuration, CloudStorageUrlConfigKey, true);
    }

    public string BucketName { get; }

    public int AllowedFileSizeInMb { get; }

    public string AllowedFileExtensions { get; }

    public string AccessKey { get; }

    public string AccessSecret { get; }

    public string Region { get; }

    public string S3BaseDomain { get; }

    public string CloudStorageUrl { get; }

    private BasicAWSCredentials GetCredentials() => new (AccessKey, AccessSecret);

    private AmazonS3Config AmazonS3Config()
    {
        var config = new AmazonS3Config
        {
            ServiceURL = CloudStorageUrl,
            ForcePathStyle = true,
        };

        if (!string.IsNullOrEmpty(Region))
        {
            config.RegionEndpoint = RegionEndpoint.GetBySystemName(Region);
        }

        return config;
    }

    public IAmazonS3 CreateClient()
        => new AmazonS3Client(GetCredentials(), AmazonS3Config());

    public void SetupS3HealthCheck(
        S3BucketOptions options)
    {
        options.BucketName = BucketName;
        options.AccessKey = AccessKey;
        options.SecretKey = AccessSecret;
        options.S3Config = AmazonS3Config();
    }

    private static string GetConfigValueOrFail(
        IConfiguration configuration, string configKey, bool optional = false)
    {
        var value = configuration[configKey];
        if (!optional && string.IsNullOrEmpty(value))
        {
            throw new ArgumentException($"The setting [{configKey}] does not exist");
        }

        return value;
    }
}