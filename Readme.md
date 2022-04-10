# AspNetCore.Aws.S3.Simple

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/maximgorbatyuk/AspNetCore.Aws.S3.Simple/Tests)
![Nuget](https://img.shields.io/nuget/dt/AspNetCore.Aws.S3.Simple)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/maximgorbatyuk/AspNetCore.Aws.S3.Simple)
![GitHub](https://img.shields.io/github/license/maximgorbatyuk/AspNetCore.Aws.S3.Simple)

A [nuget package](https://www.nuget.org/packages/AspNetCore.Aws.S3.Simple/) which allows you to integrate S3 storage into your Web Application. Also, the library allows you to use [localstack](https://github.com/localstack/localstack) for debugging.

## Installation

```
dotnet add package AspNetCore.Aws.S3.Simple
```

## Demonstration

For simple demo, you need Docker installed in your PC. If you have it, then follow the instruction:

1. Pull the repo
2. run `win-run.ps1` or `unix-run.ps1` script
3. Go to https://localhost:5001/swagger/index.html. There are two endpoints: `/files/upload` and `/files/download`.

## How to install and setup

1. Install the nuget package `AspNetCore.Aws.S3.Simple`;
2. Create inheritors of `IFileStorageBase.cs` and `AmazonS3StorageBase`;

```csharp
using S3.Integration.AmazonServices;
using S3.Integration.Contracts;
using S3.Integration.Settings;

public interface IAvatarsStorage : IFileStorageBase
{
}

public class AvatarsS3Storage : AmazonS3StorageBase, IAvatarsStorage
{
    public AvatarsS3Storage(
        S3StorageSettings configuration,
        IS3FileValidator fileValidator)
        : base(configuration, fileValidator, "user-avatars")
    {
    }
}
```

3. Add settings in the Startul.cs or Program.cs file:

```csharp
var builder = WebApplication.CreateBuilder(args);
// ...
builder.Services
    .AddS3Settings()
    .AddS3Storage<IAvatarsStorage, AvatarsS3Storage>();
```

4. Add healthcheck settings (if necessary):

```csharp
builder.Services
    .AddHealthChecks()
    .AddS3HealthChecks(builder.Configuration);
```

5. Use the storage service

## Roadmap

- [] Add API for reading all files in the bucket
- [] Storing private and public files

## Sample project

CHeck out this sample [Web.Api](https://github.com/maximgorbatyuk/AspNetCore.Aws.S3.Simple/tree/dev/src/Sample.Api) project and see how to use the nuget

## Resources

- https://helibertoarias.com/blog/aws/localstack-using-s3-with-net-core/
- https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/tree/master/src/HealthChecks.Aws.S3
- https://purple.telstra.com/blog/local-amazon-s3
- https://github.com/localstack/localstack
- https://www.stevejgordon.co.uk/running-aws-s3-simple-storage-service-using-docker-for-net-core-developers
- https://github.com/dayanrr91/aws-net-core-examples
