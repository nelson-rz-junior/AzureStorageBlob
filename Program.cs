using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.IO;
using static System.Console;

namespace AzureStorageBlob
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();

            // Get a connection string to our Azure Storage account.
            var connectionString = configuration["ConnectionStrings:StorageAccount"];

            // Get a reference to the container client object so you can create the "photos" container
            string containerName = configuration["ContainerName"];
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            container.CreateIfNotExists();

            // Uploads the image to Blob storage. If a blob already exists with this name it will be overwritten
            string blobName = configuration["BlobName"];
            string fileName = "docs-and-friends-selfie-stick.png";
            BlobClient blobClient = container.GetBlobClient(blobName);
            blobClient.Upload(fileName, true);

            // List out all the blobs in the container
            var blobs = container.GetBlobs();
            foreach (var blob in blobs)
            {
                WriteLine($"{blob.Name} --> Created On: {blob.Properties.CreatedOn:yyyy-MM-dd HH:mm:ss}  Size: {blob.Properties.ContentLength}");
            }
        }
    }
}
