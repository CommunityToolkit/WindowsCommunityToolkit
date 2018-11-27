// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace Microsoft.Toolkit.Uwp.PlatformSpecificAnalyzer
{
    /// <summary>
    /// This class offers loads platform differences for use by Code Analyzer and Code Fixer.
    /// </summary>
    public static class Analyzer
    {
        internal enum TypePresenceIndicator
        {
            New,
            Changes,
            NotFound,
        }

        private static Dictionary<string, Dictionary<string, List<NewMember>>> _differencesDictionary = null;

        /// <summary>
        /// Embedded differences between API contract version 4 and 5.
        /// </summary>
        public const string N1DifferencesRes = "Differences-5.0.0.0.gz";

        /// <summary>
        /// Embedded differences between API contract version 5 and 6.
        /// </summary>
        public const string N0DifferencesRes = "Differences-6.0.0.0.gz";

        /// <summary>
        /// Earliest supported SDK version.
        /// </summary>
        public const string N2SDKVersion = "15063";

        /// <summary>
        /// Intermediate SDK version.
        /// </summary>
        public const string N1SDKVersion = "16299";

        /// <summary>
        /// Latest SDK version.
        /// </summary>
        public const string N0SDKVersion = "17134";

        /// <summary>
        /// Platform related diagnostic descriptor
        /// </summary>
        public static readonly DiagnosticDescriptor PlatformRule = new DiagnosticDescriptor("UWP001", "Platform-specific", "Platform-specific code detected. Consider using ApiInformation.IsTypePresent to guard against failure", "Safety", DiagnosticSeverity.Warning, true);

        /// <summary>
        /// Version related diagnostic descriptor
        /// </summary>
        public static readonly DiagnosticDescriptor VersionRule = new DiagnosticDescriptor("UWP002", "Version-specific", "Version-specific code detected. Consider using ApiInformation.IsTypePresent / ApiInformation.IsMethodPresent / ApiInformation.IsPropertyPresent to guard against failure", "Safety", DiagnosticSeverity.Warning, true);

        private static char[] typeMemberSeparator = { ':' };
        private static char[] memberSeparator = { ',' };

        static Analyzer()
        {
            _differencesDictionary = new Dictionary<string, Dictionary<string, List<NewMember>>>();
            _differencesDictionary.Add(N0DifferencesRes, GetApiAdditions(N0DifferencesRes));
            _differencesDictionary.Add(N1DifferencesRes, GetApiAdditions(N1DifferencesRes));
        }

        /// <summary>
        /// Gets the API differences from specified resource.
        /// </summary>
        /// <param name="resourceName">name of embedded resource</param>
        /// <returns>Dictionary with Fully qualified name of type as key and list of new members as value</returns>
        public static Dictionary<string, List<NewMember>> GetUniversalApiAdditions(string resourceName)
        {
            return _differencesDictionary[resourceName];
        }

        private static Dictionary<string, List<NewMember>> GetApiAdditions(string resourceName)
        {
            Dictionary<string, List<NewMember>> apiAdditionsDictionary = new Dictionary<string, List<NewMember>>();

            Assembly assembly = typeof(Analyzer).GetTypeInfo().Assembly;

            var resource = assembly.GetManifestResourceStream("Microsoft.Toolkit.Uwp.PlatformSpecificAnalyzer." + resourceName);

            if (resource == null)
            {
                System.Diagnostics.Debug.WriteLine($"Resource {resourceName} not found.");
                return new Dictionary<string, List<NewMember>>();
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

        /// <summary>
        /// This function tells which version/platform the symbol is from.
        /// </summary>
        /// <param name="symbol">represents a compiler <see cref="ISymbol"/></param>
        /// <returns>instance of <see cref="Platform"/></returns>
        public static Platform GetPlatformForSymbol(ISymbol symbol)
        {
            if (symbol == null)
            {
                return new Platform(PlatformKind.Unchecked);
            }

            if (symbol.ContainingNamespace != null && symbol.ContainingNamespace.ToDisplayString().StartsWith("Windows."))
            {
                var assembly = symbol.ContainingAssembly.Name;
                var version = symbol.ContainingAssembly.Identity.Version.Major;

                // Any call to ApiInformation.* is allowed without warning
                if (symbol.ContainingType?.Name == "ApiInformation")
                {
                    return new Platform(PlatformKind.Uwp, Analyzer.N2SDKVersion);
                }

                // Don't want to give warning when analyzing code in an PCL project.
                // In those two targets, every Windows type is found in Windows.winmd, so that's how we'll suppress it:
                if (assembly == "Windows")
                {
                    return new Platform(PlatformKind.Unchecked);
                }

                // Some WinRT types like Windows.UI.Color get projected to come from .NET assemblies, always present:
                if (assembly.StartsWith("System.Runtime."))
                {
                    return new Platform(PlatformKind.Uwp, Analyzer.N2SDKVersion);
                }

                // Some things are emphatically part of UWP.10240
                if (assembly == "Windows.Foundation.FoundationContract" || (assembly == "Windows.Foundation.UniversalApiContract" && version == 1))
                {
                    return new Platform(PlatformKind.Uwp, Analyzer.N2SDKVersion);
                }

                if (assembly == "Windows.Foundation.UniversalApiContract")
                {
                    var isType = symbol.Kind == SymbolKind.NamedType;

                    var typeName = isType ? symbol.ToDisplayString() : symbol.ContainingType.ToDisplayString();

                    TypePresenceIndicator presentInN0ApiDiff = CheckCollectionForType(Analyzer.GetUniversalApiAdditions(Analyzer.N0DifferencesRes), typeName, symbol);

                    if (presentInN0ApiDiff == TypePresenceIndicator.New)
                    {
                        // the entire type was found in Target Version
                        return new Platform(PlatformKind.Uwp, Analyzer.N0SDKVersion);
                    }
                    else if (presentInN0ApiDiff == TypePresenceIndicator.Changes)
                    {
                        // the entire type was found in Target Version with matching parameter lengths
                        return new Platform(PlatformKind.Uwp, Analyzer.N0SDKVersion, true);
                    }
                    else
                    {
                        TypePresenceIndicator presentInN1ApiDiff = CheckCollectionForType(Analyzer.GetUniversalApiAdditions(Analyzer.N1DifferencesRes), typeName, symbol);

                        if (presentInN1ApiDiff == TypePresenceIndicator.New)
                        {
                            // the entire type was found in Target Version
                            return new Platform(PlatformKind.Uwp, Analyzer.N1SDKVersion);
                        }
                        else if (presentInN1ApiDiff == TypePresenceIndicator.Changes)
                        {
                            // the entire type was found in Target Version with matching parameter lengths
                            return new Platform(PlatformKind.Uwp, Analyzer.N1SDKVersion, true);
                        }
                        else
                        {
                            // the type was in Min version
                            return new Platform(PlatformKind.Uwp, Analyzer.N2SDKVersion);
                        }
                    }
                }

                // All other Windows.* types come from platform-specific extensions
                return new Platform(PlatformKind.ExtensionSDK);
            }
            else
            {
                return new Platform(PlatformKind.Unchecked);
            }
        }

        /// <summary>
        /// returns instance of <see cref="HowToGuard"/> for <see cref="ISymbol"/>
        /// </summary>
        /// <param name="target">instance of <see cref="ISymbol"/></param>
        /// <returns>instance of <see cref="HowToGuard"/></returns>
        public static HowToGuard GetGuardForSymbol(ISymbol target)
        {
            var plat = Analyzer.GetPlatformForSymbol(target);

            switch (plat.Kind)
            {
                case PlatformKind.ExtensionSDK:
                    return new HowToGuard()
                    {
                        TypeToCheck = target.Kind == SymbolKind.NamedType ? target.ToDisplayString() : target.ContainingType.ToDisplayString(),
                        KindOfCheck = "IsTypePresent"
                    };
                case PlatformKind.Uwp:
                    if (target.Kind == SymbolKind.NamedType)
                    {
                        return new HowToGuard()
                        {
                            TypeToCheck = target.ToDisplayString(),
                            KindOfCheck = "IsTypePresent"
                        };
                    }
                    else
                    {
                        var g = new HowToGuard
                        {
                            TypeToCheck = target.ContainingType.ToDisplayString()
                        };

                        var d0 = Analyzer.GetUniversalApiAdditions(Analyzer.N0DifferencesRes);
                        var d1 = Analyzer.GetUniversalApiAdditions(Analyzer.N1DifferencesRes);

                        if (!d0.TryGetValue(g.TypeToCheck, out List<NewMember> newMembers))
                        {
                            d1.TryGetValue(g.TypeToCheck, out newMembers);
                        }

                        if (newMembers == null)
                        {
                            throw new InvalidOperationException("oops! expected this UWP version API to be in the dictionary of new things");
                        }

                        g.MemberToCheck = target.Name;

                        if (target.Kind == SymbolKind.Field)
                        {
                            // the only fields in WinRT are enum fields
                            g.KindOfCheck = "IsEnumNamedValuePresent";
                        }
                        else if (target.Kind == SymbolKind.Event)
                        {
                            g.KindOfCheck = "IsEventPresent";
                        }
                        else if (target.Kind == SymbolKind.Property)
                        {
                            // TODO: if SDK starts introducing additional accessors on properties, we'll have to change this
                            g.KindOfCheck = "IsPropertyPresent";
                        }
                        else if (target.Kind == SymbolKind.Method)
                        {
                            g.KindOfCheck = "IsMethodPresent";

                            if (target.Kind == SymbolKind.Method && plat.ByParameterCount)
                            {
                                g.ParameterCountToCheck = (target as IMethodSymbol).Parameters.Length;
                            }
                        }

                        return g;
                    }

                default:
                    throw new InvalidOperationException("oops! don't know why I was asked to check something that's fine");
            }
        }

        private static TypePresenceIndicator CheckCollectionForType(Dictionary<string, List<NewMember>> collection, string typeName, ISymbol symbol)
        {
            List<NewMember> newMembers = null;

            if (!collection.TryGetValue(typeName, out newMembers))
            {
                return TypePresenceIndicator.NotFound;
            }

            if (newMembers == null || newMembers.Count == 0)
            {
                return TypePresenceIndicator.New;
            }

            if (symbol.Kind == SymbolKind.NamedType)
            {
                return TypePresenceIndicator.NotFound;
            }

            var memberName = symbol.Name;

            foreach (var newMember in newMembers)
            {
                if (memberName == newMember.Name && !newMember.ParameterCount.HasValue)
                {
                    return TypePresenceIndicator.New;
                }

                // this member was new in collection
                if (symbol.Kind != SymbolKind.Method)
                {
                    // TODO: Continue For... Warning!!! not translated
                }

                if (memberName == newMember.Name && ((IMethodSymbol)symbol).Parameters.Length == newMember.ParameterCount)
                {
                    return TypePresenceIndicator.Changes;
                }
            }

            // this member existed in a different collection
            return TypePresenceIndicator.NotFound;
        }
    }
}
