Configuration EtwTrace {
    [CmdletBinding()]
    param
    (
        [ValidateSet('Present', 'Absent')]
        [String]
        $Ensure = 'Present',

        [Parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [String]
        $EventSourceName,
        
        [Parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [String]
        $FilePath,

        [int]
        $MaximumFileSize = 100
    )

    Import-DscResource -ModuleName PsDesiredStateConfiguration

    Node localhost
    {

        Script Script
        {
            SetScript = 
            {
                $EventSourceName = $Using:EventSourceName
                if ($Using:Node.Ensure -eq 'Absent')
                {
                    Stop-EtwTraceSession -Name $EventSourceName
                }
                else # 'Present'
                {
                    $EventSourceGuid = [System.Diagnostics.Tracing.EventSource]::new($EventSourceName).Guid.ToString('B')
                    $EVENT_TRACE_FILE_MODE_NEWFILE = 0x00000008
                    $Time = [DateTime]::Now.ToString('yyyyMMdd\-HHmm')
                    $Path = [System.IO.Path]::Combine($Using:FilePath, "$EventSourceName-$Time-%d.etl")
                    New-EtwTraceSession -Name $EventSourceName -LocalFilePath $Path -LogFileMode $EVENT_TRACE_FILE_MODE_NEWFILE -MaximumFileSize $Using:MaximumFileSize
                    Add-EtwTraceProvider -SessionName $EventSourceName -Guid $EventSourceGuid
                }
            }

            TestScript =
            {
                $session = Get-EtwTraceSession $Using:EventSourceName -ErrorAction SilentlyContinue
                Write-Verbose -Message "EtwTraceSession LocalFilePath: $($session.LocalFilePath)"
                return $session -ne $null
            }

            GetScript =
            {
                $session = Get-EtwTraceSession $Using:EventSourceName -ErrorAction SilentlyContinue
                @{ Result = $session.LocalFilePath }
            }          
        }

        LocalConfigurationManager
        {
            ConfigurationMode = 'ApplyAndAutoCorrect'
        }
    }
}
