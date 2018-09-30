using System;
using Library.AzureStorage.Policy.SharedAccessPolicy;

namespace Library.AzureStorage.Wrappers
{
    public interface ICloudBlobContainer
    {
        string GetSharedAccessSignature(CloudSharedAccessBlobPolicy sharedAccessBlobPolicyWrapper);
        string GetSharedAccessSignatureUri(CloudSharedAccessBlobPolicy sharedAccessBlobPolicy);
    }
}