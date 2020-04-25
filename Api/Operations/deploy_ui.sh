#!/bin/bash

cd /opt/xRandNet/code/Ui

npm install

ng build --prod

mkdir -p /opt/xRandNet/ui

cp -R /opt/xRandNet/code/Ui/dist/xRandNet/* /opt/xRandNet/ui

service nginx restart
