// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace Microsoft.Toolkit.Uwp.PlatformSpecificAnalyzer
{
    /// <summary>
    /// This class is a Roslyn code analyzer that checks for types / members that should be guarded against.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.VisualBasic)]
    public class PlatformSpecificAnalyzerVB : DiagnosticAnalyzer
    {
        /// <summary>
        /// Gets supported diagnostics
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(Analyzer.PlatformRule, Analyzer.VersionRule); }
        }

        /// <summary>
        /// Gets instance of symbol from sytax node
        /// </summary>
        /// <param name="node">instance of <see cref="SyntaxNode"/></param>
        /// <param name="semanticModel"><see cref="SemanticModel"/></param>
        /// <returns><see cref="ISymbol"/></returns>
        public static ISymbol GetTargetOfNode(SyntaxNode node, SemanticModel semanticModel)
        {
            var parentKind = node.Parent.Kind();

            if (parentKind == SyntaxKind.InvocationExpression && node == ((InvocationExpressionSyntax)node.Parent).Expression)
            {
                // <target>(...)
                // points to the method after overload resolution
                return semanticModel.GetSymbolInfo((InvocationExpressionSyntax)node.Parent).Symbol;
            }
            else if (parentKind == SyntaxKind.AddressOfExpression)
            {
                // AddressOf <target>
                return semanticModel.GetSymbolInfo(node).Symbol; // points to the method after overload resolution
            }
            else if (parentKind == SyntaxKind.ObjectCreationExpression && node == ((ObjectCreationExpressionSyntax)node.Parent).Type)
            {
                // New <target>
                var objectCreationExpression = (ObjectCreationExpressionSyntax)node.Parent;
                var target = semanticModel.GetSymbolInfo(objectCreationExpression).Symbol;

                // points to the constructor after overload resolution
                return target;
            }
            else if (parentKind == SyntaxKind.AddHandlerStatement && node == ((AddRemoveHandlerStatementSyntax)node.Parent).EventExpression)
            {
                // AddHandler <target>, delegate
                return semanticModel.GetSymbolInfo(node).Symbol; // points to the event
            }
            else if (parentKind == SyntaxKind.NameOfExpression)
            {
                // NameOf(<target>)
                return null;
            }
            else
            {
                // f(Of <target>)(...)  -- no warning
                // Dim x As <target> = ...  -- no warning
                // property access -- warning
                // field access -- only warning on enum fields
                // method access without arguments -- warning
                var target = semanticModel.GetSymbolInfo(node).Symbol;

                if (target == null)
                {
                    return null;
                }

                var targetKind = target.Kind;

                if (targetKind == SymbolKind.Method || targetKind == SymbolKind.Property || targetKind == SymbolKind.NamedType)
                {
                    return target;
                }

                if (targetKind == SymbolKind.Field && target.ContainingType.TypeKind == TypeKind.Enum)
                {
                    return target;
                }

                return null;
            }
        }

        /// <summary>
        /// Initialises the analyzer, registering for code analysis.
        /// </summary>
        /// <param name="context"><see cref="AnalysisContext"/></param>
        public override void Initialize(AnalysisContext context)
        {
            ConcurrentDictionary<int, Diagnostic> reportsDictionary = new ConcurrentDictionary<int, Diagnostic>();

            context.RegisterSyntaxNodeAction((c) => AnalyzeExpression(c, reportsDictionary), SyntaxKind.LocalDeclarationStatement, SyntaxKind.IdentifierName, SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.QualifiedName);
        }

        private static IEnumerable<ISymbol> GetGuards(SyntaxNode node, SemanticModel semanticModel)
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
                    if (symbol == null)
                    {
                        continue;
                    }

                    var symbolKind = symbol.Kind;

                    if (symbolKind == SymbolKind.Field || symbolKind == SymbolKind.Property)
                    {
                        yield return symbol;
                    }
                }
            }
        }

        private static IEnumerable<ExpressionSyntax> GetConditions(SyntaxNode node)
        {
            var check1 = node.FirstAncestorOrSelf<MultiLineIfBlockSyntax>();

            while (check1 != null)
            {
                yield return check1.IfStatement.Condition;
                check1 = check1.Parent.FirstAncestorOrSelf<MultiLineIfBlockSyntax>();
            }

            var check2 = node.FirstAncestorOrSelf<SingleLineIfStatementSyntax>();

            while (check2 != null)
            {
                yield return check2.Condition;
                check2 = check2.Parent.FirstAncestorOrSelf<SingleLineIfStatementSyntax>();
            }
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

            var platform = Analyzer.GetPlatformForSymbol(target);

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
            DeclarationStatementSyntax containingMember = context.Node.FirstAncestorOrSelf<MethodBlockBaseSyntax>();

            if (containingMember is AccessorBlockSyntax)
            {
                containingMember = containingMember.FirstAncestorOrSelf<PropertyBlockSyntax>();
            }

            // Is this invocation properly guarded? See readme.md for explanations.
            if (IsProperlyGuarded(context.Node, context.SemanticModel))
            {
                return;
            }

            if (containingMember != null)
            {
                foreach (var ret in containingMember.DescendantNodes().OfType<ReturnStatementSyntax>())
                {
                    if (IsProperlyGuarded(ret, context.SemanticModel))
                    {
                        return;
                    }
                }
            }

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

        private bool IsProperlyGuarded(SyntaxNode node, SemanticModel semanticModel)
        {
            foreach (var symbol in GetGuards(node, semanticModel))
            {
                if (symbol.ContainingType?.Name == "ApiInformation")
                {
                    return true;
                }
            }

            return false;
        }
    }
}
