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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Microsoft.Toolkit.Uwp.PlatformSpecificAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PlatformSpecificAnalyzerCS : DiagnosticAnalyzer
    {
        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private const string Category = "Safety";

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Analyzer.PlatformRule, Analyzer.VersionRule); } }

        public override void Initialize(AnalysisContext context)
        {
            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information

            ConcurrentDictionary<int, Diagnostic> reportsDictionary = new ConcurrentDictionary<int, Diagnostic>();

            context.RegisterSyntaxNodeAction((c) => AnalyzeExpression(c, reportsDictionary), SyntaxKind.IdentifierName, SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.QualifiedName);
        }

        private void AnalyzeExpression(SyntaxNodeAnalysisContext context, ConcurrentDictionary<int, Diagnostic> reports)
        {
            var parentKind = context.Node.Parent.Kind();

            // will be handled at higher level
            if (parentKind == SyntaxKind.SimpleMemberAccessExpression || parentKind == SyntaxKind.QualifiedName)
            {
                return;
            }

            var target = GetTargetOfNode(context.Node, context.SemanticModel);

            if (target == null)
            {
                return;
            }

            var platform = Platform.OfSymbol(target);

            // Some quick escapes
            if (platform.Kind == PlatformKind.Unchecked)
            {
                return;
            }

            if (platform.Kind == PlatformKind.Uwp && platform.Version == Analyzer.N2SDKVersion)
            {
                return;
            }

            // Is this expression inside a method/constructor/property that claims to be specific?
            var containingBlock = context.Node.FirstAncestorOrSelf<BlockSyntax>();

            // for constructors and methods
            MemberDeclarationSyntax containingMember = containingBlock?.FirstAncestorOrSelf<BaseMethodDeclarationSyntax>();

            if (containingBlock == null || containingBlock?.Parent is AccessorDeclarationSyntax)
            {
                containingMember = context.Node.FirstAncestorOrSelf<PropertyDeclarationSyntax>();
            }

            if (containingMember != null)
            {
                var containingMemberSymbol = context.SemanticModel.GetDeclaredSymbol(containingMember);
                if (Analyzer.HasPlatformSpecificAttribute(containingMemberSymbol))
                {
                    return;
                }
            }

            // Is this invocation properly guarded? See readme.md for explanations.
            if (IsProperlyGuarded(context.Node, context.SemanticModel))
            {
                return;
            }

            if (containingBlock != null)
            {
                foreach (var ret in containingBlock.DescendantNodes().OfType<ReturnStatementSyntax>())
                {
                    if (IsProperlyGuarded(ret, context.SemanticModel))
                    {
                        return;
                    }
                }
            }

            // Some things we can't judge whether to report until after we've looked up the project version...
            //if (platform.Kind == PlatformKind.Uwp && platform.Version != Analyzer.N2SDKVersion)
            //{
            //    var projMinVersion = Analyzer.GetTargetPlatformMinVersion(context.Options.AdditionalFiles);

            //    if (projMinVersion >= Convert.ToInt32(platform.Version))
            //    {
            //        return;
            //    }
            //}

            // We'll report only a single diagnostic per line, the first.
            var loc = context.Node.GetLocation();
            if (!loc.IsInSource)
            {
                return;
            }

            var line = loc.GetLineSpan().StartLinePosition.Line;

            Diagnostic diagnostic = null;

            if (reports.TryGetValue(line, out diagnostic) && diagnostic.Location.SourceSpan.Start <= loc.SourceSpan.Start)
            {
                return;
            }

            diagnostic = Diagnostic.Create(platform.Kind == PlatformKind.Uwp ? Analyzer.VersionRule : Analyzer.PlatformRule, loc);
            
            reports[line] = diagnostic;

            context.ReportDiagnostic(diagnostic);
        }

        public static ISymbol GetTargetOfNode(SyntaxNode node, SemanticModel semanticModel)
        {
            var parentKind = node.Parent.Kind();

            if (parentKind == SyntaxKind.InvocationExpression && node == ((InvocationExpressionSyntax)node.Parent).Expression)
            {
                // <target>(...)
                // points to the method after overload resolution
                return semanticModel.GetSymbolInfo((InvocationExpressionSyntax)node.Parent).Symbol;
            }
            else if (parentKind == SyntaxKind.ObjectCreationExpression && node == ((ObjectCreationExpressionSyntax)node.Parent).Type)
            {
                // New <target>
                var objectCreationExpression = (ObjectCreationExpressionSyntax)node.Parent;
                var target = semanticModel.GetSymbolInfo(objectCreationExpression).Symbol;

                // points to the constructor after overload resolution
                return target;
            }
            else
            {
                // f<target>(...)
                // <target> x = ...
                // Action x = <target>  -- note that following code does pick the right overload
                // <target> += delegate -- the following code does recognize events
                // nameof(<target>) -- I think it's nicer to report on this, even if not technically needed
                // Field access? I'll disallow it for enum values, and allow it for everything else
                var target = semanticModel.GetSymbolInfo(node).Symbol;

                if (target == null)
                {
                    return null;
                }

                if (target.Kind == SymbolKind.Method || target.Kind == SymbolKind.Event || target.Kind == SymbolKind.Property)
                {
                    return target;
                }

                if (target.Kind == SymbolKind.Field && target.ContainingType.TypeKind == TypeKind.Enum)
                {
                    return target;
                }

                return null;
            }
        }

        private bool IsProperlyGuarded(SyntaxNode node, SemanticModel semanticModel)
        {
            foreach (var symbol in GetGuards(node, semanticModel))
            {
                if (symbol.ContainingType?.Name == "ApiInformation")
                {
                    return true;
                }

                if (Analyzer.HasPlatformSpecificAttribute(symbol))
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<ISymbol> GetGuards(SyntaxNode node, SemanticModel semanticModel)
        {
            foreach (var condition in GetConditions(node))
            {
                // First check for invocations of ApiInformation.IsTypePresent
                foreach (var invocation in condition.DescendantNodesAndSelf(i => i is InvocationExpressionSyntax))
                {
                    var targetMethod = semanticModel.GetSymbolInfo(invocation).Symbol;

                    if (targetMethod?.ContainingType?.Name == "ApiInformation")
                    {
                        yield return targetMethod;
                    }
                }

                // Next check for any property/field access
                var accesses1 = condition.DescendantNodesAndSelf(d => d is MemberAccessExpressionSyntax).Select(n => semanticModel.GetSymbolInfo(n).Symbol);
                var accesses2 = condition.DescendantNodesAndSelf(d => d is IdentifierNameSyntax).Select(n => semanticModel.GetSymbolInfo(n).Symbol);

                foreach (var symbol in accesses1.Concat(accesses2))
                {
                    if (symbol?.Kind == SymbolKind.Field || symbol?.Kind == SymbolKind.Property)
                    {
                        yield return symbol;
                    }
                }
            }
        }

        private static IEnumerable<ExpressionSyntax> GetConditions(SyntaxNode node)
        {
            var check = node.FirstAncestorOrSelf<IfStatementSyntax>();

            while (check != null)
            {
                yield return check.Condition;
                check = check.Parent.FirstAncestorOrSelf<IfStatementSyntax>();
            }
        }
    }
}
