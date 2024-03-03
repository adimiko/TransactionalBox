#!/bin/bash
cd source/$1

dotnet pack -c release /p:PackageVersion=$GITHUB_REF_NAME --no-restore -o .

dotnet nuget push *.nupkg -k $NUGET_API_KEY -s https://api.nuget.org/v3/index.json