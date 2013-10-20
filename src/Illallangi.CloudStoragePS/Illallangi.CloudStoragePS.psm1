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