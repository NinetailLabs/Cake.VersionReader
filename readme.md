##Cake.VersionReader

An addin for [CakeBuild](http://cakebuild.net/) that allows for the easy retrieval of version numbers from an assembly. Also support creating CI version number based that fit with SemVer based on [this](http://www.xavierdecoster.com/semantic-versioning-auto-incremented-nuget-package-versions) article.

You can easilty reference Cake.VersionReader directly in you build script using Cake's Addin syntax:
```csharp
#addin "Cake.FileHelpers"
```

###Methods
The following methods are provided:
*GetVersionNumber(this ICakeContext context, FilePath file)*
- Get the version number for the spesified file in the format "0.0.0"

*GetVersionNumberWithContinuesIntegrationNumberAppended(this ICakeContext context, FilePath file, int buildNumber)*
- Get the CI version number in the format "0.0.0-CI00000"

###Icon
[Cyclops](https://thenounproject.com/term/cyclops/60203/) by [Mike Hince](https://thenounproject.com/zer0mike/) from the Noun Project, remixed with the Cake icon.