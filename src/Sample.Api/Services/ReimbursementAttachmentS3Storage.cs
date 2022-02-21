using AspNetCore.Aws.S3.Simple.AmazonServices;
using AspNetCore.Aws.S3.Simple.Contracts;
using AspNetCore.Aws.S3.Simple.Settings;

namespace Sample.Api.Services;

public class ReimbursementAttachmentS3Storage : AmazonS3StorageBase, IReimbursementFileStorage
{
    public ReimbursementAttachmentS3Storage(
        S3StorageSettings configuration,
        IS3FileValidator fileValidator)
        : base(configuration, fileValidator, "reimbursement-attachments")
    {
    }
}