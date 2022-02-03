using S3.Integration.AmazonServices;
using S3.Integration.Contracts;
using S3.Integration.Settings;

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