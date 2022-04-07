using AspNetCore.Aws.S3.Simple.AmazonServices;
using AspNetCore.Aws.S3.Simple.Models;
using AspNetCore.Aws.S3.Simple.Settings;
using Moq;
using Xunit;

namespace AspNetCore.Aws.S3.Simple.Tests.AmazonServices;

public class S3FileValidatorTests
{
    [Theory]
    [InlineData(6 * 1024 * 1024, "report.pdf", 6, "pdf", FileValidationResult.Valid)]
    [InlineData(6 * 1024 * 1024, "REPORT.PDF", 6, "pdf", FileValidationResult.Valid)]
    [InlineData(6 * 1024 * 1024, "report", 6, "pdf", FileValidationResult.FileExtensionEmpty)]
    [InlineData(6 * 1024 * 1024, "report.png", 6, "pdf", FileValidationResult.FileExtensionNotAllowed)]
    [InlineData(6 * 1024 * 1024, "report.jpg", 6, "pdf", FileValidationResult.FileExtensionNotAllowed)]
    [InlineData(6 * 1024 * 1024, "report.jpeg", 6, "pdf", FileValidationResult.FileExtensionNotAllowed)]
    [InlineData(6 * 1024 * 1024, "report.pdf", 5, "pdf", FileValidationResult.FilesizeExceeded)]
    [InlineData(6 * 1024 * 1024, "report.pdf", 8, "pdf", FileValidationResult.Valid)]
    [InlineData(6 * 1024 * 1024, "report.pdf", 6, "jpg,jpeg,png,pdf", FileValidationResult.Valid)]
    [InlineData(6 * 1024 * 1024, "report.jpeg", 6, "jpg,jpeg,png,pdf", FileValidationResult.Valid)]
    [InlineData(6 * 1024 * 1024, "report.jpg", 6, "jpg,jpeg,png,pdf", FileValidationResult.Valid)]
    [InlineData(6 * 1024 * 1024, "report.png", 6, "jpg,jpeg,png,pdf", FileValidationResult.Valid)]
    [InlineData(6 * 1024 * 1024, "report.gif", 6, "jpg,jpeg,png,pdf,gif", FileValidationResult.Valid)]
    public void IsValid_Cases_Match(
        int fileLength,
        string fileName,
        int allowedFileSize,
        string allowedExtensionsString,
        FileValidationResult expected)
    {
        var configuration = new S3StorageSettings(new InMemoryConfigurationBuilder()
            .AddS3Settings("bucket", allowedExtensions: allowedExtensionsString, allowedFileSizeInMb: allowedFileSize)
            .Build());

        var target = new S3FileValidator(configuration);

        var mock = new Mock<IUploadFileRequest>();
        mock.Setup(x => x.FileSize).Returns(fileLength);
        mock.Setup(x => x.FileName).Returns(fileName);

        Assert.Equal(expected, target.Validate(mock.Object));
    }
}