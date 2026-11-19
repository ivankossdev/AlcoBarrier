# Runs XML parser checks (harness scripts, no test project).
#
# Steps:
#   1. builds AlcoBarrier.csproj (Debug) if needed;
#   2. compiles tests\XmlParserHarness.cs against bin\Debug\AlcoBarrier.exe;
#   3. runs the harness in a temp folder (so logs never land in the repo);
#   4. exits 0 when all checks pass, otherwise 1.
#
# Usage: powershell -ExecutionPolicy Bypass -File tests\run-xml-parser-tests.ps1

[CmdletBinding()]
param(
    [switch]$KeepArtifacts,
    [switch]$SkipBuild
)

$ErrorActionPreference = 'Stop'

$root = Split-Path $PSScriptRoot -Parent
$project = Join-Path $root 'AlcoBarrier\AlcoBarrier.csproj'
$exe = Join-Path $root 'AlcoBarrier\bin\Debug\AlcoBarrier.exe'
$vswhere = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"

function Find-MSBuild {
    if (Test-Path $vswhere) {
        $found = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' |
            Select-Object -First 1
        if ($found) { return $found }
    }

    $command = Get-Command msbuild -ErrorAction SilentlyContinue
    if ($command) { return $command.Source }

    return $null
}

function Find-RoslynCsc {
    if (Test-Path $vswhere) {
        $vsPath = & $vswhere -latest -products * -property installationPath
        if ($vsPath) {
            $candidate = Join-Path $vsPath 'MSBuild\Current\Bin\Roslyn\csc.exe'
            if (Test-Path $candidate) { return $candidate }
        }
    }

    throw 'Roslyn csc.exe not found (Visual Studio / Build Tools are required).'
}

if (-not $SkipBuild) {
    $msbuild = Find-MSBuild
    if (-not $msbuild) { throw 'MSBuild not found. Build AlcoBarrier.csproj manually first.' }

    Write-Host "Build: $project"
    & $msbuild $project /t:Build /p:Configuration=Debug /v:q /nologo
    if ($LASTEXITCODE -ne 0) { throw 'AlcoBarrier build failed.' }
}

if (-not (Test-Path $exe)) {
    throw "Not found: $exe - build the project in Debug configuration first."
}

$csc = Find-RoslynCsc
$work = Join-Path $env:TEMP 'alcobarrier-xml-harness'
Remove-Item -Recurse -Force $work -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Force -Path $work | Out-Null
Copy-Item $exe (Join-Path $work 'AlcoBarrier.exe') -Force

$harnessExe = Join-Path $work 'XmlParserHarness.exe'
Write-Host "Compile harness: $harnessExe"
& $csc /nologo "/out:$harnessExe" /target:exe "/r:$exe" (Join-Path $PSScriptRoot 'XmlParserHarness.cs')
if ($LASTEXITCODE -ne 0) { throw 'Harness compilation failed.' }

Push-Location $work
try {
    & $harnessExe
    $exitCode = $LASTEXITCODE
}
finally {
    Pop-Location
}

if ($KeepArtifacts) {
    Write-Host "Artifacts kept in $work"
}
else {
    Remove-Item -Recurse -Force $work -ErrorAction SilentlyContinue
}

if ($exitCode -ne 0) {
    Write-Host 'Parser tests: FAILED'
    exit 1
}

Write-Host 'Parser tests: OK'
exit 0
