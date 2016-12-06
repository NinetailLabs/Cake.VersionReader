## Cake.VersionReader

[![Build status](https://ci.appveyor.com/api/projects/status/ns29kdfwrpfts3kh?svg=true)](https://ci.appveyor.com/project/DeadlyEmbrace/cake-versionreader)
[![NuGet](https://img.shields.io/nuget/v/Cake.VersionReader.svg)](https://www.nuget.org/packages/Cake.VersionReader/)

An addin for [Cake](http://cakebuild.net/) that allows for the easy retrieval of version numbers from an assembly. Also support creating CI version numbers that fit with SemVer based on [this](http://www.xavierdecoster.com/semantic-versioning-auto-incremented-nuget-package-versions) article.

You can easily reference Cake.VersionReader directly in your build script using Cake's Addin syntax:
```csharp
#addin nuget:?package=Cake.VersionReader
```

### Methods
The following methods are provided:

- GetVersionNumber(this ICakeContext context, FilePath file)

*Get the version number for the spesified file in the SemVer format "0.0.0"*

- GetFullVersionNumber(this ICakeContext context, FilePath file)

*Get the version number for the spesified file in 4-digit format "0.0.0.0"*

- GetVersionNumberWithContinuesIntegrationNumberAppended(this ICakeContext context, FilePath file, int buildNumber)

*Get the CI version number in the format "0.0.0-CI00000"*

- GetFullVersionNumberWithContinuesIntegrationNumberAppended(this ICakeContext context, FilePath file, int buildNumber)

*Get the CI version number in the format "0.0.0.0-CI00000"*

### Icon
[Cyclops](https://thenounproject.com/term/cyclops/60203/) by [Mike Hince](https://thenounproject.com/zer0mike/) from the Noun Project, remixed with the Cake icon.
