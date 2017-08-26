#tool "nuget:?package=GitVersion.CommandLine"
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

var baseDir = MakeAbsolute(Directory("../")).ToString();
var buildDir = baseDir + "/build";
var Solution = baseDir + "/UWP Community Toolkit.sln";
var binDir = baseDir + "/bin";
var tempDir = binDir + "/temp";
var binariesDir = binDir + "/binaries";
var nupkgDir = binDir + "/nupkg";

var signClientSettings = MakeAbsolute(File("SignClientSettings.json")).ToString();
var signClientSecret = EnvironmentVariable("SignClientSecret");
var signClientAppPath = tempDir + "/SignClient/Tools/netcoreapp1.1/SignClient.dll";

var styler = tempDir + "/XamlStyler.Console/tools/xstyler.exe";
var stylerFile = baseDir + "/settings.xamlstyler";

GitVersion Version = null;
var name = "UWP Community Toolkit";
var address = "https://developer.microsoft.com/en-us/windows/uwp-community-toolkit";

//////////////////////////////////////////////////////////////////////
// Methods
//////////////////////////////////////////////////////////////////////

void VerifyHeaders(bool Replace)
{
    var header = FileReadText("header.txt") + "\r\n";
    bool hasMissing = false;

    Func<IFileSystemInfo, bool> exclude_objDir =
        fileSystemInfo => !fileSystemInfo.Path.Segments.Contains("obj");

    var files = GetFiles(baseDir + "/**/*.cs", exclude_objDir).Where(file => 
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
                Error("\nWrong/missing header on " + file);
                hasMissing = true;
            }
        }
    }

    if(!Replace && hasMissing)
    {
        throw new Exception("Please run '.\\build.ps1 -target=UpdateHeaders' and commit the changes.");
    }
}

void CreateNugetPackages()
{
    var nuGetPackSettings = new NuGetPackSettings
    {
        OutputDirectory = nupkgDir,
        Properties = new Dictionary<string, string>
        {
            { "binaries", binariesDir }
        }
    };

    if(Version != null)
    {
        nuGetPackSettings.Version = Version.NuGetVersionV2;
    }

    var nupsecs = GetFiles("*.nuspec");
    Information("\nPacking " + nupsecs.Count() + " Packages");
    foreach(var nuspec in nupsecs)
    {
        NuGetPack(nuspec, nuGetPackSettings);
    }
}

//////////////////////////////////////////////////////////////////////
// Default Task
//////////////////////////////////////////////////////////////////////

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
    var buildSettings = new MSBuildSettings
    {
        MaxCpuCount = 0
    }
    .SetConfiguration("Release")
    .WithTarget("Clean;Restore;Build")
    .WithProperty("GenerateSolutionSpecificOutputFolder", "true")
    .WithProperty("GenerateLibraryLayout", "true")
    .WithProperty("TreatWarningsAsErrors", "false")
    .WithProperty("OutDir", binariesDir);

    MSBuild(Solution, buildSettings);
});

Task("PackNuGet")
    .Description("Create the NuGet packages")
    .IsDependentOn("Build")
    .Does(() =>
{
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
            Version = "0.8.0"
        };
        NuGetInstall(new []{"SignClient"}, installSettings);

        var packages = GetFiles(nupkgDir + "/*.nupkg"); 
        Information("\n Signing " + packages.Count() + " Packages");      
        foreach(var package in packages)
        {
            Information("\nSubmitting " + package + " for signing...");
            DotNetCoreTool(signClientAppPath, "sign", "-c " + signClientSettings + " -s " + signClientSecret + " -n '" + name + "' -d '" + name +"' -u '" + address + "'");
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

Task("UpdateHeaders")
    .Description("Updates the headers in *.cs files")
    .Does(() =>
{
    VerifyHeaders(true);
});

Task("PackNuGetNoBuild")
    .Description("Create the NuGet packages with existing binaries")
    .Does(() =>
{
    EnsureDirectoryExists(nupkgDir);
    Version = GitVersion();
    CreateNugetPackages();
});

Task("StyleXaml")
    .Description("Ensures XAML Formatting is Clean")
    .Does(() =>
{
    if(!FileExists(styler))
    {
        Information("\nDownloading XamlStyler...");
        var installSettings = new NuGetInstallSettings {
            ExcludeVersion  = true,
            OutputDirectory = tempDir
        };
        
        NuGetInstall(new []{"xamlstyler.console"}, installSettings);
    }

    Func<IFileSystemInfo, bool> exclude_objDir =
        fileSystemInfo => !fileSystemInfo.Path.Segments.Contains("obj");

    var files = GetFiles(baseDir + "/**/*.xaml", exclude_objDir);
    Information("\nChecking " + files.Count() + " file(s) for XAML Structure");
    foreach(var file in files)
    {
        StartProcess(styler, "-f \"" + file + "\" -c \"" + stylerFile + "\"");
    }
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
