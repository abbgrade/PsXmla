# PsXmla

PsXmla connects XMLA and PowerShell. It gives you PowerShell Cmdlets with the power of [Microsoft.AnalysisServices.Tabular](https://www.nuget.org/packages/Microsoft.AnalysisServices.Tabular/). For example you can automate your work with [Tabular Editor](https://github.com/TabularEditor/TabularEditor) and the [Power BI XMLA endpoints](https://docs.microsoft.com/en-us/power-bi/admin/service-premium-connect-tools).

## Installation

This module can be installed from [PsGallery](https://www.powershellgallery.com/packages/PsXmla).

```powershell
Install-Module -Name PsXmla -Scope CurrentUser
```

Alternatively it can be build and installed from source.

1. Install the development dependencies
2. Download or clone it from GitHub
3. Run the installation task:

```powershell
Invoke-Build Install
```

## Usage

TODO

### Commands

| Command                      | Description                               | Status  |
| ---------------------------- | ----------------------------------------- | ------- |
| Connect-Instance             | Create a new database connection.         | &#9745; |
| Disconnect-Instance          | Close connection                          | &#9745; |
| Invoke-Command               | Execute SQLCMD scripts                    | &#9744; |
| &#11185; Retry support       | Specify the number of retry attempts      | &#9745; |
| &#11185; Power BI exceptions | Parse and handle exceptions from Power BI | &#9744; |

### Build

The build scripts require InvokeBuild. If it is not installed, install it with the command `Install-Module InvokeBuild -Scope CurrentUser`.

You can build the module using the VS Code build task or with the command `Invoke-Build Build`.

## Development

- This is a [Portable Module](https://docs.microsoft.com/de-de/powershell/scripting/dev-cross-plat/writing-portable-modules?view=powershell-7) based on [PowerShell Standard](https://github.com/powershell/powershellstandard) and [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard).
- [VSCode](https://code.visualstudio.com) is recommended as IDE. [VSCode Tasks](https://code.visualstudio.com/docs/editor/tasks) are configured.
- Build automation is based on [InvokeBuild](https://github.com/nightroman/Invoke-Build)
- Test automation is based on [Pester](https://pester.dev)
- Commands are named based on [Approved Verbs for PowerShell Commands](https://docs.microsoft.com/de-de/powershell/scripting/developer/cmdlet/approved-verbs-for-windows-powershell-commands)
