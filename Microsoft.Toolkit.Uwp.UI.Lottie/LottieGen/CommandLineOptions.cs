// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

internal enum Lang
{
    // Language wasn't recognized.
    Unknown,
    // Language specified was ambigious.
    Ambiguous,

    CSharp,
    Cx,
    WinrtCpp,
    LottieXml,
    WinCompXml,
    WinCompDgml,
    Stats,
}

sealed class CommandLineOptions
{
    readonly List<string> _languageStrings = new List<string>();

    // The parse error, or null if the parse succeeded.
    internal string ErrorDescription { get; private set; }
    internal string InputFile { get; private set; }
    internal IEnumerable<Lang> Languages { get; private set; }
    internal string ClassName { get; private set; }
    internal string OutputFolder { get; private set; }
    internal bool StrictMode { get; private set; }
    internal bool HelpRequested { get; private set; }
    internal bool DisableTranslationOptimizer { get; private set; }
    internal bool DisableCodeGenOptimizer { get; private set; }
    internal bool Verbose { get; private set; }

    enum Keyword
    {
        None,
        Ambiguous,
        Help,
        InputFile,
        Language,
        ClassName,
        OutputFolder,
        Strict,
        DisableTranslationOptimizer,
        DisableCodeGenOptimizer,
        Verbose,
    }

    // Returns the parsed command line. If ErrorDescription is non-null, then the parse failed.
    internal static CommandLineOptions ParseCommandLine(string[] args)
    {
        var result = new CommandLineOptions();
        result.ParseCommandLineStrings(args);

        // Convert the language strings to language values.
        var languageTokenizer = new CommandlineTokenizer<Lang>(Lang.Ambiguous)
                .AddKeyword("csharp", Lang.CSharp)
                .AddKeyword("cppcx", Lang.Cx)
                .AddKeyword("cx", Lang.Cx)
                .AddKeyword("winrtcpp", Lang.WinrtCpp)
                .AddKeyword("lottiexml", Lang.LottieXml)
                .AddKeyword("wincompxml", Lang.WinCompXml)
                .AddKeyword("dgml", Lang.WinCompDgml)
                .AddKeyword("stats", Lang.Stats);

        var languages = new List<Lang>();

        // Parse the language string.
        foreach (var languageString in result._languageStrings)
        {
            languageTokenizer.TryMatchKeyword(languageString, out var language);
            languages.Add(language);
            switch (language)
            {
                case Lang.Unknown:
                    result.ErrorDescription = $"Unrecognized language: {languageString}";
                    break;
                case Lang.Ambiguous:
                    result.ErrorDescription = $"Ambiguous language: {languageString}";
                    break;
            }
        }
        result.Languages = languages.Distinct();

        return result;
    }

    void ParseCommandLineStrings(string[] args)
    {
        // Define the keywords accepted on the command line.
        var tokenizer = new CommandlineTokenizer<Keyword>(Keyword.Ambiguous)
            .AddPrefixedKeyword("?", Keyword.Help)
            .AddPrefixedKeyword("help", Keyword.Help)
            .AddPrefixedKeyword("inputfile", Keyword.InputFile)
            .AddPrefixedKeyword("language", Keyword.Language)
            .AddPrefixedKeyword("classname", Keyword.ClassName)
            .AddPrefixedKeyword("outputfolder", Keyword.OutputFolder)
            .AddPrefixedKeyword("strict", Keyword.Strict)
            .AddPrefixedKeyword("disablecodegenoptimizer", Keyword.DisableCodeGenOptimizer)
            .AddPrefixedKeyword("disabletranslationoptimizer", Keyword.DisableTranslationOptimizer)
            .AddPrefixedKeyword("verbose", Keyword.Verbose);

        // The last keyword recognized. This defines what the following parameter value is for, 
        // or None if not expecting a parameter value.
        var previousKeyword = Keyword.None;

        foreach (var (keyword, arg) in tokenizer.Tokenize(args))
        {
            var prev = previousKeyword;
            previousKeyword = Keyword.None;
            switch (prev)
            {
                case Keyword.None:
                    // Expecting a keyword.
                    switch (keyword)
                    {
                        case Keyword.Ambiguous:
                            ErrorDescription = $"Ambiguous: {arg}";
                            return;
                        case Keyword.None:
                            ErrorDescription = $"Unexpected: {arg}";
                            return;
                        case Keyword.Help:
                            HelpRequested = true;
                            return;
                        case Keyword.Strict:
                            StrictMode = true;
                            break;
                        case Keyword.DisableCodeGenOptimizer:
                            DisableCodeGenOptimizer = true;
                            break;
                        case Keyword.DisableTranslationOptimizer:
                            DisableTranslationOptimizer = true;
                            break;
                        case Keyword.Verbose:
                            Verbose = true;
                            break;

                        // The following keywords require a parameter as the next token.
                        case Keyword.InputFile:
                        case Keyword.Language:
                        case Keyword.ClassName:
                        case Keyword.OutputFolder:
                            previousKeyword = keyword;
                            break;
                        default:
                            // Should never get here.
                            throw new InvalidOperationException();
                    }
                    break;
                case Keyword.InputFile:
                    if (InputFile != null)
                    {
                        ErrorDescription = "input specified more than once";
                        return;
                    }
                    InputFile = arg;
                    previousKeyword = Keyword.None;
                    break;
                case Keyword.Language:
                    _languageStrings.Add(arg);
                    break;
                case Keyword.ClassName:
                    if (ClassName != null)
                    {
                        ErrorDescription = "class name specified more than once";
                        return;
                    }
                    ClassName = arg;
                    break;
                case Keyword.OutputFolder:
                    if (OutputFolder != null)
                    {
                        ErrorDescription = "output folder specified more than once";
                        return;
                    }
                    OutputFolder = arg;
                    break;
                default:
                    // Should never get here.
                    throw new InvalidOperationException();
            }
        }

        // All tokens consumed. Ensure we are not waiting for the final parameter value.
        if (previousKeyword != Keyword.None)
        {
            ErrorDescription = "Missing value";
        }
    }
}

