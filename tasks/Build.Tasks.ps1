requires Configuration

[System.IO.FileInfo] $global:Manifest = "$PSScriptRoot/../src/PsXmla/bin/$Configuration/net5.0/publish/PsXmla.psd1"


# Synopsis: Build project.
task Build {
	exec { dotnet publish ./src/PsXmla -c $Configuration }
}

# Synopsis: Remove files.
task Clean {
	remove src/PsXmla/bin, src/PsXmla/obj
}

# Synopsis: Install the module.
task Install -Jobs Build, {
    $info = Import-PowerShellDataFile $global:Manifest.FullName
    $version = ([System.Version] $info.ModuleVersion)
    $name = $global:Manifest.BaseName
    $defaultModulePath = $env:PsModulePath -split ';' | Select-Object -First 1
    $installPath = Join-Path $defaultModulePath $name $version.ToString()
    New-Item -Type Directory $installPath -Force | Out-Null
    Get-ChildItem $global:Manifest.Directory | Copy-Item -Destination $installPath -Recurse -Force
}

# Synopsis: Publish the module to PSGallery.
task Publish -Jobs Install, {

	assert ( $Configuration -eq 'Release' )

	Publish-Module -Name PsXmla -NuGetApiKey $NuGetApiKey
}