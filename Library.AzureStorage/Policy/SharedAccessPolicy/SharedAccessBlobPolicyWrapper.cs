using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Library.AzureStorage.Policy.SharedAccessPolicy;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Library.AzureStorage.Policy.SharedAccessPolicy
{
    public class CloudSharedAccessBlobPolicy : ISharedAccessBlobPolicy
    {
        public SharedAccessBlobPolicy m_sharedAccessBlobPolicy;

        public CloudSharedAccessBlobPolicy(DateTimeOffset? sharedAccessStartTime, DateTimeOffset? sharedAccessExpiryTime, IEnumerable<SharedAccessBlobPermissions> sharedAccessBlobPermissionsList)
        {
            m_sharedAccessBlobPolicy = new SharedAccessBlobPolicy();
            m_sharedAccessBlobPolicy.SharedAccessStartTime = sharedAccessStartTime  ?? DateTimeOffset.UtcNow;
            m_sharedAccessBlobPolicy.SharedAccessExpiryTime = sharedAccessExpiryTime ?? DateTimeOffset.UtcNow.AddHours(24);
            SetPermissions(sharedAccessBlobPermissionsList);
        }

        private void SetPermissions(IEnumerable<SharedAccessBlobPermissions> sharedAccessBlobPermissionsList)
        {
            foreach (var sharedAccessBlobPermissions in sharedAccessBlobPermissionsList)
            {
                m_sharedAccessBlobPolicy.Permissions |= sharedAccessBlobPermissions;
            }
        }
    }
}