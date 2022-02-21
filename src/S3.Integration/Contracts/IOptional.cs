using System;

namespace S3.Integration.Contracts;

public interface IOptional<out TResult>
{
    bool Success { get; }

    TResult Result { get; }

    Exception ThrownError { get; }
}