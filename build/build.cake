#tool nuget:?package=GitVersion.CommandLine
#addin "Cake.FileHelpers"

using System;
using System.Linq;
using System.Text.RegularExpressions;

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var baseDir = "..";
var buildDir = baseDir + "\\build";
var Solution = baseDir + "\\UWP Community Toolkit.sln";
var binDir = baseDir + "\\bin";
var tempDir = binDir + "\\temp";
var binariesDir = MakeAbsolute(Directory(binDir + "\\binaries")).ToString();
var nupkgDir = MakeAbsolute(Directory(binDir + "\\nupkg")).ToString();

var signClientSettings = MakeAbsolute(File("SignClientSettings.json")).ToString();
var signClientSecret = EnvironmentVariable("SignClientSecret");
var signClientAppPath = tempDir + "\\SignClient\\Tools\\SignClient.dll";

GitVersion Version = null;
var name = "UWP Community Toolkit";
var address = "https://developer.microsoft.com/en-us/windows/uwp-community-toolkit";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

void VerifyHeaders(bool Replace)
{
    var header = FileReadText("header.txt") + "\r\n";
    bool hasMissing = false;

    Func<IFileSystemInfo, bool> exclude_objDir =
        fileSystemInfo => !fileSystemInfo.Path.Segments.Contains("obj");

    var files = GetFiles(baseDir + "\\**\\*.cs", exclude_objDir).Where(file => 
    {
        var path = file.ToString();
        return !(path.EndsWith(".g.cs") || path.EndsWith(".i.cs") || System.IO.Path.GetFileName(path).Contains("TemporaryGeneratedFile"));
    });

    Information("\nChecking " + files.Count() + " file header(s)");
    foreach(var file in files)
    {
        var oldContent = FileReadText(file);
        var rgx = new Regex("^(//.*\r?\n|\r?\n)*");
        var newContent = header + rgx.Replace(oldContent, "");

        if(!newContent.Equals(oldContent, StringComparison.Ordinal))
        {
            if(Replace)
            {
                Information("\nUpdating " + file + " header...");
                FileWriteText(file, newContent);
            }
            else
            {
                Error("Wrong/missing header on " + file);
                hasMissing = true;
            }
        }
    }

    if(!Replace && hasMissing)
    {
        throw new Exception("Please run '.\\build.ps1 -target=UpdateHeaders' and commit the changes.");
    }
}

Task("UpdateHeaders")
    .Description("Updates the headers in *.cs files")
    .Does(() =>
{
    VerifyHeaders(true);
});

Task("Clean")
    .Description("Clean the output folder")
    .Does(() =>
{
    if(DirectoryExists(binDir))
    {
        Information("\nCleaning Working Directory");
        CleanDirectory(binDir);
    }
    else
    {
        CreateDirectory(binDir);
    }
});

Task("Verify")
    .Description("Run pre-build verifications")
    .IsDependentOn("Clean")
    .Does(() =>
{
    VerifyHeaders(false);
});

Task("Version")
    .Description("Updates the version entries in AssemblyInfo.cs files")
    .IsDependentOn("Verify")
    .Does(() =>
{
    Version = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true
    });
});

Task("Build")
    .Description("Build all projects and get the assemblies")
    .IsDependentOn("Version")
    .Does(() =>
{
    EnsureDirectoryExists(binariesDir);

    Information("\nBuilding Solution");
    MSBuild(Solution, configurator =>
        configurator.SetConfiguration("Release")
            .SetVerbosity(Verbosity.Quiet)
            .SetMSBuildPlatform(MSBuildPlatform.x86)
            .WithTarget("Clean;Restore;Build")
            .WithProperty("GenerateSolutionSpecificOutputFolder", "true")
            .WithProperty("GenerateLibraryLayout", "true")
            .WithProperty("TreatWarningsAsErrors", "false")
            .WithProperty("OutDir", binariesDir));
});

void CreateNugetPackages()
{
    var nuGetPackSettings = new NuGetPackSettings
    {
        OutputDirectory = nupkgDir,
        Symbols = true,
        Properties = new Dictionary<string, string>
        {
            { "binaries", binariesDir }
        }
    };

    if(Version != null)
    {
        nuGetPackSettings.Version = Version.NuGetVersionV2;
    }

    var nupsecs = GetFiles(buildDir + "\\*.nuspec");
    Information("\n Packing " + nupsecs.Count() + " Packages");
    foreach(var nuspec in nupsecs)
    {
        Information("\n Packing " + nuspec);
        NuGetPack(nuspec, nuGetPackSettings);
    }
}

Task("PackNuGet")
    .Description("Create the NuGet packages")
    .IsDependentOn("Build")
    .Does(() =>
{
    CreateNugetPackages();
});

Task("PackNuGetNoBuild")
    .Description("Create the NuGet packages with existing binaries")
    .Does(() =>
{
    EnsureDirectoryExists(nupkgDir);
    Version = GitVersion();
    CreateNugetPackages();
});

Task("SignNuGet")
    .Description("Sign the NuGet packages with the Code Signing service")
    .IsDependentOn("PackNuGet")
    .Does(() =>
{
    if(!string.IsNullOrWhiteSpace(signClientSecret))
    {
        Information("\nDownloading Sign Client...");
        var installSettings = new NuGetInstallSettings {
            ExcludeVersion  = true,
            OutputDirectory = tempDir,
            Prerelease = true,
            Version = "0.5.0-beta4"
        };
        NuGetInstall(new []{"SignClient"}, installSettings);

        var packages = GetFiles(nupkgDir + "\\*.nupkg"); 
        Information("\n Signing " + packages.Count() + " Packages");      
        foreach(var package in packages)
        {
            Information("\nSubmitting " + package + " for signing...");
            DotNetCoreTool(signClientAppPath, "zip", "-c " + signClientSettings + " -s " + signClientSecret + " -n '" + name + "' -d '" + name +"' -u '" + address + "'");
            Information("\nFinished signing " + package);
        }
    }
    else
    {
        Warning("\nClient Secret not found, not signing packages...");
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("PackNuGet");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
