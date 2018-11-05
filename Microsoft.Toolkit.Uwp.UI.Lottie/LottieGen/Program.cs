// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using Microsoft.Toolkit.Uwp.UI.Lottie.LottieData;
using Microsoft.Toolkit.Uwp.UI.Lottie.LottieData.Serialization;
using Microsoft.Toolkit.Uwp.UI.Lottie.LottieData.Tools;
using Microsoft.Toolkit.Uwp.UI.Lottie.LottieToWinComp;
using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData;
using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.CodeGen;
using Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

sealed class Program
{
    static readonly Assembly s_thisAssembly = Assembly.GetExecutingAssembly();
    readonly CommandLineOptions _options;
    readonly TextWriter _infoStream;
    readonly TextWriter _errorStream;
    readonly Profiler _profiler;

    enum RunResult
    {
        Success,
        InvalidUsage,
        Failure,
    }

    static int Main(string[] args)
    {
        var infoStream = Console.Out;
        var errorStream = Console.Out;

        switch (Run(CommandLineOptions.ParseCommandLine(args), infoStream: infoStream, errorStream: errorStream))
        {
            case RunResult.Success:
                return 0;

            case RunResult.Failure:
                return 1;

            case RunResult.InvalidUsage:
                errorStream.WriteLine();
                ShowUsage(errorStream);
                return 1;

            default:
                // Should never get here.
                throw new InvalidOperationException();
        }
    }


    static IEnumerable<string> ExpandWildcards(string path)
    {
        var directoryPath = Path.GetDirectoryName(path);
        if (string.IsNullOrWhiteSpace(directoryPath))
        {
            directoryPath = ".";
        }
        return Directory.EnumerateFiles(directoryPath, Path.GetFileName(path));
    }

    // Helper for writing info lines to the info stream.
    void WriteInfo(string infoMessage)
    {
        _infoStream.WriteLine(infoMessage);
    }

    // Writes a new line to the info stream.
    void WriteInfoNewLine()
    {
        _infoStream.WriteLine();
    }

    // Helper for writing errors to the error stream with a standard format.
    void WriteError(string errorMessage)
    {
        _errorStream.WriteLine($"Error: {errorMessage}");
    }

    Program(CommandLineOptions options, TextWriter infoStream, TextWriter errorStream)
    {
        _options = options;
        _infoStream = infoStream;
        _errorStream = errorStream;
        _profiler = new Profiler();
    }

    static RunResult Run(CommandLineOptions options, TextWriter infoStream, TextWriter errorStream)
    {
        return new Program(options, infoStream, errorStream).Run();
    }

    RunResult Run()
    {
        // Sign on
        var assemblyVersion = s_thisAssembly.GetName().Version;

        var toolNameAndVersion = $"Lottie for Windows Code Generator version {assemblyVersion}";
        WriteInfo(toolNameAndVersion);
        WriteInfoNewLine();

        if (_options.ErrorDescription != null)
        {
            // Failed to parse the command line.
            WriteError("Invalid arguments.");
            _errorStream.WriteLine(_options.ErrorDescription);
            return RunResult.InvalidUsage;
        }
        else if (_options.HelpRequested)
        {
            ShowHelp(_infoStream);
            return RunResult.Success;
        }

        // Check for required args
        if (_options.InputFile == null)
        {
            WriteError("Lottie file not specified.");
            return RunResult.InvalidUsage;
        }

        // Validate the languages.
        if (!_options.Languages.Any())
        {
            WriteError("Language not specified.");
            return RunResult.InvalidUsage;
        }

        foreach (var language in _options.Languages)
        {
            if (language == Lang.Unknown)
            {
                WriteError("Invalid language.");
                return RunResult.InvalidUsage;
            }
        }

        // Check that at least one file matches InputFile.
        var matchingInputFiles = ExpandWildcards(_options.InputFile).ToArray();
        if (matchingInputFiles.Length == 0)
        {
            WriteError($"File not found: {_options.InputFile}");
            return RunResult.Failure;
        }

        // Get the output folder as an absolute path, defaulting to the current directory
        // if no output folder was specified.
        var outputFolder = MakeAbsolutePath(_options.OutputFolder ?? Directory.GetCurrentDirectory());


        // Assume success.
        var result = RunResult.Success;

        foreach (var file in matchingInputFiles)
        {
            // Get the input file as an absolute path.
            var inputFile = MakeAbsolutePath(file);

            Microsoft.Toolkit.Uwp.UI.Lottie.LottieData.Tools.Stats lottieStats;
            Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools.Stats beforeOptimizationStats;
            Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools.Stats afterOptimizationStats;

            var codeGenResult = TryGenerateCode(
                        lottieJsonFilePath: inputFile,
                        outputFolder: outputFolder,
                        codeGenClassName: _options.ClassName,
                        strictTranslation: _options.StrictMode,
                        lottieStats: out lottieStats,
                        beforeOptimizationStats: out beforeOptimizationStats,
                        afterOptimizationStats: out afterOptimizationStats)
                    ? RunResult.Success
                    : RunResult.Failure;

            // Output extra information if the user specified verbose output.
            if (_options.Verbose)
            {
                if (_profiler.HasAnyResults)
                {
                    WriteInfoNewLine();
                    WriteInfo(" === Timings ===");
                    _profiler.WriteReport(_infoStream);
                }

                if (lottieStats != null)
                {
                    WriteInfoNewLine();
                    WriteLottieStatsReport(_infoStream, lottieStats);
                }

                if (beforeOptimizationStats != null && afterOptimizationStats != null)
                {
                    WriteInfoNewLine();
                    WriteCodeGenStatsReport(_infoStream, beforeOptimizationStats, afterOptimizationStats);
                }
            }

            if (result == RunResult.Success && codeGenResult != RunResult.Success)
            {
                result = codeGenResult;
            }
        }
        return result;
    }

