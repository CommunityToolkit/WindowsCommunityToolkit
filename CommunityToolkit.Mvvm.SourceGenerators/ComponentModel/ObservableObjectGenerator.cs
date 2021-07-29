// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static CommunityToolkit.Mvvm.SourceGenerators.Diagnostics.DiagnosticDescriptors;

namespace CommunityToolkit.Mvvm.SourceGenerators
{
    /// <summary>
    /// A source generator for the <c>ObservableObjectAttribute</c> type.
    /// </summary>
    [Generator]
    public sealed class ObservableObjectGenerator : TransitiveMembersGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableObjectGenerator"/> class.
        /// </summary>
        public ObservableObjectGenerator()
            : base("CommunityToolkit.Mvvm.ComponentModel.ObservableObjectAttribute")
        {
        }

        /// <inheritdoc/>
        protected override DiagnosticDescriptor TargetTypeErrorDescriptor => ObservableObjectGeneratorError;

        /// <inheritdoc/>
        protected override bool ValidateTargetType(
            GeneratorExecutionContext context,
            AttributeData attributeData,
            ClassDeclarationSyntax classDeclaration,
            INamedTypeSymbol classDeclarationSymbol,
            [NotNullWhen(false)] out DiagnosticDescriptor? descriptor)
        {
            INamedTypeSymbol
                iNotifyPropertyChangedSymbol = context.Compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged")!,
                iNotifyPropertyChangingSymbol = context.Compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanging")!;

            // Check if the type already implements INotifyPropertyChanged...
            if (classDeclarationSymbol.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i, iNotifyPropertyChangedSymbol)))
            {
                descriptor = DuplicateINotifyPropertyChangedInterfaceForObservableObjectAttributeError;

                return false;
            }

            // ...or INotifyPropertyChanging
            if (classDeclarationSymbol.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i, iNotifyPropertyChangingSymbol)))
            {
                descriptor = DuplicateINotifyPropertyChangingInterfaceForObservableObjectAttributeError;

                return false;
            }

            descriptor = null;

            return true;
        }
    }
}
