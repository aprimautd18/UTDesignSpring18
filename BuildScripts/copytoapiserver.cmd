@echo off
echo "Copying angular files to main project"
xcopy ..\ImprovedSchedulingSystemClient\app ..\ImprovedSchedulingSystemApi\ImprovedSchedulingSystemApi\wwwroot\ /s /e
REM powershell -Command "(gc myFile.txt) -replace 'foo', 'bar' | Out-File myFile.txt"
pause