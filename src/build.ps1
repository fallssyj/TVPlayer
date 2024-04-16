param (
	[Parameter()]
	[ValidateNotNullOrEmpty()]
	[string]
	$OutputPath = '.\bin\TVPlayer'
)

if ( Test-Path -Path .\bin\TVPlayer) {
    rm -Recurse -Force $OutputPath
}

Write-Host 'Building'

dotnet publish `
	TVPlayer.csproj `
	-c Release `
	--self-contained false `
	-o $OutputPath

if ( -Not $? ) {
	exit $lastExitCode
	}

if ( Test-Path -Path .\bin\TVPlayer) {
    rm -Force "$OutputPath\*.pdb"
    rm -Force "$OutputPath\*.xml"
}

Write-Host 'Build done'

cp LICENSE "$OutputPath\LICENSE"

7z a .\bin\TVPlayer.7z $OutputPath

ls $OutputPath



exit 0