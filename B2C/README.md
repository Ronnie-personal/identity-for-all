
# File Collaboration and Microsoft Idenity Platform Project

> :Info: This project is live now, when you try it out, it will take a little time to load up, since the free tier Azure app service is NOT always on.

 1. [Overview](#overview)
 1. [Architecture Design](#architecture-design)
 1. [Contents](#contents)
 1. [Prerequisites](#prerequisites)
 1. [Setup](#setup)
 1. [Registration](#registration)
 1. [Captcha Integration and Custom Page Layout](#captcha-integration-and-custom-page-layout)  
 1. [Running the sample](#running-the-sample)
 1. [Explore the sample](#explore-the-sample)
 1. [About the code](#about-the-code)
 1. [More information](#more-information)

## Overview

This project demonstrates an Angular single-page application (SPA) calling C# .NET Core web API secured with [Azure AD B2C](https://docs.microsoft.com/azure/active-directory-b2c/overview) using the [Microsoft Authentication Library for Angular](https://github.com/AzureAD/microsoft-authentication-library-for-js/tree/dev/lib/msal-angular) (MSAL Angular) for the SPA and the [Microsoft.Identity.Web](https://github.com/AzureAD/microsoft-identity-web) (M.I.W) for the web API.  

API layer works with Azure blob files and calls Micrsoft Graph for Azure AD B2C user account managment.  

We will walkthrough the steps to get the project up and running.

## Architecture Design

![Overview](https://github.com/Ronnie-personal/identity-for-all/blob/main/ReadmeFiles/fcollab-architecture.png?raw=true)

## Contents

| File/folder                         | Description                                                |
|-------------------------------------|------------------------------------------------------------|
| `SPA/src/app/auth-config.ts`        | Authentication parameters for SPA project reside here.     |
| `SPA/src/app/app.module.ts`         | MSAL Angular is initialized here.                          |
| `API/appsettings.json`              | Authentication parameters for API project reside here.     |
| `API/Program.cs`                    | Microsoft.Identity.Web is initialized here.                |
| `captcha/local.settings.json`       | reCAPTCHA integration function app setting.                |  

## Prerequisites

- An **Azure AD B2C** tenant. For more information see: [How to get an Azure AD B2C tenant](https://docs.microsoft.com/azure/active-directory-b2c/tutorial-create-tenant)  
- An **Azure AD** tenant and **Azure subscriptoin**  
- An **API key pair** from google reCAPTCHA v2. For more information see: [Sign up for an API key pair](http://www.google.com/recaptcha/admin)  
- A **Google application**. For more information see: [Create Google application](https://docs.microsoft.com/en-us/azure/active-directory-b2c/identity-provider-google?pivots=b2c-user-flow#create-a-google-application)  
- A **Github application**. For more information see: [Create a GitHub OAuth application](https://docs.microsoft.com/en-us/azure/active-directory-b2c/identity-provider-github?pivots=b2c-user-flow#create-a-github-oauth-application)  
- A **Amazon app**. For more information see: [Create an app in the Amazon developer console](https://docs.microsoft.com/en-us/azure/active-directory-b2c/identity-provider-amazon?pivots=b2c-user-flow#create-an-app-in-the-amazon-developer-console)  
- A **Facebook application**. For more information see: [Create a Facebook application](https://docs.microsoft.com/en-us/azure/active-directory-b2c/identity-provider-facebook?pivots=b2c-user-flow#create-a-facebook-application)  
- An **App regirstration** in your Azure AD tenant. For more information see: [Create a Microsoft account application](https://docs.microsoft.com/en-us/azure/active-directory-b2c/identity-provider-microsoft-account?pivots=b2c-user-flow#create-a-microsoft-account-application) - 


## Setup

### Step 1. Clone or download this repository

```console
    git clone https://github.com/Ronnie-personal/identity-for-all.git
```

or download and extract the repository .zip file.

> :warning: To avoid path length limitations on Windows, we recommend cloning into a directory near the root of your drive.

### Step 2. Install .NET Core API dependencies

```console
    cd identity-for-all
    cd B2C/API
    dotnet restore
```

### Step 3. Trust development certificates

```console
    dotnet dev-certs https --clean
    dotnet dev-certs https --trust
```

For more information and potential issues, see: [HTTPS in .NET Core](https://docs.microsoft.com/aspnet/core/security/enforcing-ssl).

### Step 4. Install Angular SPA dependencies

```console
    cd ../
    cd B2C/SPA
    npm install
```

### Step 5. Create storage account to host document and photos  
- Create Azure storage account and container, for more information see: [Create storage account](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create?toc=%2Fazure%2Fstorage%2Fblobs%2Ftoc.json&tabs=azure-portal)  
- Disable public access, for more information see: [Disable public access](https://docs.microsoft.com/en-us/azure/storage/common/shared-key-authorization-prevent?tabs=portal#remediate-authorization-via-shared-key)  
- Disable storage account access key, for more information see: [Disable storage account access key](https://docs.microsoft.com/en-us/azure/storage/blobs/anonymous-read-access-configure?tabs=portal#allow-or-disallow-public-read-access-for-a-storage-account)    
- Grant RBAC permission to your API/Azure app service managed identity
```
New-AzRoleAssignment -RoleDefinitionName "Storage Blob Data Reader" -ObjectId <Managed Identity OID> -Scope <Resource ID>
```

## Registration

### Choose the Azure AD tenant where you want to create your applications

As a first step you'll need to:

1. Sign in to the [Azure portal](https://portal.azure.com).
1. If your account is present in more than one Azure AD B2C tenant, select your profile at the top right corner in the menu on top of the page, and then **switch directory** to change your portal session to the desired Azure AD B2C tenant.

### Create User Flows and Custom Policies

Please refer to: [Tutorial: Create user flows in Azure Active Directory B2C](https://docs.microsoft.com/azure/active-directory-b2c/tutorial-create-user-flows)

### Add External Identity Providers

Please refer to: [Tutorial: Add identity providers to your applications in Azure Active Directory B2C](https://docs.microsoft.com/azure/active-directory-b2c/tutorial-add-identity-providers)

### Register the Service App (msal-dotnet-api and ConfClientGraph)  
#### msal-dotnet-api  

1. Navigate to the [Azure portal](https://portal.azure.com) and select the **Azure AD B2C** service.
1. Select the **App Registrations** blade on the left, then select **New registration**.
1. In the **Register an application page** that appears, enter your application's registration information:
   - In the **Name** section, enter a meaningful application name that will be displayed to users of the app, for example `msal-dotnet-api`.
   - Under **Supported account types**, select **Accounts in any identity provider or organizational directory (for authenticating users with user flows)**.
1. Select **Register** to create the application.
1. In the app's registration screen, find and note the **Application (client) ID**. You use this value in your app's configuration file(s) later in your code.
1. Select **Save** to save your changes.
1. In the app's registration screen, select the **Expose an API** blade to the left to open the page where you can declare the parameters to expose this app as an API for which client applications can obtain [access tokens](https://docs.microsoft.com/azure/active-directory/develop/access-tokens) for.
The first thing that we need to do is to declare the unique [resource](https://docs.microsoft.com/azure/active-directory/develop/v2-oauth2-auth-code-flow) URI that the clients will be using to obtain access tokens for this Api. To declare an resource URI, follow the following steps:
   - Select `Set` next to the **Application ID URI** to generate a URI that is unique for this app.
   - For this sample, use this URI (`https://{tenantName}.onmicrosoft.com/fcollab`) by selecting **Save**.
1. All APIs have to publish a minimum of one [scope](https://docs.microsoft.com/azure/active-directory/develop/v2-oauth2-auth-code-flow#request-an-authorization-code) for the client's to obtain an access token successfully. To publish a scope, follow the following steps:
   - Select **Add a scope** button open the **Add a scope** screen and Enter the values as indicated below:
        - For **Scope name**, use `read`.
        - For **Admin consent display name** type `Access msal-dotnet-api`.
        - For **Admin consent description** type `Allows the app to access msal-dotnet-api as the signed-in user.`
        - Keep **State** as **Enabled**.
        - Select the **Add scope** button on the bottom to save this scope.
1. Add three more scopes: `write`, `profile.read` and `profile.delete`.  
1. On the right side menu, select the `Manifest` blade.
   - Set `accessTokenAcceptedVersion` property to **2**.
   - Click on **Save**.

#### ConfClientGraph
Create a single tenant app registration in Azure B2C tananet for the API to call Microsoft graph API, so that we can list user profile or delete user.  
For more infomration see: [Register Management Application](https://docs.microsoft.com/en-us/azure/active-directory-b2c/microsoft-graph-get-started?tabs=app-reg-ga#register-management-application), it needs graph API `User.ReadWrite.All` permission.  

#### Configure the Service App (msal-dotnet-api) to Use Your App Registration

Open the project in your IDE (like Visual Studio or Visual Studio Code) to configure the code.

> In the steps below, "ClientID" is the same as "Application ID" or "AppId".

1. Open the `API\appsettings.json` file.
1. Find the key `AzureAdB2C:Instance` and replace the existing value with your Azure AD B2C tenant name.
1. Find the key `AzureADB2C:ClientId` and replace the existing value with the application ID (clientId) of `msal-dotnet-api` app copied from the Azure portal.
1. Find the key `AzureADB2C:Domain` and replace the existing value with your Azure AD B2C domain name.

1. Find the key `AzureADB2C:SignUpSignInPolicyId` and replace the existing value with your sign-up/sign-in user-flow string e.g. `b2c_1_signupsignin`.
1. Find the key `AzureADB2C:EditProfilePolicyId` and replace the existing value with your profile-edit user-flow string e.g. `b2c_1_editprofile`.

1. Find the key `AzureADB2C:Scopes` and replace value with `read write`.
1. Find the key `AzureADB2C:ProfileScopes` and replace its value with `profile.read profile.delete`.  

1. Find the key `AzureStorage:AcctName` and replace value with your Azure storage account which hosts your document and photos.
1. Find the key `AzureStorage:ContainerName` and replace its value with your storage account container.
  
1. Find the key `AzureAd:Domain` and replace value with your Azure AD B2C domain name.
1. Find the key `AzureAd:TenantId` and replace its value with your Azure AD B2C tenant id.  
1. Find the key `AzureAd:ClientId` and replace its value with the application ID(clientId) of `ConfClientGrap`.    
1. Find the key `AzureAd:ClientSecret` and replace its value with the secret of app registration `ConfClientGrap`.  

### Register the Client App (msal-angular-spa)

1. Navigate to the [Azure portal](https://portal.azure.com) and select the **Azure AD B2C** service.
1. Select the **App Registrations** blade on the left, then select **New registration**.
1. In the **Register an application page** that appears, enter your application's registration information:
   - In the **Name** section, enter a meaningful application name that will be displayed to users of the app, for example `msal-angular-spa`.
   - Under **Supported account types**, select **Accounts in any identity provider or organizational directory (for authenticating users with user flows)**.
   - In the **Redirect URI (optional)** section, select **Single-page application** in the combo-box and enter the following redirect URI: `http://localhost:4200/`.
1. Select **Register** to create the application.
1. In the app's registration screen, find and note the **Application (client) ID**. You use this value in your app's configuration file(s) later in your code.
1. Select **Save** to save your changes.
1. In the app's registration screen, select the **API permissions** blade in the left to open the page where we add access to the APIs that your application needs.
   - Select the **Add a permission** button and then,
   - Ensure that the **My APIs** tab is selected.
   - In the list of APIs, select the API `msal-dotnet-api`.
   - In the **Delegated permissions** section, select the **Access 'msal-dotnet-api'** in the list. Use the search box if necessary.
   - Select the **Add permissions** button at the bottom.
   - Finally, click on the **Grant admin consent** button at the top.

#### Configure the Client app (msal-angular-spa) to Use Your App Registration

Open the project in your IDE (like Visual Studio or Visual Studio Code) to configure the code.

> In the steps below, "ClientID" is the same as "Application ID" or "AppId".

1. Open the `SPA\src\app\auth-config.ts` file.
1. Find the key `Configuration.auth.clientId` and replace the existing value with the application ID (clientId) of `msal-angular-spa` app copied from the Azure portal.
1. Find the key `Configuration.auth.redirectUri` and replace the existing value with your SPA home page URL, for example `http://localhost:4200`.
1. Find the key `protectedResources.MyStorageApi.scopes` and replace the existing value with the scope of the web API that you have just exposed during the web API registration steps, for example `[https://{tenantName}.onmicrosoft.com/{service_clientId}/read, https://{tenantName}.onmicrosoft.com/{service_clientId}/write]`  
1. Find the key `protectedResources.MyProfileApi.scopes` and replace the existing value with the scope of the web API that you have just exposed during the web API registration steps, for example `[https://{tenantName}.onmicrosoft.com/{service_clientId}/profile.read, https://{tenantName}.onmicrosoft.com/{service_clientId}/profile.delete]`  

To setup your B2C user-flows, do the following:

1. Find the key `b2cPolicies.names.signUpSignIn` and populate it with your policy names e.g. `B2C_1_signupsignin`.
1. Find the key `b2cPolicies.names.editProfile` and populate it with your policy names e.g. `B2C_1_editprofile`.
1. Find the key `authorities.signUpSignIn.authority` and populate it with your policy authority strings e.g. `https://<your-tenant-name>.b2clogin.com/<your-tenant-name>.onmicrosoft.com/b2c_1_signupsignin`.
1. Find the key `authorities.editProfile.authority` and populate it with your policy authority strings e.g. `https://<your-tenant-name>.b2clogin.com/<your-tenant-name>.onmicrosoft.com/b2c_1_editprofile`.  
1. Find the key `authorityDomain` and populate it with the domain portion of your authority string e.g. `<your-tenant-name>.b2clogin.com`.

## Captcha integration and custom page layout

1. Create Azure storage account to host custom page.  
1. Deploy Azure function app to integrate with Azure AD B2C API connector, when run from local, add settings to local.settings.json, when run from Azure function app, use funcation app app settings.
    - Find the key `BASIC_AUTH_USERNAME` and populate it with your arbitrary username e.g. `b2capiconnector`.  
    - Find the key `BASIC_AUTH_PASSWORD` and populate it with your arbitrary password.  
    - Find the key `CAPTCHA_SECRET_KEY` and populate it with your reCAPTCHA secret key, you get the key when create API key pair from Google reCAPTCHA v2.  
    - Find the key `B2C_EXTENSIONS_APP_ID` and populate it with application id(client Id) of `b2c-extensions-app`, ensure to remove all the hyphons.  
1. Add cutom attribute `CaptchaUserResponseToken` in Azure B2C.    
1. New API connector from Azure AD B2C with basic auth, userid is the value from `BASIC_AUTH_USERNAME`, passowrd is the value from `BASIC_AUTH_PASSWORD`.      
1. From Azure AD B2C `User flows` blade  
    - Select your signupsignin flow
    - In Overview page, select `Page layouts`  
    - Update `Custom page URI` for both Local account sign up page and Socail account sign up page with your custom page URL e.g `https://<storage acct>.blob.core.windows.net/<container>/selfAsserted.html`  

  For more information see: [deployment](https://github.com/Azure-Samples/active-directory-b2c-node-sign-up-user-flow-captcha).  

## Running the sample

Using a command line interface such as VS Code integrated terminal, locate the application directory. Then:  

```console
    cd B2C/SPA
    npm start
```

In a separate console window, execute the following commands:

```console
    cd B2C/API
    dotnet run
```

## Explore the sample

1. Open your browser and navigate to `http://localhost:4200`.
2. Sign-in using the button on the top-left corner.
3. Select the **List Azure Blob** button on the navigation bar to access shared document or photos.
4. Select the **Show Profile** or **Edit Profile** button on the navigation bar to work with user profile.  
5. Select the **Close Account** button on the navigation bar if you need to delete user account.  

> :information_source: Did the sample not work for you as expected? Then please reach out to us using the [GitHub Issues](../../../../issues) page.

> :information_source: if you believe your issue is with the B2C service itself rather than with the sample, please file a support ticket with the B2C team by following the instructions [here](https://docs.microsoft.com/azure/active-directory-b2c/support-options).

## We'd love your feedback!

Were we successful in addressing your learning objective? Consider taking a moment to [share your experience with us](https://forms.office.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR73pcsbpbxNJuZCMKN0lURpUOU5PNlM4MzRRV0lETkk2ODBPT0NBTEY5MCQlQCN0PWcu).

## About the code

### Access token validation

On the SPA side, clients should treat access tokens as opaque strings, as the contents of the token are intended for the resource only (such as a web API or Microsoft Graph). For validation and debugging purposes, developers can decode **JWT**s (*JSON Web Tokens*) using a site like [jwt.ms](https://jwt.ms).

On the web API side, token validation is handled by [Microsoft.Identity.Web](https://github.com/AzureAD/microsoft-identity-web), using `JwtBearerDefaults.AuthenticationScheme`. Simply initialize `AddMicrosoftIdentityWebApi()` with your configuration and add `AddAuthorization()` to the service;

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));
```

In your controller, add [Authorize] decorator, which will make sure all incoming requests have an authentication bearer:

```csharp
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
```

### CORS configuration

You need to set **CORS** policy to be able to call the **MyBlobFileAPI** in [Program.cs](./API/Program.cs). For the purpose of this sample, we are setting it to allow *any* domain and methods. In production, you should modify this to allow only the domains and methods you designate.

```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
                      policy  =>
                      {
                          policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("content-disposition");
                      });
}); 
```

### Debugging the sample

To debug the .NET Core web API that comes with this sample, install the [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) for Visual Studio Code.

Learn more about using [.NET Core with Visual Studio Code](https://docs.microsoft.com/dotnet/core/tutorials/with-visual-studio-code).

## More information

- [What is Azure Active Directory B2C?](https://docs.microsoft.com/azure/active-directory-b2c/overview)
- [Application types that can be used in Active Directory B2C](https://docs.microsoft.com/azure/active-directory-b2c/application-types)
- [Recommendations and best practices for Azure Active Directory B2C](https://docs.microsoft.com/azure/active-directory-b2c/best-practices)
- [Azure AD B2C session](https://docs.microsoft.com/azure/active-directory-b2c/session-overview)
- [Initialize client applications using MSAL.js](https://docs.microsoft.com/azure/active-directory/develop/msal-js-initializing-client-applications)
- [Single sign-on with MSAL.js](https://docs.microsoft.com/azure/active-directory/develop/msal-js-sso)
- [Handle MSAL.js exceptions and errors](https://docs.microsoft.com/azure/active-directory/develop/msal-handling-exceptions?tabs=javascript)
- [Logging in MSAL.js applications](https://docs.microsoft.com/azure/active-directory/develop/msal-logging?tabs=javascript)
- [Pass custom state in authentication requests using MSAL.js](https://docs.microsoft.com/azure/active-directory/develop/msal-js-pass-custom-state-authentication-request)
- [Prompt behavior in MSAL.js interactive requests](https://docs.microsoft.com/azure/active-directory/develop/msal-js-prompt-behavior)
- [Use MSAL.js to work with Azure AD B2C](https://docs.microsoft.com/azure/active-directory/develop/msal-b2c-overview)

For more information about how OAuth 2.0 protocols work in this scenario and other scenarios, see [Authentication Scenarios for Azure AD](https://docs.microsoft.com/azure/active-directory/develop/authentication-flows-app-scenarios).
