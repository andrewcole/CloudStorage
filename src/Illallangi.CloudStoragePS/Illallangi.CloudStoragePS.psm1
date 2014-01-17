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

Function Backup-Flickr
{
	param(
		[string]$UserName = (Get-FlickrPeopleInfo).UserName
	)
	Process
	{
		$user = Get-FlickrPeopleInfo -UserName "$($UserName)"
		foreach ($photoSet in Get-FlickrPhotoSet -UserId $user.UserId)
		{
			foreach ($photo in Get-FlickrPhoto -PhotoSetId $photoSet.PhotoSetId)
			{
				$set = (Get-FlickrPhotoset -PhotoId $photo.PhotoId | Sort-Object { $_.NumberOfPhotos } | Select-Object -Last 1)
				Write-Host "$($set.Title)\$($photo.Title) ($($photo.PhotoId)).$($photo.OriginalFormat.ToLower())"
				New-Item -Type Directory -Path "$($user.UserName)\$($set.Title)" -ErrorAction SilentlyContinue
				$photo.Description | Out-File -FilePath "$($user.UserName)\$($set.Title)\$($photo.Title) ($($photo.PhotoId)).html"
				Backup-FlickrPhoto -Photo $photo -Path "$($user.UserName)\$($set.Title)\$($photo.Title) ($($photo.PhotoId)).$($photo.OriginalFormat)" -Type Small
			}
		}
	}
}