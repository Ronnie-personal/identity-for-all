using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Text;

namespace SecureApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
public class MyBlobFileController : ControllerBase
{
    private readonly ILogger<MyBlobFileController> _logger;
    private readonly IConfiguration _configuration;
    private string url;
   
    public MyBlobFileController(ILogger<MyBlobFileController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        url = "https://" + _configuration.GetValue<string>("AzureStorage:AcctName") + ".blob.core.windows.net/" + _configuration.GetValue<string>("AzureStorage:ContainerName") + "/";
    }
[HttpGet(Name = "GetMyBlobFile")]
    public async Task<ActionResult<IEnumerable<MyBlobFile>>> Get()
    {
        var credential = new DefaultAzureCredential();
        var blobContainerClient =
            new BlobContainerClient(new Uri(this.url), credential);
        List<MyBlobFile> list = new List<MyBlobFile>();
        
        await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
        {
            list.Add(item: new MyBlobFile { BlobFile = blobItem.Name, LastModified = blobItem.Properties.LastModified });
        }
        return list.ToArray();
    }
[HttpPost]
    public async Task<ActionResult<MyBlobFile>> PostBlobFile(MyBlobFile blobFile )
    {
        var credential = new DefaultAzureCredential();

        BlobContainerClient blobContainerClient =
            new BlobContainerClient(new Uri(this.url), credential);
        BlobClient blobClient = blobContainerClient.GetBlobClient(blobFile.BlobFile);
        string downloadFileName = blobFile.BlobFile.Replace("/", "-");
       
        BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
        //var downloadedData = Encoding.UTF8.GetBytes(downloadResult.Content.ToString());
        var downloadedData = downloadResult.Content;
        HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
        return File(downloadedData.ToArray(), "application/octet-stream", downloadFileName);
    }   
}