    static void WriteLottieStatsReport(
        TextWriter writer,
        Microsoft.Toolkit.Uwp.UI.Lottie.LottieData.Tools.Stats stats)
    {
        writer.WriteLine(" === Lottie info ===");
        WriteStatsStringLine("BodyMovin Version", stats.Version.ToString());
        WriteStatsStringLine("Name", stats.Name);
        WriteStatsStringLine("Size", $"{stats.Width} x {stats.Height}");
        WriteStatsStringLine("Duration", $"{stats.Duration.TotalSeconds.ToString("#,##0.0##")} seconds");
        WriteStatsIntLine("Images", stats.ImageLayerCount);
        WriteStatsIntLine("PreComps", stats.PreCompLayerCount);
        WriteStatsIntLine("Shapes", stats.ShapeLayerCount);
        WriteStatsIntLine("Solids", stats.SolidLayerCount);
        WriteStatsIntLine("Nulls", stats.NullLayerCount);
        WriteStatsIntLine("Texts", stats.TextLayerCount);
        WriteStatsIntLine("Masks", stats.MaskCount);
        WriteStatsIntLine("MaskAdditive", stats.MaskAdditiveCount);
        WriteStatsIntLine("MaskDarken", stats.MaskDarkenCount);
        WriteStatsIntLine("MaskDifference", stats.MaskDifferenceCount);
        WriteStatsIntLine("MaskIntersect", stats.MaskIntersectCount);
        WriteStatsIntLine("MaskLighten", stats.MaskLightenCount);
        WriteStatsIntLine("MaskSubtract", stats.MaskSubtractCount);

        const int nameWidth = 19;
        void WriteStatsIntLine(string name, int value)
        {
            if (value > 0)
            {
                writer.WriteLine($"{name,nameWidth}  {value,6:n0}");
            }
        }

        void WriteStatsStringLine(string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                writer.WriteLine($"{name,nameWidth}  {value}");
            }
        }

    }

