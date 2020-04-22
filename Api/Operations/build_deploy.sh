#!/bin/bash

rm -rf /opt/xRandNet/xRandNet

git clone https://github.com/kocharyan-ani/xRandNet.git /opt/xRandNet/xRandNet

# copying config file
cp /opt/xRandNet/automation/appsettings.json /opt/xRandNet/xRandNet/Api/appsettings.json
cp /opt/xRandNet/automation/appsettings.json /opt/xRandNet/xRandNet/Api/appsettings.Development.json

#run dotnet command to create corresponding directories for mono build
cd /opt/xRandNet/xRandNet/Api

dotnet build 2>&1 > /dev/null

#build project with mono
msbuild

dotnet ef database update --no-build

dotnet publish --no-build

cd /opt/xRandNet/xRandNet/Ui

npm install

ng build --prod

service nginx restart

service xrandnet restart