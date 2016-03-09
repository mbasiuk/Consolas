<#
.SYNOPSIS
Unistall consolas application
#>

param([string]$TargetPath="$env:ProgramFiles\Consolas", [string]$WorkingDirectory="$env:ProgramData\Consolas", [string]$Shortcut="$env:USERPROFILE\Desktop\Consolas.lnk")

ri $TargetPath -Recurse -Force
ri $WorkingDirectory -Recurse -Force
ri $Shortcut -Recurse -Force