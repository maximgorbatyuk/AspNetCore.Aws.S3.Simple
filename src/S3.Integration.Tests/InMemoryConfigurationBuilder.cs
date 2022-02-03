using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using S3.Integration.Settings;

namespace S3.Integration.Tests;

public class InMemoryConfigurationBuilder
{
    private readonly IDictionary<string, string> _dictionary;

    public InMemoryConfigurationBuilder(IDictionary<string, string>? dictionary = null)
    {
        _dictionary = dictionary ?? new Dictionary<string, string>();
    }

    public InMemoryConfigurationBuilder Add(string key, string value)
    {
        _dictionary.Add(key, value);
        return this;
    }

    public InMemoryConfigurationBuilder AddS3Settings(
        string bucketName,
        string region = "eu-central-1",
        string allowedExtensions = "jpg,jpeg,png,pdf",
        int allowedFileSizeInMb = 6,
        string baseDomain = "amazonaws.com",
        string accessKey = "access",
        string accessSecret = "access-secret",
        string localS3Url = "http://localhost:9000")
    {
        _dictionary.Add(S3StorageSettings.AccessKeyConfigKey, accessKey);
        _dictionary.Add(S3StorageSettings.SecretKeyConfigKey, accessSecret);
        _dictionary.Add(S3StorageSettings.BucketNameConfigKey, bucketName);
        _dictionary.Add(S3StorageSettings.RegionConfigKey, region);
        _dictionary.Add(S3StorageSettings.BaseDomainConfigKey, baseDomain);
        _dictionary.Add(S3StorageSettings.AllowedFileExtensionsConfigKey, allowedExtensions);
        _dictionary.Add(S3StorageSettings.AllowedFileSizeInMbConfigKey, allowedFileSizeInMb.ToString());
        _dictionary.Add(S3StorageSettings.CloudStorageUrlConfigKey, localS3Url);
        return this;
    }

    public IConfiguration Build()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(_dictionary)
            .Build();
    }
}