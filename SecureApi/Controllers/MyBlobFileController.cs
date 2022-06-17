using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace SecureApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class MyBlobFileController : ControllerBase
{
    private readonly ILogger<MyBlobFileController> _logger;
    private readonly IConfiguration _configuration;

    public MyBlobFileController(ILogger<MyBlobFileController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
[HttpGet(Name = "GetMyBlobFile")]
    public async Task<ActionResult<IEnumerable<MyBlobFile>>> Get()
    {
        var credential = new DefaultAzureCredential();
        var Uri = "https://" + _configuration.GetValue<string>("AzureStorage:AcctName") + ".blob.core.windows.net/" + _configuration.GetValue<string>("AzureStorage:ContainerName") + "/";

        var blobContainerClient =
            new BlobContainerClient(new Uri(Uri), credential);
        List<MyBlobFile> list = new List<MyBlobFile>();
        
        await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
        {

            DateTimeOffset? finalDate = blobItem.Properties.LastModified.HasValue ?  blobItem.Properties.LastModified.Value.ToLocalTime() : null;
            list.Add(new MyBlobFile { BlobFile = blobItem.Name, LastModified = finalDate });
        }
        return list.ToArray();
    }
[HttpPost]
    public async Task<ActionResult<MyBlobFile>> PostBlobFile(MyBlobFile blobFile )
    {
        var credential = new DefaultAzureCredential();
        var Uri = "https://" + _configuration.GetValue<string>("AzureStorage:AcctName") + ".blob.core.windows.net/" + _configuration.GetValue<string>("AzureStorage:ContainerName") + "/";

        BlobContainerClient blobContainerClient =
            new BlobContainerClient(new Uri(Uri), credential);
        BlobClient blobClient = blobContainerClient.GetBlobClient(blobFile.BlobFile);
        string downloadFileName = blobFile.BlobFile.Replace(".txt", ".DOWNLOADED.txt");
       
        BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
        var downloadedData = Encoding.UTF8.GetBytes(downloadResult.Content.ToString());
        return File(downloadedData, "text/csv", downloadFileName);
    }    
}
