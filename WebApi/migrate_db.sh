#!/bin/bash
msbuild &&
dotnet ef migrations add `date +%F` --no-build && 
msbuild &&
dotnet ef database update --no-build