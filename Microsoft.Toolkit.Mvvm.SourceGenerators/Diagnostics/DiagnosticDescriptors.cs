﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Microsoft.CodeAnalysis;

namespace Microsoft.Toolkit.Mvvm.SourceGenerators.Diagnostics
{
    /// <summary>
    /// A container for all <see cref="DiagnosticDescriptor"/> instances for errors reported by analyzers in this project.
    /// </summary>
    internal static class DiagnosticDescriptors
    {
        /// <summary>
        /// Gets a <see cref="DiagnosticDescriptor"/> indicating when <see cref="INotifyPropertyChangedGenerator"/> failed to run on a given type.
        /// <para>
        /// Format: <c>"The generator INotifyPropertyChangedGenerator failed to execute on type {0}"</c>.
        /// </para>
        /// </summary>
        public static readonly DiagnosticDescriptor INotifyPropertyChangedGeneratorError = new(
            id: "MVVMTK0001",
            title: $"Internal error for {nameof(INotifyPropertyChangedGenerator)}",
            messageFormat: $"The generator {nameof(INotifyPropertyChangedGenerator)} failed to execute on type {{0}}",
            category: typeof(INotifyPropertyChangedGenerator).FullName,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: $"The {nameof(INotifyPropertyChangedGenerator)} generator encountered an error while processing a type. Please report this issue at https://aka.ms/mvvmtoolkit.",
            helpLinkUri: "https://aka.ms/mvvmtoolkit");

        /// <summary>
        /// Gets a <see cref="DiagnosticDescriptor"/> indicating when <see cref="ObservableObjectGenerator"/> failed to run on a given type.
        /// <para>
        /// Format: <c>"The generator ObservableObjectGenerator failed to execute on type {0}"</c>.
        /// </para>
        /// </summary>
        public static readonly DiagnosticDescriptor ObservableObjectGeneratorError = new(
            id: "MVVMTK0002",
            title: $"Internal error for {nameof(ObservableObjectGenerator)}",
            messageFormat: $"The generator {nameof(ObservableObjectGenerator)} failed to execute on type {{0}}",
            category: typeof(ObservableObjectGenerator).FullName,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: $"The {nameof(ObservableObjectGenerator)} generator encountered an error while processing a type. Please report this issue at https://aka.ms/mvvmtoolkit.",
            helpLinkUri: "https://aka.ms/mvvmtoolkit");

        /// <summary>
        /// Gets a <see cref="DiagnosticDescriptor"/> indicating when <see cref="ObservableRecipientGenerator"/> failed to run on a given type.
        /// <para>
        /// Format: <c>"The generator ObservableRecipientGenerator failed to execute on type {0}"</c>.
        /// </para>
        /// </summary>
        public static readonly DiagnosticDescriptor ObservableRecipientGeneratorError = new(
            id: "MVVMTK0003",
            title: $"Internal error for {nameof(ObservableRecipientGenerator)}",
            messageFormat: $"The generator {nameof(ObservableRecipientGenerator)} failed to execute on type {{0}}",
            category: typeof(ObservableRecipientGenerator).FullName,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: $"The {nameof(ObservableRecipientGenerator)} generator encountered an error while processing a type. Please report this issue at https://aka.ms/mvvmtoolkit.",
            helpLinkUri: "https://aka.ms/mvvmtoolkit");

        /// <summary>
        /// Gets a <see cref="DiagnosticDescriptor"/> indicating when a duplicate declaration of <see cref="INotifyPropertyChanged"/> would happen.
        /// <para>
        /// Format: <c>"Cannot apply [INotifyPropertyChangedAttribute] to type {0}, as it already declares the INotifyPropertyChanged interface"</c>.
        /// </para>
        /// </summary>
        public static readonly DiagnosticDescriptor DuplicateINotifyPropertyChangedInterfaceForINotifyPropertyChangedAttributeError = new(
            id: "MVVMTK0004",
            title: $"Duplicate {nameof(INotifyPropertyChanged)} definition",
            messageFormat: $"Cannot apply [INotifyPropertyChanged] to type {{0}}, as it already declares the {nameof(INotifyPropertyChanged)} interface",
            category: typeof(INotifyPropertyChangedGenerator).FullName,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: $"Cannot apply [INotifyPropertyChanged] to a type that already declares the {nameof(INotifyPropertyChanged)} interface.",
            helpLinkUri: "https://aka.ms/mvvmtoolkit");

