Install-Module -Name Microsoft.Online.SharePoint.PowerShell -Scope CurrentUser
Get-Module -Name Microsoft.Online.SharePoint.PowerShell -ListAvailable | Select Name,Version

Connect-SPOService -Url "https://$env:o365value-admin.sharepoint.com"

Set-SPOTenant -EnableAzureADB2BIntegration $true
Set-SPOTenant -SyncAadB2BManagementPolicy $true

Connect-AzureAD
Get-AzureADUser -Filter "startswith('rkao', userPrincipalName)"