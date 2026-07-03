$path='C:\Users\user\.nuget\packages\devexpress.blazor\25.1.3\lib\net8.0\DevExpress.Blazor.v25.1.dll'
if(-not (Test-Path $path)){
    Write-Host 'MISSING_ASSEMBLY'
    exit 1
}
$a=[Reflection.Assembly]::LoadFrom($path)
try{
    $a.GetTypes() | Where-Object { $_.FullName -match 'Load' } | ForEach-Object { Write-Host $_.FullName }
} catch [Reflection.ReflectionTypeLoadException] {
    $_.Exception.LoaderExceptions | ForEach-Object { Write-Host "LoaderEx: $($_.Message)" }
}
