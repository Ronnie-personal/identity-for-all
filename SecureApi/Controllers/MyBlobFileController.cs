using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

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
}
