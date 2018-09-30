using System;
using Library.AzureStorage.Policy.SharedAccessPolicy;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Library.AzureStorage.Wrappers
{
    public class CloudBlobContainer : ICloudBlobContainer
    {
        private readonly Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer m_cloudBlobContainer;
        public Uri Uri => m_cloudBlobContainer.Uri;
        string ICloudBlobContainer.GetSharedAccessSignatureUri(CloudSharedAccessBlobPolicy sharedAccessBlobPolicy)
        {
            return GetSharedAccessSignatureUri(sharedAccessBlobPolicy);
        }

        public CloudBlobContainer(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer cloudBlobContainer)
        {
            m_cloudBlobContainer = cloudBlobContainer;
        }

        public string GetSharedAccessSignature(CloudSharedAccessBlobPolicy sharedAccessBlobPolicy)
        {
            var sasContainerToken =
                m_cloudBlobContainer.GetSharedAccessSignature(sharedAccessBlobPolicy.m_sharedAccessBlobPolicy);
            return sasContainerToken;
        }

        public string GetSharedAccessSignatureUri(CloudSharedAccessBlobPolicy sharedAccessBlobPolicy)
        {
            var sasContainerToken =
                m_cloudBlobContainer.GetSharedAccessSignature(sharedAccessBlobPolicy.m_sharedAccessBlobPolicy);
            return $"{m_cloudBlobContainer.Uri}{sasContainerToken}";
        }
    }
}