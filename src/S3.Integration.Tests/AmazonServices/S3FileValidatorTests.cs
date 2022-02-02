using Moq;
using S3.Integration.AmazonServices;
using S3.Integration.Models;
using S3.Integration.Settings;
using Xunit;

namespace S3.Integration.Tests.AmazonServices;

public class S3FileValidatorTests
{
    [Theory]
    [InlineData(6 * 1024 * 1024, "report.pdf", 6, "pdf", true)]
    [InlineData(6 * 1024 * 1024, "REPORT.PDF", 6, "pdf", true)]
    [InlineData(6 * 1024 * 1024, "report", 6, "pdf", false)]
    [InlineData(6 * 1024 * 1024, "report.png", 6, "pdf", false)]
    [InlineData(6 * 1024 * 1024, "report.jpg", 6, "pdf", false)]
    [InlineData(6 * 1024 * 1024, "report.jpeg", 6, "pdf", false)]
    [InlineData(6 * 1024 * 1024, "report.pdf", 5, "pdf", false)]
    [InlineData(6 * 1024 * 1024, "report.pdf", 8, "pdf", true)]
    [InlineData(6 * 1024 * 1024, "report.pdf", 6, "jpg,jpeg,png,pdf", true)]
    [InlineData(6 * 1024 * 1024, "report.jpeg", 6, "jpg,jpeg,png,pdf", true)]
    [InlineData(6 * 1024 * 1024, "report.jpg", 6, "jpg,jpeg,png,pdf", true)]
    [InlineData(6 * 1024 * 1024, "report.png", 6, "jpg,jpeg,png,pdf", true)]
    public void IsValid_Cases_Match(
        int fileLength,
        string fileName,
        int allowedFileSize,
        string allowedExtensionsString,
        bool expected)
    {
        var configuration = new S3StorageSettings(new InMemoryConfigurationBuilder()
            .AddS3Settings("bucket", allowedExtensions: allowedExtensionsString, allowedFileSizeInMb: allowedFileSize)
            .Build());

        var target = new S3FileValidator(configuration);

        var mock = new Mock<IUploadFileRequest>();
        mock.Setup(x => x.FileSize).Returns(fileLength);
        mock.Setup(x => x.FileName).Returns(fileName);

        Assert.Equal(expected, target.IsValid(mock.Object));
    }
}