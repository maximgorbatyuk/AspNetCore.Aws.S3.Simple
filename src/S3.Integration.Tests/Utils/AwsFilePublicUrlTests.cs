using System.Collections.Generic;
using S3.Integration.Settings;
using S3.Integration.Utils;
using Xunit;

namespace S3.Integration.Tests.Utils;

public class AwsFilePublicUrlTests
{
    [Theory]
    [InlineData("intranet-files", "us-region-1", "amazonaws.com", "file.jpg", true, "https://intranet-files.us-region-1.amazonaws.com/file.jpg")]
    [InlineData("intranet-files", "us-region-1", "amazonaws.com", "file.jpg", false, "https://intranet-files.amazonaws.com/file.jpg")]
    [InlineData("intranet", "us-region-2", "amazonaws.com", "report.jpg", true, "https://intranet.us-region-2.amazonaws.com/report.jpg")]
    public void Ctor_Cases_Match(
        string bucketName,
        string awsRegion,
        string baseDomain,
        string uniqueStorageKey,
        bool userRegion,
        string expected)
    {
        var myConfiguration = new InMemoryConfigurationBuilder()
            .AddS3Settings(
                bucketName,
                awsRegion,
                baseDomain: baseDomain);

        var target = new AwsFilePublicUrl(new S3StorageSettings(myConfiguration.Build()), uniqueStorageKey, userRegion);
        Assert.Equal(expected, target.ToString());
    }
}