        /// <summary>
        /// Gets a <see cref="DiagnosticDescriptor"/> indicating when a duplicate declaration of <see cref="INotifyPropertyChanged"/> would happen.
        /// <para>
        /// Format: <c>"Cannot apply [ObservableObjectAttribute] to type {0}, as it already declares the INotifyPropertyChanged interface"</c>.
        /// </para>
        /// </summary>
        public static readonly DiagnosticDescriptor DuplicateINotifyPropertyChangedInterfaceForObservableObjectAttributeError = new(
            id: "MVVMTK0005",
            title: $"Duplicate {nameof(INotifyPropertyChanged)} definition",
            messageFormat: $"Cannot apply [ObservableObject] to type {{0}}, as it already declares the {nameof(INotifyPropertyChanged)} interface",
            category: typeof(ObservableObjectGenerator).FullName,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: $"Cannot apply [ObservableObject] to a type that already declares the {nameof(INotifyPropertyChanged)} interface.",
            helpLinkUri: "https://aka.ms/mvvmtoolkit");

        /// <summary>
        /// Gets a <see cref="DiagnosticDescriptor"/> indicating when a duplicate declaration of <see cref="INotifyPropertyChanging"/> would happen.
        /// <para>
        /// Format: <c>"Cannot apply [ObservableObjectAttribute] to type {0}, as it already declares the INotifyPropertyChanging interface"</c>.
        /// </para>
        /// </summary>
        public static readonly DiagnosticDescriptor DuplicateINotifyPropertyChangingInterfaceForObservableObjectAttributeError = new(
            id: "MVVMTK0006",
            title: $"Duplicate {nameof(INotifyPropertyChanging)} definition",
            messageFormat: $"Cannot apply [ObservableObject] to type {{0}}, as it already declares the {nameof(INotifyPropertyChanging)} interface",
            category: typeof(ObservableObjectGenerator).FullName,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: $"Cannot apply [ObservableObject] to a type that already declares the {nameof(INotifyPropertyChanging)} interface.",
            helpLinkUri: "https://aka.ms/mvvmtoolkit");

        /// <summary>
        /// Gets a <see cref="DiagnosticDescriptor"/> indicating when a duplicate declaration of <see cref="INotifyPropertyChanging"/> would happen.
        /// <para>
        /// Format: <c>"Cannot apply [ObservableRecipientAttribute] to type {0}, as it already inherits from the ObservableRecipient class"</c>.
        /// </para>
        /// </summary>
        public static readonly DiagnosticDescriptor DuplicateObservableRecipientError = new(
            id: "MVVMTK0007",
            title: "Duplicate ObservableRecipient definition",
            messageFormat: $"Cannot apply [ObservableRecipient] to type {{0}}, as it already inherits from the ObservableRecipient class",
            category: typeof(ObservableRecipientGenerator).FullName,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: $"Cannot apply [ObservableRecipient] to a type that already inherits from the ObservableRecipient class.",
            helpLinkUri: "https://aka.ms/mvvmtoolkit");

        /// <summary>
        /// Gets a <see cref="DiagnosticDescriptor"/> indicating when there is a missing base functionality to enable <c>ObservableRecipientAttribute</c>.
        /// <para>
        /// Format: <c>"Cannot apply [ObservableRecipientAttribute] to type {0}, as it lacks necessary base functionality (it should either inherit from ObservableObject, or be annotated with [ObservableObjectAttribute] or [INotifyPropertyChangedAttribute])"</c>.
        /// </para>
        /// </summary>
        public static readonly DiagnosticDescriptor MissingBaseObservableObjectFunctionalityError = new(
            id: "MVVMTK0008",
            title: "Missing base ObservableObject functionality",
            messageFormat: $"Cannot apply [ObservableRecipient] to type {{0}}, as it lacks necessary base functionality (it should either inherit from ObservableObject, or be annotated with [ObservableObject] or [INotifyPropertyChanged])",
            category: typeof(ObservableRecipientGenerator).FullName,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: $"Cannot apply [ObservableRecipient] to a type that lacks necessary base functionality (it should either inherit from ObservableObject, or be annotated with [ObservableObject] or [INotifyPropertyChanged]).",
            helpLinkUri: "https://aka.ms/mvvmtoolkit");

