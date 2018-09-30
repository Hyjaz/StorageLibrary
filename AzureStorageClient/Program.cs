using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using AzureStorageClient.Models;
using Library.AzureStorage;
using Library.AzureStorage.Policy.SharedAccessPolicy;
using Library.AzureStorage.Wrappers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using CloudBlobContainer = Library.AzureStorage.Wrappers.CloudBlobContainer;

namespace AzureStorageClient
{
    class Program
    {
        private static ConfigurationModel s_configuration;
        private static HttpClient s_httpClient;
        static void Main(string[] args)
        {
            s_httpClient = new HttpClient();

            LoadConfiguration();

            // TODO: Wrap CloudStorageAccount
            var storageAccount = CloudStorageAccount.Parse(s_configuration.StorageAccountConnectionString);

            // TODO: Add method to get a CloudBlobClientWrapper to CloudStorageAccount
            var blobClient = storageAccount.CreateCloudBlobClient();

            // TODO: Add methods to get a cloudBlobContainerWrapper to CloudStorageAccount
            var container = blobClient.GetContainerReference("sascontainer");
            container.CreateIfNotExistsAsync().Wait();

            IList<SharedAccessBlobPermissions> sharedAccessBlobPermissionsList = new List<SharedAccessBlobPermissions>()
            {
                SharedAccessBlobPermissions.Add,
                SharedAccessBlobPermissions.Read,
                SharedAccessBlobPermissions.List
            };

            var cloudblobContainer = new CloudBlobContainer(container);

            var sharedAccessPolicy = new CloudSharedAccessBlobPolicy(null, null, sharedAccessBlobPermissionsList);
            var sasToken = cloudblobContainer.GetSharedAccessSignature(sharedAccessPolicy);
            ListContainer(cloudblobContainer, sasToken).Wait();
            Console.WriteLine($"sasToken={sasToken}");

            Console.ReadKey();
        }

        private static async Task ListContainer(CloudBlobContainer cloudblobContainer, string sasToken)
        {

            var uriBuilder = new UriBuilder(cloudblobContainer.Uri);

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["restype"] = "container";
            query["comp"] = "list";
            uriBuilder.Query = $"{query.ToString()}&{sasToken.Remove(0, 1)}";

            var requestUri = uriBuilder.ToString();
            var response = await s_httpClient.GetAsync(requestUri);
            var s = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Console.WriteLine(s);
        }
        private static void LoadConfiguration()
        {
            s_configuration =
                JsonConvert.DeserializeObject<ConfigurationModel>(File.ReadAllText(@".\\appsettings.json"));
        }
    }
}
