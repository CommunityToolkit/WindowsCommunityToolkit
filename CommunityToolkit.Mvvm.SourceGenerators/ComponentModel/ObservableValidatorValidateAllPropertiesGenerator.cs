// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.SourceGenerators.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static CommunityToolkit.Mvvm.SourceGenerators.Diagnostics.DiagnosticDescriptors;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

#pragma warning disable SA1008

namespace CommunityToolkit.Mvvm.SourceGenerators
{
    /// <summary>
    /// A source generator for properties validation without relying on compiled LINQ expressions.
    /// </summary>
    [Generator]
    public sealed partial class ObservableValidatorValidateAllPropertiesGenerator : ISourceGenerator
    {
        /// <inheritdoc/>
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(static () => new SyntaxReceiver());
        }

        /// <inheritdoc/>
        public void Execute(GeneratorExecutionContext context)
        {
            // Get the syntax receiver with the candidate nodes
            if (context.SyntaxContextReceiver is not SyntaxReceiver syntaxReceiver ||
                syntaxReceiver.GatheredInfo.Count == 0)
            {
                return;
            }

            // Validate the language version (this needs at least C# 8.0 due to static local functions being used).
            // If a lower C# version is set, just skip the execution silently. The fallback path will be used just fine.
            if (context.ParseOptions is not CSharpParseOptions { LanguageVersion: >= LanguageVersion.CSharp8 })
            {
                return;
            }

            // Get the symbol for the required attributes
            INamedTypeSymbol
                validationSymbol = context.Compilation.GetTypeByMetadataName("System.ComponentModel.DataAnnotations.ValidationAttribute")!,
                observablePropertySymbol = context.Compilation.GetTypeByMetadataName("CommunityToolkit.Mvvm.ComponentModel.ObservablePropertyAttribute")!;

            // Prepare the attributes to add to the first class declaration
            AttributeListSyntax[] classAttributes = new[]
            {
                AttributeList(SingletonSeparatedList(
                    Attribute(IdentifierName($"global::System.CodeDom.Compiler.GeneratedCode"))
                    .AddArgumentListArguments(
                        AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(GetType().FullName))),
                        AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(GetType().Assembly.GetName().Version.ToString())))))),
                AttributeList(SingletonSeparatedList(Attribute(IdentifierName("global::System.Diagnostics.DebuggerNonUserCode")))),
                AttributeList(SingletonSeparatedList(Attribute(IdentifierName("global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage")))),
                AttributeList(SingletonSeparatedList(
                    Attribute(IdentifierName("global::System.ComponentModel.EditorBrowsable")).AddArgumentListArguments(
                    AttributeArgument(ParseExpression("global::System.ComponentModel.EditorBrowsableState.Never"))))),
                AttributeList(SingletonSeparatedList(
                    Attribute(IdentifierName("global::System.Obsolete")).AddArgumentListArguments(
                    AttributeArgument(LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        Literal("This type is not intended to be used directly by user code"))))))
            };

            foreach (INamedTypeSymbol classSymbol in syntaxReceiver.GatheredInfo)
            {
                // Create a static factory method creating a delegate that can be used to validate all properties in a given class.
                // This pattern is used so that the library doesn't have to use MakeGenericType(...) at runtime, nor use unsafe casts
                // over the created delegate to be able to cache it as an Action<object> instance. This pattern enables the same
                // functionality and with almost identical performance (not noticeable in this context anyway), but while preserving
                // full runtime type safety (as a safe cast is used to validate the input argument), and with less reflection needed.
                // Note that we're deliberately creating a new delegate instance here and not using code that could see the C# compiler
                // create a static class to cache a reusable delegate, because each generated method will only be called at most once,
                // as the returned delegate will be cached by the MVVM Toolkit itself. So this ensures the the produced code is minimal,
                // and that there will be no unnecessary static fields and objects being created and possibly never collected.
                // This code takes a class symbol and produces a compilation unit as follows:
                //
                // // Licensed to the .NET Foundation under one or more agreements.
                // // The .NET Foundation licenses this file to you under the MIT license.
                // // See the LICENSE file in the project root for more information.
                //
                // #pragma warning disable
                //
                // namespace CommunityToolkit.Mvvm.ComponentModel.__Internals
                // {
                //     [global::System.CodeDom.Compiler.GeneratedCode("...", "...")]
                //     [global::System.Diagnostics.DebuggerNonUserCode]
                //     [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
                //     [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
                //     [global::System.Obsolete("This type is not intended to be used directly by user code")]
                //     internal static partial class __ObservableValidatorExtensions
                //     {
                //         [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
                //         [global::System.Obsolete("This method is not intended to be called directly by user code")]
                //         public static global::System.Action<object> CreateAllPropertiesValidator(<INSTANCE_TYPE> _)
                //         {
                //             static void ValidateAllProperties(object obj)
                //             {
                //                 var instance = (<INSTANCE_TYPE>)obj;
                //                 <BODY>
                //             }
                //
                //             return ValidateAllProperties;
                //         }
                //     }
                // }
                var source =
                    CompilationUnit().AddMembers(
                    NamespaceDeclaration(IdentifierName("CommunityToolkit.Mvvm.ComponentModel.__Internals")).WithLeadingTrivia(TriviaList(
                        Comment("// Licensed to the .NET Foundation under one or more agreements."),
                        Comment("// The .NET Foundation licenses this file to you under the MIT license."),
                        Comment("// See the LICENSE file in the project root for more information."),
                        Trivia(PragmaWarningDirectiveTrivia(Token(SyntaxKind.DisableKeyword), true)))).AddMembers(
                    ClassDeclaration("__ObservableValidatorExtensions").AddModifiers(
                        Token(SyntaxKind.InternalKeyword),
                        Token(SyntaxKind.StaticKeyword),
                        Token(SyntaxKind.PartialKeyword)).AddAttributeLists(classAttributes).AddMembers(
                    MethodDeclaration(
                        GenericName("global::System.Action").AddTypeArgumentListArguments(PredefinedType(Token(SyntaxKind.ObjectKeyword))),
                        Identifier("CreateAllPropertiesValidator")).AddAttributeLists(
                            AttributeList(SingletonSeparatedList(
                                Attribute(IdentifierName("global::System.ComponentModel.EditorBrowsable")).AddArgumentListArguments(
                                AttributeArgument(ParseExpression("global::System.ComponentModel.EditorBrowsableState.Never"))))),
                            AttributeList(SingletonSeparatedList(
                                Attribute(IdentifierName("global::System.Obsolete")).AddArgumentListArguments(
                                AttributeArgument(LiteralExpression(
                                    SyntaxKind.StringLiteralExpression,
                                    Literal("This method is not intended to be called directly by user code"))))))).AddModifiers(
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.StaticKeyword)).AddParameterListParameters(
                            Parameter(Identifier("_")).WithType(IdentifierName(classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))))
                        .WithBody(Block(
                            LocalFunctionStatement(
                                PredefinedType(Token(SyntaxKind.VoidKeyword)),
                                Identifier("ValidateAllProperties"))
                            .AddModifiers(Token(SyntaxKind.StaticKeyword))
                            .AddParameterListParameters(
                                Parameter(Identifier("obj")).WithType(PredefinedType(Token(SyntaxKind.ObjectKeyword))))
                            .WithBody(Block(
                                LocalDeclarationStatement(
                                    VariableDeclaration(IdentifierName("var")) // Cannot use Token(SyntaxKind.VarKeyword) here (throws an ArgumentException)
                                    .AddVariables(
                                        VariableDeclarator(Identifier("instance"))
                                        .WithInitializer(EqualsValueClause(
                                            CastExpression(
                                                IdentifierName(classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)),
                                                IdentifierName("obj")))))))
                                .AddStatements(EnumerateValidationStatements(classSymbol, validationSymbol, observablePropertySymbol).ToArray())),
                            ReturnStatement(IdentifierName("ValidateAllProperties")))))))
                    .NormalizeWhitespace()
                    .ToFullString();

                // Reset the attributes list (so the same class doesn't get duplicate attributes)
                classAttributes = Array.Empty<AttributeListSyntax>();

                // Add the partial type
                context.AddSource($"{classSymbol.GetFullMetadataNameForFileName()}.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        /// <summary>
        /// Gets a sequence of statements to validate declared properties (including generated ones).
        /// </summary>
        /// <param name="classSymbol">The input <see cref="INamedTypeSymbol"/> instance to process.</param>
        /// <param name="validationSymbol">The type symbol for the <c>ValidationAttribute</c> type.</param>
        /// <param name="observablePropertySymbol">The type symbol for the <c>ObservablePropertyAttribute</c> type.</param>
        /// <returns>The sequence of <see cref="StatementSyntax"/> instances to validate declared properties.</returns>
        [Pure]
        private static IEnumerable<StatementSyntax> EnumerateValidationStatements(INamedTypeSymbol classSymbol, INamedTypeSymbol validationSymbol, INamedTypeSymbol observablePropertySymbol)
        {
            foreach (var memberSymbol in classSymbol.GetMembers())
            {
                if (memberSymbol is not (IPropertySymbol { IsIndexer: false } or IFieldSymbol))
                {
                    continue;
                }

                ImmutableArray<AttributeData> attributes = memberSymbol.GetAttributes();

                // Also include fields that are annotated with [ObservableProperty]. This is necessary because
                // all generators run in an undefined order and looking at the same original compilation, so the
                // current one wouldn't be able to see generated properties from other generators directly.
                if (memberSymbol is IFieldSymbol &&
                    !attributes.Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, observablePropertySymbol)))
                {
                    continue;
                }

                // Skip the current member if there are no validation attributes applied to it
                if (!attributes.Any(a => a.AttributeClass?.InheritsFrom(validationSymbol) == true))
                {
                    continue;
                }

                // Get the target property name either directly or matching the generated one
                string propertyName = memberSymbol switch
                {
                    IPropertySymbol propertySymbol => propertySymbol.Name,
                    IFieldSymbol fieldSymbol => ObservablePropertyGenerator.GetGeneratedPropertyName(fieldSymbol),
                    _ => throw new InvalidOperationException("Invalid symbol type")
                };

                // This enumerator produces a sequence of statements as follows:
                //
                // __ObservableValidatorHelper.ValidateProperty(instance, instance.<PROPERTY_0>, nameof(instance.<PROPERTY_0>));
                // __ObservableValidatorHelper.ValidateProperty(instance, instance.<PROPERTY_0>, nameof(instance.<PROPERTY_0>));
                // ...
                // __ObservableValidatorHelper.ValidateProperty(instance, instance.<PROPERTY_1>, nameof(instance.<PROPERTY_1>));
                yield return
                    ExpressionStatement(
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName("__ObservableValidatorHelper"),
                                IdentifierName("ValidateProperty")))
                        .AddArgumentListArguments(
                            Argument(IdentifierName("instance")),
                            Argument(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("instance"),
                                    IdentifierName(propertyName))),
                            Argument(
                                InvocationExpression(IdentifierName("nameof"))
                                .AddArgumentListArguments(Argument(
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName("instance"),
                                        IdentifierName(propertyName)))))));
            }
        }
    }
}