    static void WriteCodeGenStatsReport(
        TextWriter writer,
        Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools.Stats beforeOptimization,
        Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools.Stats afterOptimization)
    {
        if (beforeOptimization == null)
        {
            return;
        }

        writer.WriteLine(" === Translation output stats ===");

        writer.WriteLine("                      Type   Count  Optimized away");

        if (afterOptimization == null)
        {
            // No optimization was performed. Just report on the before stats.
            afterOptimization = beforeOptimization;
        }

        // Report on the after stats and indicate how much optimization
        // improved things (where it did).
        WriteStatsLine("CanvasGeometry", beforeOptimization.CanvasGeometryCount, afterOptimization.CanvasGeometryCount);
        WriteStatsLine("ColorBrush", beforeOptimization.ColorBrushCount, afterOptimization.ColorBrushCount);
        WriteStatsLine("ColorKeyFrameAnimation", beforeOptimization.ColorKeyFrameAnimationCount, afterOptimization.ColorKeyFrameAnimationCount);
        WriteStatsLine("CompositionPath", beforeOptimization.CompositionPathCount, afterOptimization.CompositionPathCount);
        WriteStatsLine("ContainerShape", beforeOptimization.ContainerShapeCount, afterOptimization.ContainerShapeCount);
        WriteStatsLine("ContainerVisual", beforeOptimization.ContainerVisualCount, afterOptimization.ContainerVisualCount);
        WriteStatsLine("CubicBezierEasingFunction", beforeOptimization.CubicBezierEasingFunctionCount, afterOptimization.CubicBezierEasingFunctionCount);
        WriteStatsLine("EllipseGeometry", beforeOptimization.EllipseGeometryCount, afterOptimization.EllipseGeometryCount);
        WriteStatsLine("ExpressionAnimation", beforeOptimization.ExpressionAnimationCount, afterOptimization.ExpressionAnimationCount);
        WriteStatsLine("GeometricClip", beforeOptimization.GeometricClipCount, afterOptimization.GeometricClipCount);
        WriteStatsLine("InsetClip", beforeOptimization.InsetClipCount, afterOptimization.InsetClipCount);
        WriteStatsLine("LinearEasingFunction", beforeOptimization.LinearEasingFunctionCount, afterOptimization.LinearEasingFunctionCount);
        WriteStatsLine("PathGeometry", beforeOptimization.PathGeometryCount, afterOptimization.PathGeometryCount);
        WriteStatsLine("PathKeyFrameAnimation", beforeOptimization.PathKeyFrameAnimationCount, afterOptimization.PathKeyFrameAnimationCount);
        WriteStatsLine("Property value", beforeOptimization.PropertySetProperyCount, afterOptimization.PropertySetProperyCount);
        WriteStatsLine("PropertySet", beforeOptimization.PropertySetCount, afterOptimization.PropertySetCount);
        WriteStatsLine("RectangleGeometry", beforeOptimization.RectangleGeometryCount, afterOptimization.RectangleGeometryCount);
        WriteStatsLine("RoundedRectangleGeometry", beforeOptimization.RoundedRectangleGeometryCount, afterOptimization.RoundedRectangleGeometryCount);
        WriteStatsLine("ScalarKeyFrameAnimation", beforeOptimization.ScalarKeyFrameAnimationCount, afterOptimization.ScalarKeyFrameAnimationCount);
        WriteStatsLine("ShapeVisual", beforeOptimization.ShapeVisualCount, afterOptimization.ShapeVisualCount);
        WriteStatsLine("SpriteShape", beforeOptimization.SpriteShapeCount, afterOptimization.SpriteShapeCount);
        WriteStatsLine("StepEasingFunction", beforeOptimization.StepEasingFunctionCount, afterOptimization.StepEasingFunctionCount);
        WriteStatsLine("Vector2KeyFrameAnimation", beforeOptimization.Vector2KeyFrameAnimationCount, afterOptimization.Vector2KeyFrameAnimationCount);
        WriteStatsLine("Vector3KeyFrameAnimation", beforeOptimization.Vector3KeyFrameAnimationCount, afterOptimization.Vector3KeyFrameAnimationCount);
        WriteStatsLine("ViewBox", beforeOptimization.ViewBoxCount, afterOptimization.ViewBoxCount);

        void WriteStatsLine(string name, int before, int after)
        {
            if (after > 0 || before > 0)
            {
                const int nameWidth = 26;
                if (before != after)
                {
                    writer.WriteLine($"{name,nameWidth}  {after,6:n0} {before - after,6:n0}");

                }
                else
                {
                    writer.WriteLine($"{name,nameWidth}  {after,6:n0}");
                }
            }
        }
    }


