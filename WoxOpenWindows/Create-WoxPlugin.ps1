param($outputDir)

Write-Output "OutputDir: $outputDir"
$woxPluginLocation = Join-Path $outputDir "WoxOpenWindow"
Write-Output "Plugin Location: $woxPluginLocation"
$woxPluginZipLocation = "$woxPluginLocation.zip"
$woxPluginWoxLocation = "$woxPluginLocation.wox"

Get-ChildItem -Path $outputDir -Exclude "*.zip","*.wox" | `
	Compress-Archive -DestinationPath $woxPluginZipLocation -Verbose -Force; mv $woxPluginZipLocation $woxPluginWoxLocation -Force -verbose