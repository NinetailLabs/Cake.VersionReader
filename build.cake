//Addins
#addin Cake.VersionReader
#addin Cake.FileHelpers

var sln = "./Cake.VersionReader/Cake.VersionReader.sln";
var nuspec = "./Cake.VersionReader/Cake.VersionReader.nuspec";
var releaseFolder = "./Cake.VersionReader/Cake.VersionReader/bin/Release";
var releaseDll = "/Cake.VersionReader.dll";
var nuspecFile = "./Cake.VersionReader/Cake.VersionReader.nuspec";

var target = Argument ("target", "Build");
var buildType = Argument<string>("buildType", "develop");

var version = "0.0.0";

Task ("Build")
	.Does (() => {
		NuGetRestore (sln);
		DotNetBuild (sln, c => c.Configuration = "Release");
		var file = MakeAbsolute(Directory(releaseFolder)) + releaseDll;
		version = GetVersionNumber(file);
		Information("Version: " + version);
	});

Task ("Nuget")
	.WithCriteria(buildType == "master")
	.IsDependentOn ("Build")
	.Does (() => {
		CreateDirectory ("./nupkg/");
		ReplaceRegexInFiles(nuspecFile, "0.0.0", version);
		
		NuGetPack (nuspec, new NuGetPackSettings { 
			Verbosity = NuGetVerbosity.Detailed,
			OutputDirectory = "./nupkg/"
		});	
	});

Task ("Push")
	.WithCriteria(buildType == "master")
	.IsDependentOn ("Nuget").Does (() => {
		// Get the newest (by last write time) to publish
		var newestNupkg = GetFiles ("nupkg/*.nupkg")
			.OrderBy (f => new System.IO.FileInfo (f.FullPath).LastWriteTimeUtc)
			.LastOrDefault ();

		var apiKey = TransformTextFile ("c:/nuget/nugetapikey").ToString ();

		//NuGetPush (newestNupkg, new NuGetPushSettings { 
		//	Verbosity = NuGetVerbosity.Detailed,
		//	ApiKey = apiKey
		//});
	});

Task ("Clean").Does (() => 
{
	CleanDirectories ("./**/bin");
	CleanDirectories ("./**/obj");

	CleanDirectories ("./**/Components");
	CleanDirectories ("./**/tools");

	DeleteFiles ("./**/*.apk");
});

Task("Default")
	.IsDependentOn("Push");

RunTarget (target);