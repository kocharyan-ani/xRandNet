#!/bin/bash

dotnet ef migrations add Migration_`date +%F_%T` --no-build 