    bool TryGenerateCode(
        string lottieJsonFilePath,
        string outputFolder,
        string codeGenClassName,
        bool strictTranslation,
        out Microsoft.Toolkit.Uwp.UI.Lottie.LottieData.Tools.Stats lottieStats,
        out Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools.Stats beforeOptimizationStats,
        out Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools.Stats afterOptimizationStats)
    {
        lottieStats = null;
        beforeOptimizationStats = null;
        afterOptimizationStats = null;

        if (!TryEnsureDirectoryExists(outputFolder))
        {
            WriteError($"Failed to create the output directory: {outputFolder}");
            return false;
        }

        // Read the Lottie .json text.
        WriteInfo($"Reading Lottie file: {lottieJsonFilePath}");
        var jsonStream = TryReadTextFile(lottieJsonFilePath);

        if (jsonStream == null)
        {
            WriteError($"Failed to read Lottie file: {lottieJsonFilePath}");
            return false;
        }

        // Parse the Lottie.
        var lottieComposition =
            LottieCompositionReader.ReadLottieCompositionFromJsonStream(
                jsonStream,
                LottieCompositionReader.Options.IgnoreMatchNames,
                out var readerIssues);

        _profiler.OnParseFinished();

        foreach (var issue in readerIssues)
        {
            WriteInfo(IssueToString(lottieJsonFilePath, issue));
        }

        if (lottieComposition == null)
        {
            WriteError($"Failed to parse Lottie file: {lottieJsonFilePath}");
            return false;
        }

        lottieStats = new Microsoft.Toolkit.Uwp.UI.Lottie.LottieData.Tools.Stats(lottieComposition);

        var translateSucceeded = LottieToWinCompTranslator.TryTranslateLottieComposition(
                    lottieComposition,
                    strictTranslation, // strictTranslation
                    true, // TODO - make this configurable?  !excludeCodegenDescriptions, // add descriptions for codegen commments
                    out var winCompDataRootVisual,
                    out var translationIssues);

        _profiler.OnTranslateFinished();

        foreach (var issue in translationIssues)
        {
            WriteInfo(IssueToString(lottieJsonFilePath, issue));
        }

        if (!translateSucceeded)
        {
            WriteError("Failed to translate Lottie file.");
            return false;
        }

        beforeOptimizationStats = new Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools.Stats(winCompDataRootVisual);
        _profiler.OnUnmeasuredFinished();

        // Get an appropriate name for the class.
        string className =
            InstantiatorGeneratorBase.TrySynthesizeClassName(codeGenClassName) ??
            InstantiatorGeneratorBase.TrySynthesizeClassName(Path.GetFileNameWithoutExtension(lottieJsonFilePath)) ??
            // If all else fails, just call it Lottie.
            InstantiatorGeneratorBase.TrySynthesizeClassName("Lottie");

        // Optimize the code unless told not to.
        Visual optimizedWincompDataRootVisual = winCompDataRootVisual;
        if (!_options.DisableTranslationOptimizer)
        {
            optimizedWincompDataRootVisual = Optimizer.Optimize(winCompDataRootVisual, ignoreCommentProperties: true);
            _profiler.OnOptimizationFinished();

            afterOptimizationStats = new Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools.Stats(optimizedWincompDataRootVisual);
            _profiler.OnUnmeasuredFinished();
        }

        var lottieFileNameBase = Path.GetFileNameWithoutExtension(lottieJsonFilePath);

        var codeGenSucceeded = true;
        foreach (var lang in _options.Languages)
        {
            switch (lang)
            {
                case Lang.CSharp:
                    codeGenSucceeded &= TryGenerateCSharpCode(
                        className,
                        optimizedWincompDataRootVisual,
                        (float)lottieComposition.Width,
                        (float)lottieComposition.Height,
                        lottieComposition.Duration,
                        Path.Combine(outputFolder, $"{lottieFileNameBase}.cs"));
                    _profiler.OnCodeGenFinished();
                    break;

                case Lang.Cx:
                    codeGenSucceeded &= TryGenerateCXCode(
                        className,
                        optimizedWincompDataRootVisual,
                        (float)lottieComposition.Width,
                        (float)lottieComposition.Height,
                        lottieComposition.Duration,
                        Path.Combine(outputFolder, $"{lottieFileNameBase}.h"),
                        Path.Combine(outputFolder, $"{lottieFileNameBase}.cpp"));
                    _profiler.OnCodeGenFinished();
                    break;

                case Lang.LottieXml:
                    codeGenSucceeded &= TryGenerateLottieXml(
                        lottieComposition,
                        Path.Combine(outputFolder, $"{lottieFileNameBase}.xml"));
                    _profiler.OnSerializationFinished();
                    break;

                case Lang.WinCompXml:
                    codeGenSucceeded &= TryGenerateWincompXml(
                        optimizedWincompDataRootVisual,
                        Path.Combine(outputFolder, $"{lottieFileNameBase}.xml"));
                    _profiler.OnSerializationFinished();
                    break;

                case Lang.WinCompDgml:
                    codeGenSucceeded &= TryGenerateWincompDgml(
                        optimizedWincompDataRootVisual,
                        Path.Combine(outputFolder, $"{lottieFileNameBase}.dgml"));
                    _profiler.OnSerializationFinished();
                    break;

                case Lang.Stats:
                    codeGenSucceeded &= TryGenerateStats(
                        lottieJsonFilePath,
                        lottieStats,
                        afterOptimizationStats ?? beforeOptimizationStats,
                        readerIssues.Concat(translationIssues),
                        Path.Combine(outputFolder, lottieFileNameBase));
                    break;

                default:
                    WriteError($"Language {lang} is not supported.");
                    return false;
            }
        }

        return codeGenSucceeded;
    }

