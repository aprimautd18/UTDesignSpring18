@echo off
echo "Copying angular files to main project"
xcopy ..\ImprovedSchedulingSystemClient\app ..\ImprovedSchedulingSystemApi\ImprovedSchedulingSystemApi\wwwroot\ /s /e
