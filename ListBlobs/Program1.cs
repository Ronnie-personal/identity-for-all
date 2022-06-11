using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Identity;
namespace LsttBlobs
{
    class Program
    {
        static async Task Main()
        {
            Console
                .WriteLine("Azure Blob Storage v12 - .NET quickstart sample\n");
            string storageAcctName =
                Environment
                    .GetEnvironmentVariable("name");
// When deployed to an azure host, the default azure credential will authenticate using managed identity.
            var credential =
                new DefaultAzureCredential();
// Get a reference to a blob
            var blobContainerClient =
                new BlobContainerClient(new Uri("https://" + storageAcctName + ".blob.core.windows.net/test1/"),
                    credential);
            Console
                .WriteLine("Listing blobs...");
// List all blobs in the container
           await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
           {
              Console.WriteLine("\t" + blobItem.Name);
           
           }
        }
    }
}