    static string GenerateHashFromString(string input)
    {
        using (var hasher = SHA256.Create())
        using (var stream = new MemoryStream())
        using (var writer = new StreamWriter(stream))
        {
            // Write into the stream so that hasher can consume the input.
            writer.Write(input);

            // Generate the hash. This returns a 32 byte (256 bit) value.
            var hash = hasher.ComputeHash(stream);

            // Encode the hash as base 64 so that it is all readable characters.
            // This will return a string that is 44 characters long.
            var hashedString = Convert.ToBase64String(hash);

            // Return just the first 8 characters of the base64 encoded hash.
            return hashedString.Substring(0, 8);
        }
    }

    /// <summary>
    /// Generates csv files describing the Lottie and its translation.
    /// </summary>
    bool TryGenerateStats(
        string lottieJsonFilePath,
        Microsoft.Toolkit.Uwp.UI.Lottie.LottieData.Tools.Stats lottieStats,
        Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools.Stats translationStats,
        IEnumerable<(string Code, string Description)> issues,
        string outputFilePathBase)
    {
        // Assume success.
        var success = true;

        // Create an id for this file, based on the path.
        // The key is used so that other tables (e.g. the errors table) can reference the entry
        // for this file.
        var key = GenerateHashFromString(lottieJsonFilePath).Substring(0, 8);

        // Create the main table. Other tables will reference rows in this table.
        // Note that the main table has only one row. Typical usage will be to
        // generate tables for many Lottie files then combine them in a script.
        var sb = new StringBuilder();
        sb.AppendLine(
            "Key,Path,FileName,LottieVersion,DurationSeconds,ErrorCount,PrecompLayerCount,ShapeLayerCount," +
            "SolidLayerCount,ImageLayerCount,TextLayerCount,MaskCount,ContainerShapeCount,ContainerVisualCount," +
            "ExpressionAnimationCount,PropertySetPropertyCount,SpriteShapeCount");
        sb.Append($"\"{key}\"");
        AppendColumnValue(lottieJsonFilePath);
        AppendColumnValue(Path.GetFileName(lottieJsonFilePath));
        AppendColumnValue(lottieStats.Version);
        AppendColumnValue(lottieStats.Duration.TotalSeconds);
        AppendColumnValue(issues.Count());
        AppendColumnValue(lottieStats.PreCompLayerCount);
        AppendColumnValue(lottieStats.ShapeLayerCount);
        AppendColumnValue(lottieStats.SolidLayerCount);
        AppendColumnValue(lottieStats.ImageLayerCount);
        AppendColumnValue(lottieStats.TextLayerCount);
        AppendColumnValue(lottieStats.MaskCount);
        AppendColumnValue(translationStats.ContainerShapeCount);
        AppendColumnValue(translationStats.ContainerVisualCount);
        AppendColumnValue(translationStats.ExpressionAnimationCount);
        AppendColumnValue(translationStats.PropertySetProperyCount);
        AppendColumnValue(translationStats.SpriteShapeCount);
        sb.AppendLine();

        WriteCsvFile("basicInfo", sb.ToString());

        // Create the error table.
        sb.Clear();
        sb.AppendLine("Key,ErrorCode,Description");
        foreach ((var Code, var Description) in issues)
        {
            sb.Append($"\"{key}\"");
            AppendColumnValue(Code);
            AppendColumnValue(Description);
            sb.AppendLine();
        }

        WriteCsvFile("errors", sb.ToString());

        void AppendColumnValue(object columnValue)
        {
            sb.Append($",\"{columnValue}\"");
        }

        void WriteCsvFile(string fileDifferentiator, string text)
        {
            var filePath = $"{outputFilePathBase}.{fileDifferentiator}.csv";

            success &= TryWriteTextFile(filePath, text);
            if (success)
            {
                WriteInfo($"Stats data written to {filePath}");
            }
        }

        return success;
    }

