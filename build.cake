//Addins
#addin nuget:?package=Cake.VersionReader&version=5.1.0
#addin nuget:?package=Cake.FileHelpers&version=3.2.0
#tool "nuget:?package=NUnit.ConsoleRunner&version=3.10.0"

#region Paths

var tools = "./tools";
var sln = "./Cake.VersionReader/Cake.VersionReader.sln";
var nuspec = "./Cake.VersionReader/Cake.VersionReader/Cake.VersionReader.nuspec";
var releaseFolder = "./Cake.VersionReader/Cake.VersionReader/bin/Release/netstandard2.0";
var releaseDll = "/Cake.VersionReader.dll";
var unitTestPaths = "./Cake.VersionReader/Cake.VersionReader.Test/bin/Release/Cake.VersionReader.Tests.dll";
var testResultFile = "./TestResult.xml";

#endregion

#region Arguments

var target = Argument ("target", "Build");
var buildType = Argument<string>("buildType", "develop");
var buildCounter = Argument<int>("buildCounter", 0);

#endregion

#region Runtime Variables

var version = "0.0.0";
var ciVersion = "0.0.0-CI00000";
var runningOnAppVeyor = false;
var runningOnTeamCity = false;
var testSucceeded = true;

#endregion

#region Tasks

// Find out if we are running on a Build Server
Task("DiscoverBuildDetails")
	.Does(() =>
	{
		runningOnTeamCity = TeamCity.IsRunningOnTeamCity;
		Information("Running on TeamCity: " + runningOnTeamCity);
		runningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
		Information("Running on AppVeyor: " + runningOnAppVeyor);
	});

Task ("Build")
.IsDependentOn("DiscoverBuildDetails")
	.Does (() => {
		NuGetRestore (sln);
		MSBuild (sln, new MSBuildSettings {
			ToolVersion = MSBuildToolVersion.VS2017,
			Configuration = "Release"
		});
		var file = MakeAbsolute(Directory(releaseFolder)) + releaseDll;
		version = GetVersionNumber(file);
		ciVersion = GetVersionNumberWithContinuesIntegrationNumberAppended(file, buildCounter);
		Information("Version: " + version);
		Information("CI Version: " + ciVersion);
		PushVersion(ciVersion);
	});

//Execute Unit tests
Task("UnitTest")
	.IsDependentOn("Build")
	.Does(() =>
	{
		StartBlock("Unit Testing");
		
		var testAssemblies = GetFiles(unitTestPaths);
		
		NUnit3(testAssemblies, new NUnit3Settings {
			OutputFile = testResultFile,
			WorkingDirectory = ".",
			Work = MakeAbsolute(Directory("."))
		});

		PushTestResults(testResultFile);
		
		EndBlock("Unit Testing");
	});
	
Task ("Nuget")
	.IsDependentOn ("UnitTest")
	.Does (() => {
		CreateDirectory ("./nupkg/");
		ReplaceRegexInFiles(nuspec, "0.0.0", version);
		
		NuGetPack (nuspec, new NuGetPackSettings { 
			Verbosity = NuGetVerbosity.Detailed,
			OutputDirectory = "./nupkg/"
		});	
	});

Task ("Push")
	.WithCriteria(buildType == "master")
	.IsDependentOn ("Nuget")
	.Does (() => {
		// Get the newest (by last write time) to publish
		var newestNupkg = GetFiles ("nupkg/*.nupkg")
			.OrderBy (f => new System.IO.FileInfo (f.FullPath).LastWriteTimeUtc)
			.LastOrDefault();

		var apiKey = EnvironmentVariable("NugetKey");

		NuGetPush (newestNupkg, new NuGetPushSettings { 
			Verbosity = NuGetVerbosity.Detailed,
			Source = "https://www.nuget.org/api/v2/package/",
			ApiKey = apiKey
		});
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

#endregion

RunTarget (target);

#region Helper Methods

public void StartBlock(string blockName)
{
		if(runningOnTeamCity)
		{
			TeamCity.WriteStartBlock(blockName);
		}
}

public void StartBuildBlock(string blockName)
{
	if(runningOnTeamCity)
	{
		TeamCity.WriteStartBuildBlock(blockName);
	}
}

public void EndBlock(string blockName)
{
	if(runningOnTeamCity)
	{
		TeamCity.WriteEndBlock(blockName);
	}
}

public void EndBuildBlock(string blockName)
{
	if(runningOnTeamCity)
	{
		TeamCity.WriteEndBuildBlock(blockName);
	}
}

public void PushVersion(string version)
{
	if(runningOnTeamCity)
	{
		TeamCity.SetBuildNumber(version);
	}
	if(runningOnAppVeyor)
	{
		Information("Pushing version to AppVeyor: " + version);
		AppVeyor.UpdateBuildVersion(version);
	}
}

public void PushTestResults(string filePath)
{
	var file = MakeAbsolute(File(filePath));
	if(runningOnAppVeyor)
	{
		AppVeyor.UploadTestResults(file, AppVeyorTestResultsType.NUnit3);
	}
}

#endregion