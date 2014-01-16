if ($null -eq (Get-Module PSCompletion))
{
	Write-Debug "Import-Module PSCompletion -Global"
	Import-Module PSCompletion -Global -ErrorAction SilentlyContinue
	if ($null -eq (Get-Module PSCompletion))
	{
		Write-Warning "PSCompletion module not found; tab completion will be unavailable."
	}
}

if ($null -ne (Get-Module PSCompletion))
{
	Register-ParameterCompleter Get-FlickrPeopleInfo UserName {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-FlickrAccessToken | Where-Object { $_.UserName -like "$wordToComplete*" } | Sort-Object { $_.UserName } |%{ New-CompletionResult "$($_.UserName)" }
	}
}

Function Get-DropBoxToken
{
	Begin
	{
		Get-DropBoxRequestToken | 
			Open-DropBoxAuthorizeUrl | 
			Wait-AnyKey -Prompt "Complete the authorization process on the DropBox website and press any key to continue..." | 
			Get-DropBoxAccessToken |
			Set-DropBoxAccessToken
	}
}

Function Get-FlickrToken
{
	Begin
	{
		Get-FlickrFrob | 
			Open-FlickrAuthorizeUrl | 
			Wait-AnyKey -Prompt "Complete the authorization process on the Flickr website and press any key to continue..." | 
			Get-FlickrAccessToken |
			Set-FlickrAccessToken
	}
}

Function Push-ChildItem
{
	param(
		[string]$EMail,
		[string]$Destination,
		[string[]]$Path
	)
	Process
	{
		Foreach ($p in $Path)
		{
			Get-ChildItem -Path $p | Push-FileToDropBox -Email $EMail -Destination $Destination
		}
	}
}