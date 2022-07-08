Describe PsXmla {
    It 'is valid' {
        Test-ModuleManifest $PSScriptRoot\..\publish\PsXmla\PsXmla.psd1
    }

    It 'can be imported' {
        Import-Module $PSScriptRoot\..\publish\PsXmla\PsXmla.psd1
    }

    Context 'loaded modules' {
        BeforeAll {
            Import-Module $PSScriptRoot\..\publish\PsXmla\PsXmla.psd1 -Verbose
        }

        It 'has commands' {
            $commands = Get-Command -Module PsXmla
            $commands | Should -Not -BeNullOrEmpty
        }
    }
}
