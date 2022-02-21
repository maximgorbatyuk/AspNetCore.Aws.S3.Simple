using System;
using AspNetCore.Aws.S3.Simple.Utils;
using Xunit;

namespace AspNetCore.Aws.S3.Simple.Tests.Utils;

public class RandomFileNameTests
{
    [Theory]
    [InlineData("hello.jpg", "hello.jpg")]
    [InlineData("hello.png", "hello.png")]
    [InlineData("HELLO.PDF", "hello.pdf")]
    public void Ctor_CreatesUniqueName_Ok(
        string fileName,
        string expectedFileName)
    {
        var target = new RandomFileName(fileName);
        Assert.Equal(expectedFileName, target.OriginalFileName);
        var firstRandomName = target.ToString();

        Assert.EndsWith(expectedFileName, firstRandomName);
        Assert.NotEqual(expectedFileName, firstRandomName);

        var secondRandomName = new RandomFileName(fileName).ToString();
        Assert.NotEqual(expectedFileName, secondRandomName);
        Assert.NotEqual(firstRandomName, secondRandomName);
        Assert.EndsWith(expectedFileName, secondRandomName);
    }

    [Theory]
    [InlineData("hello")]
    public void Ctor_InvalidFileName_Exception(
        string fileName)
    {
        Assert.Throws<ArgumentException>(() => new RandomFileName(fileName));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Ctor_NullAsAFileName_Exception(
        string fileName)
    {
        Assert.Throws<ArgumentNullException>(() => new RandomFileName(fileName));
    }
}