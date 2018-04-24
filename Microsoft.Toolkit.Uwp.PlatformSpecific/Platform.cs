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
using Microsoft.CodeAnalysis;

namespace Microsoft.Toolkit.Uwp.PlatformSpecific
{
    /// <summary>
    /// 
    /// </summary>
    public struct Platform
    {
        /// <summary>
        /// Platform Kind
        /// </summary>
        public PlatformKind Kind;

        /// <summary>
        /// For UWP, this is version 10240 or 10586 etc. For User, the fully qualified name of the attribute in use
        /// </summary>
        public string Version;

        /// <summary>
        /// For UWP only
        /// </summary>
        public bool ByParameterCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="Platform"/> struct.
        /// </summary>
        /// <param name="kind"><see cref="PlatformKind"/></param>
        /// <param name="version">version</param>
        /// <param name="byParameterCount">boolean</param>
        public Platform(PlatformKind kind, string version = null, bool byParameterCount = false)
        {
            Kind = kind;
            Version = version;
            ByParameterCount = byParameterCount;

            switch (kind)
            {
                case PlatformKind.Unchecked:
                    if (version != null)
                    {
                        throw new ArgumentException("No version expected");
                    }

                    break;

                case PlatformKind.Uwp:
                    break;

                case PlatformKind.ExtensionSDK:
                    if (version != null)
                    {
                        throw new ArgumentException("Don't specify versions for extension SDKs");
                    }

                    break;

                case PlatformKind.User:
                    if (version != null && !version.EndsWith("Specific"))
                    {
                        throw new ArgumentException("User specific should end in Specific");
                    }

                    break;
            }

            if (byParameterCount && kind != PlatformKind.Uwp)
            {
                throw new ArgumentException("Only UWP can be distinguished by parameter count");
            }
        }

        /// <summary>
        /// This function tells which version/platform the symbol is from.
        /// </summary>
        /// <param name="symbol">represents a compiler <see cref="ISymbol"/></param>
        /// <returns>instance of <see cref="Platform"/></returns>
        public static Platform OfSymbol(ISymbol symbol)
        {
            // This function is hard-coded with knowledge up to SDK 10586.
            // I could have made it a general-purpose function which looks up the SDK
            // files on disk. But I think it's more elegant to hard-code it into the analyzer,
            // so as to reduce disk-access while the analyzer runs.
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

                    bool? presentInN1ApiDiff = CheckCollectionForType(Analyzer.GetUniversalApiAdditions(Analyzer.N1DifferencesRes), typeName, symbol);

                    if (presentInN1ApiDiff == null)
                    {
                        // the entire type was found in Target Version
                        return new Platform(PlatformKind.Uwp, Analyzer.N1SDKVersion);
                    }
                    else if (presentInN1ApiDiff.Value)
                    {
                        // the entire type was found in Target Version with matching parameter lengths
                        return new Platform(PlatformKind.Uwp, Analyzer.N1SDKVersion, true);
                    }
                    else
                    {
                        bool? presentInN0ApiDiff = CheckCollectionForType(Analyzer.GetUniversalApiAdditions(Analyzer.N0DifferencesRes), typeName, symbol);

                        if (presentInN0ApiDiff == null)
                        {
                            // the entire type was found in Target Version
                            return new Platform(PlatformKind.Uwp, Analyzer.N0SDKVersion);
                        }
                        else if (presentInN0ApiDiff.Value)
                        {
                            // the entire type was found in Target Version with matching parameter lengths
                            return new Platform(PlatformKind.Uwp, Analyzer.N0SDKVersion, true);
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
                var attr = GetPlatformSpecificAttribute(symbol);

                if (attr != null)
                {
                    return new Platform(PlatformKind.User, attr);
                }

                return new Platform(PlatformKind.Unchecked);
            }
        }

        private static bool? CheckCollectionForType(Dictionary<string, List<NewMember>> collection, string typeName, ISymbol symbol)
        {
            List<NewMember> newMembers = null;
            object in10586 = collection.TryGetValue(typeName, out newMembers);

            // the entire type was new in this collection
            if (newMembers == null)
            {
                return null;
            }

            if (symbol.Kind == SymbolKind.NamedType)
            {
                return false;
            }

            var memberName = symbol.Name;

            foreach (var newMember in newMembers)
            {
                if (memberName == newMember.Name && !newMember.ParameterCount.HasValue)
                {
                    return null;
                }

                // this member was new in collection
                if (symbol.Kind != SymbolKind.Method)
                {
                    // TODO: Continue For... Warning!!! not translated
                }

                if (memberName == newMember.Name && ((IMethodSymbol)symbol).Parameters.Length == newMember.ParameterCount)
                {
                    return true;
                }
            }

            // this member existed in a different collection
            return false;
        }

        private static string GetPlatformSpecificAttribute(ISymbol symbol)
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
    }
}
