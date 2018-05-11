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

namespace Microsoft.Toolkit.Uwp.PlatformSpecificAnalyzer
{
    public class HowToGuard
    {
        public string TypeToCheck { get; set; }

        public string MemberToCheck { get; set; }

        public int? ParameterCountToCheck { get; set; }

        public string KindOfCheck { get; set; }

        public string AttributeToIntroduce { get; set; }

        public string AttributeFriendlyName { get; set; }

        public HowToGuard()
        {
            KindOfCheck = "IsTypePresent";

            AttributeToIntroduce = "System.Runtime.CompilerServices.PlatformSpecific";

            AttributeFriendlyName = "PlatformSpecific";
        }

        /// <summary>
        /// returns instance of <see cref="HowToGuard"/> for <see cref="ISymbol"/>
        /// </summary>
        /// <param name="target">instance of <see cref="ISymbol"/></param>
        /// <returns>instance of <see cref="HowToGuard"/></returns>
        public static HowToGuard Symbol(ISymbol target)
        {
            var plat = Platform.OfSymbol(target);

            if (plat.Kind == PlatformKind.User)
            {
                var lastDot = plat.Version.LastIndexOf('.');
                var attrName = lastDot == -1 ? plat.Version : plat.Version.Substring(lastDot + 1);

                return new HowToGuard()
                {
                    AttributeToIntroduce = plat.Version,
                    AttributeFriendlyName = attrName,
                    TypeToCheck = "??"
                };
            }
            else if (plat.Kind == PlatformKind.ExtensionSDK)
            {
                return new HowToGuard()
                {
                    TypeToCheck = target.Kind == SymbolKind.NamedType ? target.ToDisplayString() : target.ContainingType.ToDisplayString()
                };
            }
            else if (plat.Kind == PlatformKind.Uwp && target.Kind == SymbolKind.NamedType)
            {
                return new HowToGuard()
                {
                    TypeToCheck = target.ToDisplayString()
                };
            }
            else if (plat.Kind == PlatformKind.Uwp && target.Kind != SymbolKind.NamedType)
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

            throw new InvalidOperationException("oops! don't know why I was asked to check something that's fine");
        }
    }
}
