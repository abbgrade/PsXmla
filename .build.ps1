<#
.Synopsis
	Build script <https://github.com/nightroman/Invoke-Build>
#>

param(
	[ValidateSet('Debug', 'Release')]
	[string] $Configuration = 'Debug',

	[string] $NuGetApiKey = $env:nuget_apikey,

	# Version suffix to prereleases
	[int] $BuildNumber,

	[switch] $ForceDocInit
)

$ModuleName = 'PsXmla'

. $PSScriptRoot\tasks\Build.Tasks.ps1
. $PSScriptRoot\tasks\Dependencies.Tasks.ps1
. $PSScriptRoot\tasks\PsBuild.Tasks.ps1

# Synopsis: Default task.
task . Build
