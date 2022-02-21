using System;

namespace S3.Integration.Contracts;

public record Optional<TResult> : IOptional<TResult>
{
    public Optional(TResult result, Exception ex = null)
    {
        Result = result;
        ThrownError = ex;
    }

    public virtual bool Success => ThrownError is null;

    public TResult Result { get; }

    public Exception ThrownError { get; }
}