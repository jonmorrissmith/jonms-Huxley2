# Changes put in place for execution on a Raspberry Pi

A fork of [James Singleton's masterful Huxley2](https://github.com/jpsingleton/Huxley2) with changes I needed to make to allow it to run locally via Docker on a Raspberry Pi.

## Changes to Dockerfile ##

Changed to run using alpine-arm64 as alpine-x64 woudn't build on a Pi (or a Mac).

`Docker compose up` terminated with the following fatal error:

```
failed to solve: process "/bin/sh -c dotnet publish -c Release -o out   --no-restore   
--runtime alpine-arm64   --self-contained true   /p:PublishTrimmed=true   
/p:PublishSingleFile=true" did not complete successfully: exit code: 1
```
To remedy this I had to set `PublishTrimmed=false`.

## Changes to Huxley2.csproj ##

Initially the build complained 
   `==> ERROR [huxley2 build-env 4/6] RUN dotnet restore --runtime alpine-arm64`

Accoding to [.NET SDK uses a smaller RID graph](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/8.0/rid-graph) alpine-arm64 is deprecated.

Followed the recommended action and added 
`<UseRidGraph>true</UseRidGraph>`
to the PropertyGroup.

## Changes to Program.cs ##

Added explicit port binding for Linux - connections weren't reaching the processes without this (not sure if this is Linux and/or Docker specific)
```
if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
   {
      // Explicit port binding for Linux environments (with Docker)
      options.ListenAnyIP(80);
   }
```
Also added some blocks to uncomment if you want to see oodles of debugging info.
I'll not reproduce that here, just look in the code if needed

## Changes to appsettings.json ##

Added debugging options (using Trace for Kestrel is very useful
