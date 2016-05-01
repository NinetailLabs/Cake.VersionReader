//Addins
#addin Cake.VersionReader

var sln = "./Cake.VersionReader/Cake.VersionReader.sln";
var nuspec = "./Cake.VersionReader/Cake.VersionReader.nuspec";

var target = Argument ("target", "Build");

Task ("Build")
	.Does (() => {
		NuGetRestore (sln);
		DotNetBuild (sln, c => c.Configuration = "Release");
	});

Task ("Nuget")
	.IsDependentOn ("Build")
	.Does (() => {
		CreateDirectory ("./nupkg/");

		NuGetPack (nuspec, new NuGetPackSettings { 
			Verbosity = NuGetVerbosity.Detailed,
			OutputDirectory = "./nupkg/",
			// NuGet messes up path on mac, so let's add ./ in front again
			BasePath = "././",
		});	
	});

Task ("Push")
	.IsDependentOn ("Nuget").Does (() => {
		// Get the newest (by last write time) to publish
		var newestNupkg = GetFiles ("nupkg/*.nupkg")
			.OrderBy (f => new System.IO.FileInfo (f.FullPath).LastWriteTimeUtc)
			.LastOrDefault ();

		var apiKey = TransformTextFile ("./.nugetapikey").ToString ();

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
	.IsDependentOn("Build");

RunTarget (target);