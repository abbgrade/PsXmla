@{
    RootModule = 'PsXmla.dll'
    ModuleVersion = '0.1.0'
    GUID = '88c14d97-eb95-428b-bbd0-99737f1cf5fd'
    PowerShellVersion = '7.0'
    DefaultCommandPrefix = 'Xmla'
    CmdletsToExport = @('Connect-Server', 'Disconnect-Server', 'Invoke-Command')
    RequiredAssemblies = @('Microsoft.AnalysisServices.Platform.Core.dll', 'Microsoft.AnalysisServices.Platform.Windows.dll')
}