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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Simplification;

namespace Microsoft.Toolkit.Uwp.PlatformSpecific
{
    [ExportCodeFixProvider(LanguageNames.CSharp)]
    public class CodeFixerCS : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(Analyzer.RulePlatform.Id, Analyzer.RuleVersion.Id);

        public override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            try
            {
                var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
                var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);

                // Which node are we interested in? -- if the squiggle is over A.B().C,
                // then we need the largest IdentifierName/SimpleMemberAccess/QualifiedName
                // that encompasses "C" itself
                var diagnostic = context.Diagnostics.First();
                var span = new CodeAnalysis.Text.TextSpan(diagnostic.Location.SourceSpan.End - 1, 1);
                var node = root.FindToken(span.Start).Parent;

                SyntaxKind nodeKind = node.Kind();

                while (nodeKind != SyntaxKind.IdentifierName && nodeKind != SyntaxKind.SimpleMemberAccessExpression && nodeKind != SyntaxKind.QualifiedName)
                {
                    node = node.Parent;

                    if (node == null)
                    {
                        return;
                    }

                    nodeKind = node.Kind();
                }

                while (true)
                {
                    if (node.Parent?.Kind() == SyntaxKind.SimpleMemberAccessExpression)
                    {
                        node = node.Parent;
                        continue;
                    }

                    if (node.Parent?.Kind() == SyntaxKind.QualifiedName)
                    {
                        node = node.Parent;
                        continue;
                    }

                    break;
                }

                var target = AnalyzerCS.GetTargetOfNode(node, semanticModel);
                var g = HowToGuard.Symbol(target);

                // Introduce a guard? (only if it is a method/accessor/constructor, i.e. somewhere that allows code)
                var containingBlock = node.FirstAncestorOrSelf<BlockSyntax>();
                if (containingBlock != null)
                {
                    var act1 = CodeAction.Create($"Add 'If ApiInformation.{g.KindOfCheck}'", (c) => IntroduceGuardAsync(context.Document, node, g, c), "PlatformSpecificGuard");
                    context.RegisterCodeFix(act1, diagnostic);
                }

                // Mark method/property/constructor as platform-specific?
                if (containingBlock == null || containingBlock.Parent is AccessorDeclarationSyntax)
                {
                    var propDeclaration = node.FirstAncestorOrSelf<PropertyDeclarationSyntax>();
                    if (propDeclaration == null)
                    {
                        return;
                    }

                    var name = $"property '{propDeclaration.Identifier.Text}'";
                    var act2 = CodeAction.Create($"Mark {name} as {g.AttributeFriendlyName}", (c) => AddPlatformSpecificAttributeAsync(g.AttributeToIntroduce, propDeclaration, (n, a) => n.AddAttributeLists(a), context.Document.Project.Solution, c), "PlatformSpecificMethod");
                    context.RegisterCodeFix(act2, diagnostic);
                }
                else if (containingBlock.Parent.Kind() == SyntaxKind.MethodDeclaration)
                {
                    var methodDeclaration = containingBlock.FirstAncestorOrSelf<MethodDeclarationSyntax>();
                    var name = $"method '{methodDeclaration.Identifier.Text}'";
                    var act2 = CodeAction.Create($"Mark {name} as {g.AttributeFriendlyName}", (c) => AddPlatformSpecificAttributeAsync(g.AttributeToIntroduce, methodDeclaration, (n, a) => n.AddAttributeLists(a), context.Document.Project.Solution, c), "PlatformSpecificMethod");
                    context.RegisterCodeFix(act2, diagnostic);
                }
                else if (containingBlock.Parent.Kind() == SyntaxKind.ConstructorDeclaration)
                {
                    var methodDeclaration = containingBlock.FirstAncestorOrSelf<ConstructorDeclarationSyntax>();
                    var name = $"constructor '{methodDeclaration.Identifier.Text}'";
                    var act2 = CodeAction.Create($"Mark {name} as {g.AttributeFriendlyName}", (c) => AddPlatformSpecificAttributeAsync(g.AttributeToIntroduce, methodDeclaration, (n, a) => n.AddAttributeLists(a), context.Document.Project.Solution, c), "PlatformSpecificMethod");
                    context.RegisterCodeFix(act2, diagnostic);
                }

