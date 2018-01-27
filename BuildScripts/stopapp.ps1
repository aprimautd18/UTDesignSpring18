param($websiteName)
$website = Get-AzureWebsite -Name $websiteName
Stop-AzureWebsite -Name $websiteName