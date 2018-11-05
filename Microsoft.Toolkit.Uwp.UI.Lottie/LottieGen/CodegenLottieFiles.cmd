:: Example cmd script for generating code for a folder.

setlocal
set lottiegen=LottieGen.exe
set inputFolderRoot=\lottie\lottiefiles\corpus\
set outputFolder=temp
mkdir %outputFolder% 2>NUL

:: Walks over all the directories under %inputFolderRoot%
:: Outputs the path to the file being processed to STDERR
for /r %inputFolderRoot% %%f in (*.json) do (echo %%f 1>&2 & %lottiegen% -i "%%f" -l cs -o %outputFolder%)




