#!/usr/bin/env bash
# https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish?tabs=netcore2x
printenv
RootFolder="${RELEASE_ROOT_FOLDER:-./}"
read -r VERSION<$RootFolder/necromancy.version
mkdir ./release
echo Version: $VERSION
ls
for RUNTIME in win-x86 win-x64 linux-x64 osx-x64; do
    # Server
    dotnet publish Necromancy.Cli/Necromancy.Cli.csproj /p:Version=$VERSION /p:FromMSBuild=true --runtime $RUNTIME --configuration Release --output ../publish/$RUNTIME-$VERSION/Server
    # ReleaseFiles
    cp -r ./ReleaseFiles/. ./publish/$RUNTIME-$VERSION/
    # Pack
    tar cjf ./release/$RUNTIME-$VERSION.tar.gz ./publish/$RUNTIME-$VERSION
done 
 ls