                // Mark some of the conditions as platform-specific?
                foreach (var symbol in AnalyzerCS.GetGuards(node, semanticModel))
                {
                    if (symbol.ContainingType.Name == "ApiInformation")
                    {
                        continue;
                    }

                    if (!symbol.Locations.First().IsInSource)
                    {
                        return;
                    }

                    CodeAction act4 = null;
                    var symbolSyntax = await symbol.DeclaringSyntaxReferences.First().GetSyntaxAsync(context.CancellationToken).ConfigureAwait(false);
                    var fieldSyntax = symbolSyntax.Parent.Parent as FieldDeclarationSyntax;
                    var propSyntax = symbolSyntax as PropertyDeclarationSyntax;

                    if (fieldSyntax != null)
                    {
                        act4 = CodeAction.Create($"Mark field '{symbol.Name}' as {g.AttributeFriendlyName}", (c) => AddPlatformSpecificAttributeAsync(g.AttributeToIntroduce, fieldSyntax, (n, a) => n.AddAttributeLists(a), context.Document.Project.Solution, c), "PlatformSpecificSymbol" + symbol.Name);
                    }
                    else if (propSyntax != null)
                    {
                        act4 = CodeAction.Create($"Mark property '{symbol.Name}' as {g.AttributeFriendlyName}", (c) => AddPlatformSpecificAttributeAsync(g.AttributeToIntroduce, propSyntax, (n, a) => n.AddAttributeLists(a), context.Document.Project.Solution, c), "PlatformSpecificSymbol" + symbol.Name);
                    }

                    if (act4 != null)
                    {
                        context.RegisterCodeFix(act4, diagnostic);
                    }
                }
            }
            catch
            {
            }
        }

        private async Task<Solution> AddPlatformSpecificAttributeAsync<T>(string attrName, T oldSyntax, Func<T, AttributeListSyntax, T> f, Solution solution, CancellationToken cancellationToken)
            where T : SyntaxNode
        {
            // + [System.Runtime.CompilerServices.PlatformSpecific]
            // type p // method/class/field/property
            try
            {
                var oldRoot = await oldSyntax.SyntaxTree.GetRootAsync(cancellationToken).ConfigureAwait(false);
                SyntaxNode temp = null;
                var oldDocument = (from p in solution.Projects from d in p.Documents where d.TryGetSyntaxRoot(out temp) && temp == oldRoot select d).First();

                var id = SyntaxFactory.ParseName(attrName);
                var attr = SyntaxFactory.Attribute(id);
                var attrs = SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(attr)).WithAdditionalAnnotations(Simplifier.Annotation);

                var newSyntax = f(oldSyntax, attrs);
                var newRoot = oldRoot.ReplaceNode(oldSyntax, newSyntax);
                return solution.WithDocumentSyntaxRoot(oldDocument.Id, newRoot);
            }
            catch
            {
            }

            return solution;
        }


        private async Task<Document> IntroduceGuardAsync(Document document, SyntaxNode node, HowToGuard g, CancellationToken cancellationToken)
        {
            // + if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent(targetContainingType))
            // {
            //     old-statement
            // + }
            try
            {
                var oldStatement = node.FirstAncestorOrSelf<StatementSyntax>();
                var oldLeadingTrivia = oldStatement.GetLeadingTrivia();

                var conditionReceiver = SyntaxFactory.ParseName($"Windows.Foundation.Metadata.ApiInformation.{g.KindOfCheck}").WithAdditionalAnnotations(Simplifier.Annotation);
                ArgumentListSyntax conditionArgument = null;

                if (g.MemberToCheck == null)
                {
                    var conditionString1 = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(g.TypeToCheck));
                    conditionArgument = SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(conditionString1)));
                }
                else
                {
                    var conditionString1 = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(g.TypeToCheck));
                    var conditionString2 = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(g.MemberToCheck));
                    var conditionInt3 = SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(g?.ParameterCountToCheck ?? 0));

                    IEnumerable<ArgumentSyntax> conditions = null;

                    if (g.ParameterCountToCheck.HasValue)
                    {
                        conditions = new ArgumentSyntax[] { SyntaxFactory.Argument(conditionString1), SyntaxFactory.Argument(conditionString2), SyntaxFactory.Argument(conditionInt3) };
                    }

                    if (!g.ParameterCountToCheck.HasValue)
                    {
                        conditions = new ArgumentSyntax[] { SyntaxFactory.Argument(conditionString1), SyntaxFactory.Argument(conditionString2) };
                    }

                    conditionArgument = SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(conditions));
                }

                var condition = SyntaxFactory.InvocationExpression(conditionReceiver, conditionArgument);

                var thenStatements = SyntaxFactory.Block(oldStatement.WithoutLeadingTrivia());
                var ifStatement = SyntaxFactory.IfStatement(condition, thenStatements).WithLeadingTrivia(oldLeadingTrivia).WithAdditionalAnnotations(Formatter.Annotation);

                var oldRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
                var newRoot = oldRoot.ReplaceNode(oldStatement, ifStatement);

                return document.WithSyntaxRoot(newRoot);
            }
            catch
            {
            }

            return document;
        }
    }
}
