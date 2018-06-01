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
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Simplification;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace Microsoft.Toolkit.Uwp.PlatformSpecificAnalyzer
{
    /// <summary>
    /// This class provides guard suggestion and can make the suggested changes.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.VisualBasic, Name = nameof(PlatformSpecificFixerCS))]
    [Shared]
    public class PlatformSpecificFixerVB : CodeFixProvider
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

                var target = PlatformSpecificAnalyzerVB.GetTargetOfNode(node, semanticModel);
                var g = Analyzer.GetGuardForSymbol(target);

                // Introduce a guard? (only if it is a method/accessor/constructor, i.e. somewhere that allows code)
                var containingBlock = node.FirstAncestorOrSelf<MethodBlockBaseSyntax>();
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
            // + If Windows.Foundation.Metadata.ApiInformation.IsTypePresent(targetContainingType) Then
            //      old-statement
            // + End If
            try
            {
                var oldStatement = node.FirstAncestorOrSelf<StatementSyntax>();
                var oldLeadingTrivia = oldStatement.GetLeadingTrivia();

                var conditionReceiver = SyntaxFactory.ParseName($"Windows.Foundation.Metadata.ApiInformation.{g.KindOfCheck}").WithAdditionalAnnotations(Simplifier.Annotation);
                ArgumentListSyntax conditionArgument = null;

                if (g.MemberToCheck == null)
                {
                    var conditionString1 = SyntaxFactory.StringLiteralExpression(SyntaxFactory.StringLiteralToken($"\"{g.TypeToCheck}\"", g.TypeToCheck));
                    conditionArgument = SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(SyntaxFactory.SimpleArgument(conditionString1)));
                }
                else
                {
                    var conditionString1 = SyntaxFactory.StringLiteralExpression(SyntaxFactory.StringLiteralToken($"\"{g.TypeToCheck}\"", g.TypeToCheck));
                    var conditionString2 = SyntaxFactory.StringLiteralExpression(SyntaxFactory.StringLiteralToken($"\"{g.MemberToCheck}\"", g.MemberToCheck));
                    var conditionInt3 = SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(g.ParameterCountToCheck ?? 0));

                    IEnumerable<ArgumentSyntax> conditions = null;

                    if (g.ParameterCountToCheck.HasValue)
                    {
                        conditions = new ArgumentSyntax[] { SyntaxFactory.SimpleArgument(conditionString1), SyntaxFactory.SimpleArgument(conditionString2), SyntaxFactory.SimpleArgument(conditionInt3) };
                    }
                    else
                    {
                        conditions = new ArgumentSyntax[] { SyntaxFactory.SimpleArgument(conditionString1), SyntaxFactory.SimpleArgument(conditionString2) };
                    }

                    conditionArgument = SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(conditions));
                }

                var condition = SyntaxFactory.InvocationExpression(conditionReceiver, conditionArgument);

                var ifStatement = SyntaxFactory.IfStatement(condition);
                var thenStatements = SyntaxFactory.SingletonList(oldStatement.WithoutLeadingTrivia());
                var ifBlock = SyntaxFactory.MultiLineIfBlock(ifStatement).WithStatements(thenStatements).WithLeadingTrivia(oldLeadingTrivia).WithAdditionalAnnotations(Formatter.Annotation);

                var oldRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
                var newRoot = oldRoot.ReplaceNode(oldStatement, ifBlock);

                return document.WithSyntaxRoot(newRoot);
            }
            catch
            {
            }

            return document;
        }
    }
}
