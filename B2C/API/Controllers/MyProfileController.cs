using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Azure.Identity;
using SecureApiB2C.Models;
using Microsoft.Identity.Web.Resource;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecureApiB2C.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:ProfileScopes")]
    public class MyProfileController : ControllerBase
    {
        private readonly ILogger<MyProfileController> _logger;
        private readonly IConfiguration _configuration;
        private ConfidentialClientApplicationOptions _applicationOptions;
        private GraphServiceClient graphClient;

        public MyProfileController(ILogger<MyProfileController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _applicationOptions = new ConfidentialClientApplicationOptions();
            configuration.Bind("AzureAD", _applicationOptions);
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var clientSecretCredential = new ClientSecretCredential(_configuration.GetValue<string>("AzureAD:TenantId"), _configuration.GetValue<string>("AzureAD:ClientId"), _configuration.GetValue<string>("AzureAD:ClientSecret"));
            graphClient = new GraphServiceClient(clientSecretCredential, scopes);
        }

        // GET: <ProfileController>
        [HttpGet("{id}")]
        public async Task<ActionResult<MyProfile>> Get(string id)
        {
            var a = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            if (a != id)
            {
                return BadRequest();
            }
            var profile = await graphClient.Users[id]
                    .Request()
                    .Select(e => new
                    {
                        e.Id,
                        e.DisplayName,
                        e.Mail,
                        e.GivenName,
                        e.Surname,
                        e.JobTitle,
                        e.StreetAddress,
                        e.City,
                        e.State,
                        e.PostalCode,
                        e.Country
                    })
                    .GetAsync();
           if (profile == null)
           {
               return NotFound();
           }
           MyProfile profileItem = new MyProfile
           {
               id = profile.Id,
               displayName = profile.DisplayName,
               email = profile.Mail,
               givenName = profile.GivenName,
               surname = profile.Surname,
               jobTitle = profile.JobTitle,
               state = profile.State,
               streetAddress = profile.StreetAddress,
               city = profile.City,
               country = profile.Country,
               postalCode = profile.PostalCode
           };

            return profileItem;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseMessage>> DeleteUserAccount(string id)
        {
            var a = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            if (a != id)
            {
                return BadRequest();
            } 
            var profile = await graphClient.Users[id]
                .Request()
                .Select(e => new
                {
                    e.Id
                })
                .GetAsync();
            if (profile == null)
            {
                return NotFound();
            }
            await graphClient.Users[id]
               .Request()
               .DeleteAsync();

            return new ResponseMessage { message = "successful delete" };
        }
    }
}
