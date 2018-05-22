$client = new-object System.Net.WebClient
$client.DownloadFile("https://go.microsoft.com/fwlink/p/?linkid=870807","c:\temp\winsdksetup.exe")

& "c:\temp\winsdksetup.exe" /features OptionId.UWPCpp /q