using System;
using System.IO;
using System.Net;

namespace S3.Integration.Utils;

public readonly struct RandomFileName
{
    private readonly string _fileName;

    public RandomFileName(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        OriginalFileName = WebUtility.HtmlEncode(fileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(Path.GetExtension(fileName)))
        {
            throw new ArgumentException("File name must have an extension");
        }

        _fileName = Guid.NewGuid() + "-" + OriginalFileName;
    }

    public string OriginalFileName { get; }

    public override string ToString()
    {
        return _fileName;
    }
}