    bool TryGenerateLottieXml(
        LottieComposition lottieComposition,
        string outputFilePath)
    {
        var result = TryWriteTextFile(
            outputFilePath,
            LottieCompositionXmlSerializer.ToXml(lottieComposition).ToString());

        WriteInfo($"Lottie XML written to {outputFilePath}");

        return result;
    }

    bool TryGenerateWincompXml(
        Visual rootVisual,
        string outputFilePath)
    {
        var result = TryWriteTextFile(
            outputFilePath,
            CompositionObjectXmlSerializer.ToXml(rootVisual).ToString());

        WriteInfo($"WinComp XML written to {outputFilePath}");

        return result;
    }

    bool TryGenerateWincompDgml(
        Visual rootVisual,
        string outputFilePath)
    {
        var result = TryWriteTextFile(
            outputFilePath,
            CompositionObjectDgmlSerializer.ToXml(rootVisual).ToString());

        WriteInfo($"WinComp DGML written to {outputFilePath}");

        return result;
    }

    bool TryGenerateCSharpCode(
        string className,
        Visual rootVisual,
        float compositionWidth,
        float compositionHeight,
        TimeSpan duration,
        string outputFilePath)
    {

        var code = CSharpInstantiatorGenerator.CreateFactoryCode(
                    className,
                    rootVisual,
                    compositionWidth,
                    compositionHeight,
                    duration,
                    _options.DisableCodeGenOptimizer);

        if (string.IsNullOrWhiteSpace(code))
        {
            WriteError("Failed to create the C# code.");
            return false;
        }

        if (!TryWriteTextFile(outputFilePath, code))
        {
            WriteError($"Failed to write C# code to {outputFilePath}");
            return false;
        }

        WriteInfo($"C# code for class {className} written to {outputFilePath}");
        return true;
    }

