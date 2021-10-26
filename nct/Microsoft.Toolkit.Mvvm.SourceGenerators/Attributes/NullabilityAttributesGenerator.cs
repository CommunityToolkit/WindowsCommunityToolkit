// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.Toolkit.Mvvm.SourceGenerators
{
    /// <summary>
    /// A source generator for necessary nullability attributes.
    /// </summary>
    [Generator]
    public sealed class NullabilityAttributesGenerator : ISourceGenerator
    {
        /// <inheritdoc/>
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        /// <inheritdoc/>
        public void Execute(GeneratorExecutionContext context)
        {
            AddSourceCodeIfTypeIsNotPresent(context, "System.Diagnostics.CodeAnalysis.NotNullAttribute");
            AddSourceCodeIfTypeIsNotPresent(context, "System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute");
        }

        /// <summary>
        /// Adds the source for a given attribute type if it's not present already in the compilation.
        /// </summary>
        private void AddSourceCodeIfTypeIsNotPresent(GeneratorExecutionContext context, string typeFullName)
        {
            // Check that the target attributes are not available in the consuming project. To ensure that
            // this works fine both in .NET (Core) and .NET Standard implementations, we also need to check
            // that the target types are declared as public (we assume that in this case those types are from the BCL).
            // This avoids issues on .NET Standard with Roslyn also seeing internal types from referenced assemblies.
            if (context.Compilation.GetTypeByMetadataName(typeFullName) is { DeclaredAccessibility: Accessibility.Public })
            {
                return;
            }

            string
                typeName = typeFullName.Split('.').Last(),
                filename = $"Microsoft.Toolkit.Mvvm.SourceGenerators.EmbeddedResources.{typeName}.cs";

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filename);
            StreamReader reader = new(stream);

            string
                originalSource = reader.ReadToEnd(),
                outputSource = originalSource.Replace("NETSTANDARD2_0", "true");

            context.AddSource($"{typeFullName}.cs", SourceText.From(outputSource, Encoding.UTF8));
        }
    }
}
