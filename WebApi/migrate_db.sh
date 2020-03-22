#!/bin/bash
msbuild &&
dotnet ef migrations add Migration_`date +%F_%T` --no-build && 
msbuild &&
dotnet ef database update --no-build