## Inspiration
Identity for all hackathon is a great event, it inspires me to learn and go to next level.

Here is the hypothetical use case, individuals who are outside of your organization need to do file collaboration with you. For example, download document or photos through user friendly web UI.

The file sharing must be secured with authentication and authorization and can not be anonymous. The capability should be highly scalable and customizable with either local account or federation with user’s existing social identity.
## What it does
Secured file collaboration with external user using Azure AD B2C, Microsoft Graph and managed identity, running in Azure cloud, highly scalable and customizable.
- Sign-in, Sign-up or Editing Profile through Azure AD B2C
- Azure AD B2C Integration with Google reCAPTCHA
- Managed Identity for Accessing Azure Blob Files        
- Showing B2C User Profile through Microsoft Graph API
- B2C User Deletion through Microsoft Graph API  

![Overview](https://github.com/Ronnie-personal/identity-for-all/blob/main/ReadmeFiles/fcollab-features.png?raw=true)

## How we built it
I started from very basic and gradually added more feature enhancement:  
- A console app to work Azure storage account authentication and managed identity
- Convert the console app to .NET API  
- Developed sign in user SPA code with MSAL for Angular using AAD  
- Create Azure AD B2C and user flow configuration  
- Convert both API and SPA code to work with Azure AD B2C 
- Test and debug code from visual studio and visual studio code    
- Deploy API to Azure app service  
- Deploy SPA to Azure storage static website  
- Create Azure storage to host document and photos  
- Deploy reCAPTCHA integration code to Azure function

![Overview](https://github.com/Ronnie-personal/identity-for-all/blob/main/ReadmeFiles/fcollab-architecture.png?raw=true)

please refer to following blog posts for more design and implementation details:  

https://cloudjourney.medium.com/file-collaboration-and-ms-identity-platform-826455764079  
https://cloudjourney.medium.com/msal-2-and-azure-ad-b2c-4873d264b094  
https://cloudjourney.medium.com/azure-storage-aad-authentication-f2eb48e481f0    
https://cloudjourney.medium.com/secure-azure-storage-api-16cf87dd39a6    
https://cloudjourney.medium.com/sing-in-users-and-azure-storage-api-3b7ee7dc5e7b  

For SharePoint online experiment, please refer to my blog post:  
https://cloudjourney.medium.com/file-collaboration-4ef397f3f176

## Challenges we ran into
- Coding detail is a challenge to me, since I don't do coding in my day to day job. I need to quickly refresh my memory and complete this full stack coding mission. Microsoft provides so many sample code in git hub, that is extremely helpful and I'm able to get started on both Angular and .NET API coding!  
- I also have doubt when I work on Azure AD B2C user management through Microsoft graph API, MS consultant pointed out a git hub sample code, which is exactly what I was looking for.

## Accomplishments that we're proud of
- I have better understanding of MS identity platform
- I can call myself full stack developer again after this project  
- Host the capability in Azure cloud with appropriate network and IAM security control

## What we learned
- Azure AD B2C sign-in, sign-up, edit profile, API connector, federation with social identity provider and custom page layout  
- List or delete user profile through Microsoft Graph API
- Protect API with Azure AD or Azure AD B2C  
- Sign in/up user from Angular SPA with MSAL  
- Read Azure Blob and return to API client  
- Download file from Angular  
- UI design

## What's next for File Collaboration and Microsoft Identity Platform  
Hakathon project is over soon, but my learning journey will continue.  
I plan to explore more about Microsoft identity platform, incorporate new features into this code stack.
