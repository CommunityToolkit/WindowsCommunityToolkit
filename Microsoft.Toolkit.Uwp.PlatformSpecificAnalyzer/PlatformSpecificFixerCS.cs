// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
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
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.Toolkit.Uwp.PlatformSpecificAnalyzer
{
    /// <summary>
    /// This class provides guard suggestion and can make the suggested changes.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PlatformSpecificFixerCS))]
    [Shared]
    public class PlatformSpecificFixerCS : CodeFixProvider
    {
        /// <summary>
        /// Gets the list of Diagnotics that can be fixed.
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(Analyzer.PlatformRule.Id, Analyzer.VersionRule.Id); }
        }

        /// <summary>
        /// Gets the Fix All provider
        /// </summary>
        /// <returns><see cref="WellKnownFixAllProviders"/></returns>
        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        /// <summary>
        /// Registers for code fix.
        /// </summary>
        /// <param name="context"><see cref="CodeFixContext"/></param>
        /// <returns>awaitable <see cref="Task"/></returns>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            try
            {
                var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
                var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);

                // Which node are we interested in? -- if the squiggle is over A.B().C,
                // then we need the largest IdentifierName/SimpleMemberAccess/QualifiedName
                // that encompasses "C" itself
                var diagnostic = context.Diagnostics.First();
                var span = new TextSpan(diagnostic.Location.SourceSpan.End - 1, 1);
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

                var target = PlatformSpecificAnalyzerCS.GetTargetOfNode(node, semanticModel);
                var g = Analyzer.GetGuardForSymbol(target);

                // Introduce a guard? (only if it is a method/accessor/constructor, i.e. somewhere that allows code)
                var containingBlock = node.FirstAncestorOrSelf<BlockSyntax>();
                if (containingBlock != null)
                {
                    var act1 = CodeAction.Create($"Add 'If ApiInformation.{g.KindOfCheck}'", (c) => IntroduceGuardAsync(context.Document, node, g, c), "PlatformSpecificGuard");
                    context.RegisterCodeFix(act1, diagnostic);
                }
            }
            catch
            {
            }
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
                    var conditionInt3 = SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(g.ParameterCountToCheck ?? 0));

                    IEnumerable<ArgumentSyntax> conditions = null;

                    if (g.ParameterCountToCheck.HasValue)
                    {
                        conditions = new ArgumentSyntax[] { SyntaxFactory.Argument(conditionString1), SyntaxFactory.Argument(conditionString2), SyntaxFactory.Argument(conditionInt3) };
                    }
                    else
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
