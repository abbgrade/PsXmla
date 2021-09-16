@{
    RootModule = 'PsXmla.dll'
    ModuleVersion = '0.1.0'
    GUID = '88c14d97-eb95-428b-bbd0-99737f1cf5fd'
    DefaultCommandPrefix = 'Xmla'
    Author = 'Steffen Kampmann'
    Copyright = '(c) 2021 Steffen Kampmann. Alle Rechte vorbehalten.'
    Description = 'PsXmla connects XMLA and PowerShell. It gives you PowerShell Cmdlets with the power of Microsoft.AnalysisServices.Tabular'
    PowerShellVersion = '7.0'
    
    CmdletsToExport = @(
        'Connect-Server', 
        'Disconnect-Server', 
        'Invoke-Command'
    )

    PrivateData = @{

        PSData = @{
            Category = 'Databases'
            Tags = @('xmla', 'sqlserver', 'powerbi')
            LicenseUri = 'https://github.com/abbgrade/PsXmla/blob/main/LICENSE'
            ProjectUri = 'https://github.com/abbgrade/PsXmla'
            IsPrerelease = 'True'
        }
    }
}