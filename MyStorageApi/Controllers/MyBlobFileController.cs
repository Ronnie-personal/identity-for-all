using System;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
namespace MyStorageApi.Controllers;
[ApiController]
[Route("[controller]")]
public class MyBlobFileController : ControllerBase
{
    private readonly ILogger<MyBlobFileController> _logger;
public MyBlobFileController(ILogger<MyBlobFileController> logger)
    {
        _logger = logger;
    }
[HttpGet(Name = "GetMyBlobFile")]
    public async Task<ActionResult<IEnumerable<MyBlobFile>>> Get()
    {
        string? storageAcctName = Environment.GetEnvironmentVariable("name");
        var credential = new DefaultAzureCredential();
        var blobContainerClient =
            new BlobContainerClient(new Uri("https://" + storageAcctName + ".blob.core.windows.net/test1/"),
                credential);
        List<MyBlobFile> list = new List<MyBlobFile>();
        
        await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
        {

            DateTimeOffset? finalDate = blobItem.Properties.LastModified.HasValue ?  blobItem.Properties.LastModified.Value.ToLocalTime() : null;
            list.Add(new MyBlobFile { BlobFile = blobItem.Name, LastModified = finalDate });
        }
        return list.ToArray();
    }
}