#!/bin/bash

rm -rf /opt/xRandNet/xRandNet

git clone https://github.com/kocharyan-ani/xRandNet.git /opt/xRandNet/xRandNet

# copying config file
# config is stored on a server because of security reasons it is not committed to repo
cp /opt/xRandNet/automation/appsettings.json /opt/xRandNet/xRandNet/Api/appsettings.json

#run dotnet command to create corresponding directories for mono build
cd /opt/xRandNet/xRandNet/Api

dotnet build 2>&1 > /dev/null

msbuild

dotnet publish --no-build