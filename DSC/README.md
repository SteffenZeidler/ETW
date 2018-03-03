# PowerShell Desired State Configuration (DSC) for .NET EventSource tracing
Permanent tracing of a .NET EventSource provider to a ETL file.

# Steps to setup configuration in PowerShell
- .\DscEtwTrace.ps1
  - script contains Configuration EtwTrace
- EtwTrace -EventSourceName ... -FilePath ...
- runas administrator
- Enable-PSRemoting
- Start-DscConfiguration .\EtwTrace -wait
- Set-DscLocalConfigurationManager .\EtwTrace

# DSC documentation links
- https://docs.microsoft.com/en-us/powershell/dsc/scriptresource
- https://docs.microsoft.com/en-us/powershell/dsc/metaconfig
- https://blogs.technet.microsoft.com/pstips/2017/03/01/using-dsc-with-the-winrm-service-disabled

# Usefull commands for testing
- Get-DscConfigurationStatus -All
- Get-DscLocalConfigurationManager
- Event Viewer: Microsoft-Windows-Desired State Configuration/Operational
- Get-DscConfiguration
- Remove-DscConfigurationDocument -Stage Current
