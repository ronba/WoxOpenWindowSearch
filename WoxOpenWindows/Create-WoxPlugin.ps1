param($outputDir)

Write-Output "OutputDir: $outputDir"

#update plugin version:
$json = Get-Content (Join-Path $outputDir plugin.json) | ConvertFrom-Json
$version = $json.Version.Split('.')
$version[2] = [int]$version[2] + 1
$json.Version = [string]::Join(".",$version)

ConvertTo-Json $json -Compress | Out-File (Join-Path $outputDir plugin.json)
Copy-Item (Join-Path $outputDir plugin.json) ../../ -Verbose -Force

Write-Output "Updated version to $($json.Version)"

$woxPluginLocation = Join-Path $outputDir "WoxOpenWindow"
Write-Output "Plugin Location: $woxPluginLocation"
$woxPluginZipLocation = "$woxPluginLocation.zip"
$woxPluginWoxLocation = "$woxPluginLocation.wox"

Get-ChildItem -Path $outputDir -Exclude "*.zip","*.wox" | `
	Compress-Archive -DestinationPath $woxPluginZipLocation -Verbose -Force; mv $woxPluginZipLocation $woxPluginWoxLocation -Force -verbose
Copy-Item $woxPluginWoxLocation ../../../ -verbose