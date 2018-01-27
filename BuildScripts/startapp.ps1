param($websiteName)
$website = Get-AzureWebsite -Name $websiteName
Start-AzureWebsite -Name $websiteName