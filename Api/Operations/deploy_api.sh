#!/bin/bash

# copying config file
cp /opt/xRandNet/automation/appsettings.json /opt/xRandNet/code/Api/appsettings.json
cp /opt/xRandNet/automation/appsettings.json /opt/xRandNet/code/Api/appsettings.Development.json

cd /opt/xRandNet/code/Api

dotnet build 2>&1 > /dev/null

msbuild

dotnet ef database update --no-build

dotnet publish --no-build

cp /var/log/xrandnet/xrandnet.log /var/log/xrandnet/xrandnet_`date +%F_%T`.log

rm /var/log/xrandnet/xrandnet.log

mkdir -p /opt/xRandNet/api

cp -R /opt/xRandNet/code/Api/bin/Debug/netcoreapp3.1/publish/* /opt/xRandNet/api

systemctl daemon-reload

service xrandnet restart