        /// <summary>
        /// Gets a <see cref="DiagnosticDescriptor"/> indicating when the target type doesn't inherit from the <c>ObservableValidator</c> class.
        /// <para>
        /// Format: <c>"The field {0}.{1} cannot be used to generate an observable property, as it has {2} validation attribute(s) but is declared in a type that doesn't inherit from ObservableValidator"</c>.
        /// </para>
        /// </summary>
        public static readonly DiagnosticDescriptor MissingObservableValidatorInheritanceError = new(
            id: "MVVMTK0009",
            title: "Missing ObservableValidator inheritance",
            messageFormat: "The field {0}.{1} cannot be used to generate an observable property, as it has {2} validation attribute(s) but is declared in a type that doesn't inherit from ObservableValidator",
            category: typeof(ObservablePropertyGenerator).FullName,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: $"Cannot apply [ObservableProperty] to fields with validation attributes if they are declared in a type that doesn't inherit from ObservableValidator.",
            helpLinkUri: "https://aka.ms/mvvmtoolkit");

        /// <summary>
        /// Gets a <see cref="DiagnosticDescriptor"/> indicating when <see cref="ObservablePropertyGenerator"/> failed to run on a given type.
        /// <para>
        /// Format: <c>"The generator ObservablePropertyGenerator failed to execute on type {0}"</c>.
        /// </para>
        /// </summary>
        public static readonly DiagnosticDescriptor ObservablePropertyGeneratorError = new(
            id: "MVVMTK0010",
            title: $"Internal error for {nameof(ObservablePropertyGenerator)}",
            messageFormat: $"The generator {nameof(ObservablePropertyGenerator)} failed to execute on type {{0}}",
            category: typeof(ObservableObjectGenerator).FullName,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: $"The {nameof(ObservablePropertyGenerator)} generator encountered an error while processing a type. Please report this issue at https://aka.ms/mvvmtoolkit.",
            helpLinkUri: "https://aka.ms/mvvmtoolkit");

        /// <summary>
        /// Gets a <see cref="DiagnosticDescriptor"/> indicating when <see cref="ICommandGenerator"/> failed to run on a given type.
        /// <para>
        /// Format: <c>"The generator ICommandGenerator failed to execute on type {0}"</c>.
        /// </para>
        /// </summary>
        public static readonly DiagnosticDescriptor ICommandGeneratorError = new(
            id: "MVVMTK0011",
            title: $"Internal error for {nameof(ICommandGenerator)}",
            messageFormat: $"The generator {nameof(ICommandGenerator)} failed to execute on type {{0}}",
            category: typeof(ICommandGenerator).FullName,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: $"The {nameof(ICommandGenerator)} generator encountered an error while processing a type. Please report this issue at https://aka.ms/mvvmtoolkit.",
            helpLinkUri: "https://aka.ms/mvvmtoolkit");

        /// <summary>
        /// Gets a <see cref="DiagnosticDescriptor"/> indicating when an annotated method to generate a command for has an invalid signature.
        /// <para>
        /// Format: <c>"The method {0}.{1} cannot be used to generate a command property, as its signature isn't compatible with any of the existing relay command types"</c>.
        /// </para>
        /// </summary>
        public static readonly DiagnosticDescriptor InvalidICommandMethodSignatureError = new(
            id: "MVVMTK0012",
            title: "Invalid ICommand method signature",
            messageFormat: "The method {0}.{1} cannot be used to generate a command property, as its signature isn't compatible with any of the existing relay command types",
            category: typeof(ICommandGenerator).FullName,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: $"Cannot apply [ICommand] to methods with a signature that doesn't match any of the existing relay command types.",
            helpLinkUri: "https://aka.ms/mvvmtoolkit");
    }
}