    bool TryGenerateCXCode(
        string className,
        Visual rootVisual,
        float compositionWidth,
        float compositionHeight,
        TimeSpan duration,
        string outputHeaderFilePath,
        string outputCppFilePath)
    {
        CxInstantiatorGenerator.CreateFactoryCode(
                className,
                rootVisual,
                compositionWidth,
                compositionHeight,
                duration,
                Path.GetFileName(outputHeaderFilePath),
                out var cppText,
                out var hText,
                _options.DisableCodeGenOptimizer);

        if (string.IsNullOrWhiteSpace(cppText))
        {
            WriteError("Failed to generate the .cpp code.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(hText))
        {
            WriteError("Failed to generate the .h code.");
            return false;
        }

        if (!TryWriteTextFile(outputHeaderFilePath, hText))
        {
            WriteError($"Failed to write .h code to {outputHeaderFilePath}");
            return false;
        }

        if (!TryWriteTextFile(outputCppFilePath, cppText))
        {
            WriteError($"Failed to write .cpp code to {outputCppFilePath}");
            return false;
        }

        WriteInfo($"Header code for class {className} written to {outputHeaderFilePath}");
        WriteInfo($"Source code for class {className} written to {outputCppFilePath}");
        return true;
    }

    static bool TryEnsureDirectoryExists(string directoryPath)
    {
        try
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    Stream TryReadTextFile(string filePath)
    {
        try
        {
            return File.OpenRead(filePath);
        }
        catch (Exception e)
        {
            WriteError($"Failed to read from {filePath}");
            WriteError(e.Message);
            return null;
        }
    }

    bool TryWriteTextFile(string filePath, string contents)
    {
        try
        {
            File.WriteAllText(filePath, contents, Encoding.UTF8);
            return true;
        }
        catch (Exception e)
        {
            WriteError($"Failed to write to {filePath}");
            WriteError(e.Message);
            return false;
        }
    }

    static string MakeAbsolutePath(string path)
    {
        return Path.IsPathRooted(path) ? path : Path.Combine(Directory.GetCurrentDirectory(), path);
    }

    // Outputs an error or warning message describing the error with the file path, error code, and description.
    // The format is designed to be suitable for parsing by VS.
    static string IssueToString(string originatingFile, (string Code, string Description) issue)
    {
        return $"{originatingFile}: error {issue.Code}: {issue.Description}";
    }

    static void ShowHelp(TextWriter writer)
    {
        writer.WriteLine("Generates source code from Lottie .json files.");
        writer.WriteLine();
        ShowUsage(writer);
    }

    static void ShowUsage(TextWriter writer)
    {
        writer.WriteLine(Usage);
    }

    static string Usage => string.Format(@"
Usage: {0} -InputFile LOTTIEFILE -Language LANG [Other options]

OVERVIEW:
       Generates source code from Lottie files for playing in the AnimatedVisualPlayer. 
       LOTTIEFILE is a Lottie .json file. LOTTIEFILE may contain wildcards.
       LANG is one of cs, cppcx, winrtcpp, wincompxml, lottiexml, dgml, or stats.
       -Language LANG may be specified multiple times.

       [Other options]

         -Help         Print this help message and exit.
         -ClassName    Uses the given class name for the generated code. If not 
                       specified the name is synthesized from the name of the Lottie 
                       file. The class name will be sanitized as necessary to be valid
                       for the language and will also be used as the base name of 
                       the output file(s).
         -DisableTranslationOptimizer  
                       Disables optimization of the translation from Lottie to
                       Windows code. Mainly used to detect bugs in the optimizer.
         -DisableCodeGenOptimizer
                       Disables optimization done by the code generator. This is 
                       useful when the generated code is going to be hacked on.
         -OutputFolder Specifies the output folder for the generated files. If not
                       specified the files will be written to the current directory.
         -Strict       Fails on any parsing or translation issue. If not specified, 
                       a best effort will be made to create valid output, and any 
                       issues will be reported to STDOUT.
         -Verbose      Outputs extra info to STDOUT.

EXAMPLES:

       Generate Foo.cpp and Foo.h winrtcpp files in the current directory from the 
       Lottie file Foo.json:

         {0} -InputFile Foo.json -Language winrtcpp


       Keywords can be abbreviated and are case insensitive.
       Generate Grotz.cs in the C:\temp directory from the Lottie file Bar.json:

         {0} -i Bar.json -L cs -ClassName Grotz -o C:\temp", s_thisAssembly.ManifestModule.Name);



    // Measures time spent in each phase.
    sealed class Profiler
    {
        readonly Stopwatch _sw = Stopwatch.StartNew();
        // Bucket of time to dump time we don't want to measure. Never reported.
        TimeSpan _unmeasuredTime;
        TimeSpan _parseTime;
        TimeSpan _translateTime;
        TimeSpan _optimizationTime;
        TimeSpan _codegenTime;
        TimeSpan _serializationTime;

        internal void OnUnmeasuredFinished() => OnPhaseFinished(ref _unmeasuredTime);
        internal void OnParseFinished() => OnPhaseFinished(ref _parseTime);
        internal void OnTranslateFinished() => OnPhaseFinished(ref _translateTime);
        internal void OnOptimizationFinished() => OnPhaseFinished(ref _optimizationTime);
        internal void OnCodeGenFinished() => OnPhaseFinished(ref _codegenTime);
        internal void OnSerializationFinished() => OnPhaseFinished(ref _serializationTime);

        void OnPhaseFinished(ref TimeSpan counter)
        {
            counter = _sw.Elapsed;
            _sw.Restart();
        }

        // True if there is at least one time value.
        internal bool HasAnyResults
            => new[] {
                _parseTime,
                _translateTime,
                _optimizationTime,
                _codegenTime,
                _serializationTime
            }.Any(ts => ts > TimeSpan.Zero);

        internal void WriteReport(TextWriter writer)
        {
            WriteReportForPhase(writer, "parse", _parseTime);
            WriteReportForPhase(writer, "translate", _translateTime);
            WriteReportForPhase(writer, "optimization", _optimizationTime);
            WriteReportForPhase(writer, "codegen", _codegenTime);
            WriteReportForPhase(writer, "serialization", _serializationTime);
        }

        void WriteReportForPhase(TextWriter writer, string phaseName, TimeSpan value)
        {
            // Ignore phases that didn't occur.
            if (value > TimeSpan.Zero)
            {
                writer.WriteLine($"{value} spent in {phaseName}.");
            }
        }
    }
}


