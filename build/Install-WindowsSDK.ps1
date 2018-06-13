mkdir c:\winsdktemp

$client = new-object System.Net.WebClient
$client.DownloadFile("https://go.microsoft.com/fwlink/p/?linkid=870807","c:\winsdktemp\winsdksetup.exe")

Start-Process -Wait "c:\winsdktemp\winsdksetup.exe" "/features OptionId.UWPCpp /q"