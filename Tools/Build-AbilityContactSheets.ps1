param([Parameter(Mandatory=$true)][string]$Report,[string]$FFmpeg='C:/Users/Jose/AppData/Local/Temp/rea-ffmpeg-9.0.2/ffmpeg-9.0.2-essentials_build/bin/ffmpeg.exe')
$ErrorActionPreference='Stop'
$aimReportPath=[IO.Path]::GetFullPath($Report)
$aimPrefix=$aimReportPath.Substring(0,$aimReportPath.Length-[IO.Path]::GetExtension($aimReportPath).Length)
$aimData=Get-Content -LiteralPath $aimReportPath -Raw | ConvertFrom-Json
$aimFighters=@($aimData.observations | Where-Object step -eq 'aim-move-entry' | ForEach-Object {$_.detail.Split(';')[0]} | Select-Object -Unique)
foreach($aimFighter in $aimFighters){
 $aimPaths=@(0..9 | ForEach-Object {"$aimPrefix.aim-$aimFighter-$_.png"})
 $aimMissing=@($aimPaths | Where-Object {-not(Test-Path -LiteralPath $_)})
 if($aimMissing.Count){Write-Warning "$aimFighter has missing captures: $($aimMissing.Count)";continue}
 $aimOutput="$aimPrefix.contact-$aimFighter.png"
 & $FFmpeg -hide_banner -loglevel error -framerate 1 -i "$aimPrefix.aim-$aimFighter-%d.png" -vf 'scale=384:216,tile=5x2' -frames:v 1 -y $aimOutput
 if($LASTEXITCODE -ne 0){throw "Contact generation failed for $aimFighter"}
 $aimOutput
}

