// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace Microsoft.Toolkit.Uwp.PlatformSpecificAnalyzer
{
    public static class Analyzer
    {
        public static readonly DiagnosticDescriptor PlatformRule = new DiagnosticDescriptor("UWP001", "Platform-specific", "Platform-specific code detected. Consider using ApiInformation.IsTypePresent to guard against failure", "Safety", DiagnosticSeverity.Warning, true);
        public static readonly DiagnosticDescriptor VersionRule = new DiagnosticDescriptor("UWP002", "Version-specific", "Version-specific code detected. Consider using ApiInformation.IsTypePresent / ApiInformation.IsMethodPresent / ApiInformation.IsPropertyPresent to guard against failure", "Safety", DiagnosticSeverity.Warning, true);

        public const string N1DifferencesRes = "Differences-5.0.0.0.gz";
        public const string N0DifferencesRes = "Differences-6.0.0.0.gz";

        public const string N2SDKVersion = "15063";
        public const string N1SDKVersion = "16299";
        public const string N0SDKVersion = "17134";

        private static char[] typeMemberSeparator = { ':' };
        private static char[] memberSeparator = { ',' };

        public static Dictionary<string, List<NewMember>> GetUniversalApiAdditions(string resourceName)
        {
            Dictionary<string, List<NewMember>> apiAdditionsDictionary = new Dictionary<string, List<NewMember>>();

            Assembly assembly = typeof(Analyzer).GetTypeInfo().Assembly;

            var resource = assembly.GetManifestResourceStream("PlatformSpecific." + resourceName);

            if (resource == null)
            {
                System.Diagnostics.Debug.WriteLine($"Resource {resourceName} not found.");
                return apiAdditionsDictionary;
            }

            System.Diagnostics.Debug.WriteLine($"Resource {resourceName} found.");
            Dictionary<string, List<string>> differencesDictionary = new Dictionary<string, List<string>>();

            using (GZipStream decompressionStream = new GZipStream(resource, CompressionMode.Decompress))
            {
                using (StreamReader reader = new StreamReader(decompressionStream))
                {
                    while (!reader.EndOfStream)
                    {
                        var typeDetails = reader.ReadLine();

                        var typeMemberParts = typeDetails.Split(typeMemberSeparator, StringSplitOptions.RemoveEmptyEntries);

                        if (typeMemberParts.Length == 1)
                        {
                            differencesDictionary.Add(typeMemberParts[0], null);

                            continue;
                        }

                        var membersAddedToType = typeMemberParts[1].Split(memberSeparator, StringSplitOptions.RemoveEmptyEntries);

                        differencesDictionary.Add(typeMemberParts[0], new List<string>(membersAddedToType));
                    }
                }
            }

            if (differencesDictionary == null)
            {
                return apiAdditionsDictionary;
            }

            foreach (var kvp in differencesDictionary)
            {
                var list = new List<NewMember>();
                if (kvp.Value != null)
                {
                    list.AddRange(kvp.Value.Select(v => new NewMember(v)));
                }

                apiAdditionsDictionary.Add(kvp.Key, list);
            }

            return apiAdditionsDictionary;
        }

        public static int GetTargetPlatformMinVersion(ImmutableArray<AdditionalText> additionalFiles)
        {
            // When PlatformSpecificAnalyzer is build as a NuGet package, the package includes
            // a.targets File with the following lines. The effect is to add a fake file,
            // which doesn't show up in SolnExplorer and which doesn't even exist, but whose
            // FILENAME encodes the TargetPlatformMinVersion. That way, when the user modifies
            // TargetPlatformMinVersion from within the ProjectProperties, msbuild re-evaluates
            // the AdditionalFiles, and Roslyn re-runs its analyzers and can pick it up.
            // Thanks Jason Malinowski for the hint on how to do this. He instructed me to
            // write in the comments "this is a terrible hack and no one should ever copy it".
            //      <AdditionalFileItemNames>PlatformSpecificAnalyzerInfo</AdditionalFileItemNames>
            //      <ItemGroup>
            //        <PlatformSpecificAnalyzerInfo Include = "tpmv_$(TargetPlatformMinVersion).tpmv"><Visible>False</Visible></PlatformSpecificAnalyzerInfo>
            //      </ItemGroup>
            //  I'm caching the value because, heck, it seems weird to recompute it every time.
            ImmutableArray<AdditionalText> cacheKey = default(ImmutableArray<AdditionalText>);
            int minSDK = int.Parse(N2SDKVersion);

            int cacheValue = minSDK;

            // if we don't find that terrible hack, assume min version of sdk
            if (additionalFiles == cacheKey)
            {
                return cacheValue;
            }
            else
            {
                cacheKey = additionalFiles;
            }

            var tpmv = additionalFiles.FirstOrDefault(af => af.Path.EndsWith(".tpmv"))?.Path;
            if (tpmv == null)
            {
                cacheValue = minSDK;
            }
            else
            {
                tpmv = Path.GetFileNameWithoutExtension(tpmv).Replace("tpmv_10.0.", string.Empty).Replace(".0", string.Empty);
                cacheValue = int.TryParse(tpmv, out int i) ? i : cacheValue;
            }

            return cacheValue;
        }

        public static string GetPlatformSpecificAttribute(ISymbol symbol)
        {
            if (symbol == null)
            {
                return null;
            }

            foreach (var attr in symbol.GetAttributes())
            {
                if (attr.AttributeClass.Name.EndsWith("SpecificAttribute"))
                {
                    return attr.AttributeClass.ToDisplayString().Replace("Attribute", string.Empty);
                }
            }

            return null;
        }

        public static bool HasPlatformSpecificAttribute(ISymbol symbol)
        {
            return GetPlatformSpecificAttribute(symbol) != null;
        }
    }
}
