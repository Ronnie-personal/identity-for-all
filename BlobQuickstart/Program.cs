using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
namespace BlobQuickstart
{
    class Program
    {
        static async Task Main()
        {
            Console
                .WriteLine("Azure Blob Storage - .NET quickstart sample\n");
// Retrieve the connection string for use with the application. The storage
            // connection string is stored in an environment variable on the machine
            // running the application called AZURE_STORAGE_CONNECTION_STRING. If the
            // environment variable is created after the application is launched in a
            // console or with Visual Studio, the shell or application needs to be closed
            // and reloaded to take the environment variable into account.
            string connectionString =
                Environment
                    .GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
// Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient =
                new BlobServiceClient(connectionString);
// Create a local file in the ./data/ directory for uploading and downloading
            string localPath = "./data/";
            string fileName = "quickstart" + Guid.NewGuid().ToString() + ".txt";
            string localFilePath = Path.Combine(localPath, fileName);
// Write text to the file
            await File.WriteAllTextAsync(localFilePath, "Hello, World!");
// Get a reference to a blob
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient("test1");
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
Console
                .WriteLine("Uploading to Blob storage as blob:\n\t {0}\n",
                blobClient.Uri);
// Upload data from the local file
            await blobClient.UploadAsync(localFilePath, true);
        }
    }
}