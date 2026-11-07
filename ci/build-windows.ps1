param([string]$OutputDirectory, [switch]$SmokeTest, [int]$SmokeSeconds = 60)
$ErrorActionPreference = 'Stop'
# CapriFR fork: a response file keeps long folder paths under the command line limit.
function Invoke-Compiler([string[]]$arguments) {
    $file = [IO.Path]::GetTempFileName()
    [IO.File]::WriteAllLines($file, ($arguments | ForEach-Object { '"' + $_ + '"' }), (New-Object Text.UTF8Encoding($false)))
    try { & $compiler ('@' + $file) } finally { Remove-Item -LiteralPath $file -Force }
}
$root = Split-Path -Parent $PSScriptRoot
$framework = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319'
$compiler = Join-Path $framework 'csc.exe'
$wpf = Join-Path $framework 'WPF'
$winmd = Get-ChildItem -LiteralPath (Join-Path ${env:ProgramFiles(x86)} 'Windows Kits\10\UnionMetadata') -Directory -ErrorAction SilentlyContinue | Where-Object { $_.Name -match '^\d+(\.\d+)+$' } | Sort-Object { [version]$_.Name } -Descending | ForEach-Object { Join-Path $_.FullName 'Windows.winmd' } | Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1
if (-not (Test-Path -LiteralPath $compiler)) { throw 'csc.exe not found. Capri needs the .NET Framework 4.8 that comes with Windows 10 and 11.' }
$localWinmd = Join-Path $root 'tools\windows-sdk\Windows.winmd'
$usingLocal = (-not $winmd) -and (Test-Path -LiteralPath $localWinmd)
$shortSdk = Join-Path ([IO.Path]::GetTempPath()) 'caprifr-winsdk'
if ($usingLocal) {
    # csc.exe refuses paths over 260 characters, so it reads a copy from a short folder.
    New-Item -ItemType Directory -Path $shortSdk -Force | Out-Null
    foreach ($file in [IO.Directory]::GetFiles((Split-Path -Parent $localWinmd), '*.winmd')) { [IO.File]::Copy($file, (Join-Path $shortSdk ([IO.Path]::GetFileName($file))), $true) }
    $winmd = Join-Path $shortSdk 'Windows.winmd'
}
if (-not $winmd) { throw 'Windows.winmd not found. Install the Windows 10 or 11 SDK, or put it in tools\windows-sdk.' }
if ([string]::IsNullOrWhiteSpace($OutputDirectory)) { $OutputDirectory = Join-Path $root 'out\windows' }
$out = [IO.Path]::GetFullPath($OutputDirectory)
New-Item -ItemType Directory -Path $out -Force | Out-Null
$app = Join-Path $out 'CapriFR.exe'
$setup = Join-Path $out 'CapriFR-Setup.exe'
$icon = Join-Path $root 'assets\chip.ico'
$shared = @('/nologo', '/target:winexe', '/platform:anycpu', '/optimize+', '/utf8output', '/warn:4', ('/win32manifest:' + (Join-Path $root 'app\app.manifest')), ('/win32icon:' + $icon), '/reference:System.dll', '/reference:System.Core.dll', '/reference:System.Drawing.dll', '/reference:System.Windows.Forms.dll')
$references = @('/reference:System.Runtime.Serialization.dll', '/reference:System.IO.Compression.dll', '/reference:System.IO.Compression.FileSystem.dll', '/reference:System.Security.dll', ('/reference:' + (Join-Path $wpf 'UIAutomationClient.dll')), ('/reference:' + (Join-Path $wpf 'UIAutomationTypes.dll')), ('/reference:' + (Join-Path $wpf 'WindowsBase.dll')), ('/reference:' + (Join-Path $wpf 'System.Speech.dll')), ('/reference:' + $winmd), ('/reference:' + (Join-Path $framework 'System.Runtime.dll')), ('/reference:' + (Join-Path $framework 'System.Runtime.WindowsRuntime.dll')), ('/reference:' + (Join-Path $framework 'System.Runtime.InteropServices.WindowsRuntime.dll')))
$localContracts = if ($usingLocal) { Get-ChildItem -LiteralPath $shortSdk -Filter *.winmd | Where-Object { $_.Name -ne 'Windows.winmd' } | ForEach-Object { '/reference:' + $_.FullName } } else { @() }
$references += $localContracts
$resources = @(('/resource:' + $icon + ',CapriFR.Chip.ico'), ('/resource:' + (Join-Path $root 'assets\fonts\ShareTechMono-Regular.ttf') + ',CapriFR.Font.Mono.ttf'))
$resources += ('/resource:' + (Join-Path $root 'assets\cursor.png') + ',CapriFR.Cursor.png')
$resources += ('/resource:' + (Join-Path $root 'assets\vbcable\VBCABLE_Driver_Pack45.zip') + ',CapriFR.VBCABLE.zip')
$resources += Get-ChildItem -LiteralPath (Join-Path $root 'assets\sprites') -Filter *.png | ForEach-Object { '/resource:' + $_.FullName + ',CapriFR.Sprite.' + $_.Name }
$resources += Get-ChildItem -LiteralPath (Join-Path $root 'assets\voice') -Filter *.wav | ForEach-Object { '/resource:' + $_.FullName + ',CapriFR.Voice.' + $_.Name }
$sources = @('Capri', 'FrMoves', 'Asides', 'Bolts', 'Commentary', 'Lookout', 'Brain', 'Talk', 'Mainframe', 'MainframeMods', 'MainframeModsWindow', 'Mods', 'ModDirectory', 'CodeMods', 'SoundsLike', 'SkinMods', 'ArtColors', 'MainframeTips', 'TipChecks', 'MediumChecks', 'FreeKeyChecks', 'Neon', 'Notice', 'Engine', 'Ledges', 'Voice', 'Ears', 'MicMix', 'CableSetup', 'Dictation', 'HearingStatus', 'Agent', 'Hands', 'GameHud', 'Computer', 'Arrange', 'Mixer', 'Usage', 'Network', 'Play', 'VoicePack', 'Startup', 'Presence', 'Player', 'Ocr', 'GameEars', 'GameNotes', 'GameKeys', 'Frames', 'Reminders', 'GameSense', 'GameWatch', 'SelfKnow', 'Care', 'Clips', 'ClipSettings', 'ClipChecks', 'Minis', 'Chess', 'ChessBoard', 'Files', 'FileFinder', 'Websites', 'Ink', 'Clones', 'ChaosScreen', 'Icons', 'FollowUps', 'WatchAlong', 'CallMode', 'Husk', 'CallEars', 'CallPanel', 'Reactions', 'ToolChecks', 'SmokeRun', 'AssemblyInfo') | ForEach-Object { Join-Path $root ('app\' + $_ + '.cs') }
Invoke-Compiler ($shared + $references + $resources + @('/unsafe+', ('/out:' + $app)) + $sources)
if ($LASTEXITCODE -ne 0) { throw 'CapriFR compilation failed.' }
Invoke-Compiler ($shared + @(('/out:' + $setup), '/reference:Microsoft.CSharp.dll', '/reference:System.IO.Compression.dll', '/reference:System.IO.Compression.FileSystem.dll', ('/resource:' + $app + ',CapriFR.App.exe'), ('/resource:' + $icon + ',CapriFR.Chip.ico'), ('/resource:' + (Join-Path $root 'assets\chip.png') + ',CapriFR.Chip.png'), (Join-Path $root 'app\Setup.cs'), (Join-Path $root 'app\Neon.cs'), (Join-Path $root 'app\Notice.cs'), (Join-Path $root 'app\VoicePack.cs'), (Join-Path $root 'app\Startup.cs'), (Join-Path $root 'app\AssemblyInfo.cs')))
if ($LASTEXITCODE -ne 0) { throw 'Installer compilation failed.' }
Write-Output "Built $app and $setup"
if (-not $SmokeTest) { return }
$runtime = Join-Path $out 'smoke\runtime'
$package = Join-Path $out 'smoke\package'
$check = Start-Process -FilePath $app -ArgumentList @('--smoke-test', ('"' + $runtime + '"')) -PassThru -WindowStyle Hidden
if (-not $check.WaitForExit($SmokeSeconds * 1000)) { $check.Kill(); throw 'Runtime smoke test timed out.' }
if (Test-Path -LiteralPath (Join-Path $runtime 'smoke-test.txt')) { Get-Content -LiteralPath (Join-Path $runtime 'smoke-test.txt') }
elseif ($check.ExitCode -eq 0) { throw 'Runtime smoke test wrote no results. Another smoke test probably held the lock; rerun it when that one ends.' }
if ($check.ExitCode -ne 0) {
    if (Test-Path -LiteralPath (Join-Path $runtime 'failure.txt')) { Get-Content -LiteralPath (Join-Path $runtime 'failure.txt') }
    throw 'Runtime smoke test failed.'
}
$verify = Start-Process -FilePath $setup -ArgumentList @('--verify', ('"' + $package + '"')) -PassThru -WindowStyle Hidden
if (-not $verify.WaitForExit($SmokeSeconds * 1000)) { $verify.Kill(); throw 'Installer check timed out.' }
if ($verify.ExitCode -ne 0) {
    if (Test-Path -LiteralPath (Join-Path $package 'failure.txt')) { Get-Content -LiteralPath (Join-Path $package 'failure.txt') }
    throw 'Installer check failed.'
}
if ((Get-FileHash -LiteralPath (Join-Path $package 'CapriFR.exe')).Hash -ne (Get-FileHash -LiteralPath $app).Hash) { throw 'Installer payload differs from the app executable.' }
Get-Content -LiteralPath (Join-Path $package 'packaging-test.txt')
Write-Output 'PASS installer payload matches the app'
