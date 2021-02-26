// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.SampleApp.Enums;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the <see cref="EnumValuesExtension"/> type.
    /// </summary>
    public sealed partial class EnumValuesExtensionPage : IXamlRenderListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumValuesExtensionPage"/> class.
        /// </summary>
        public EnumValuesExtensionPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
        }
    }
}

#pragma warning disable SA1403 // File may only contain a single namespace
namespace Microsoft.Toolkit.Uwp.SampleApp.Enums
{
    public enum Animal
    {
        Cat,
        Dog,
        Bunny,
        Parrot,
        Squirrel
    }
}

namespace Microsoft.Toolkit.Uwp.SampleApp.Converters
{
    public sealed class AnimalToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (Animal)value switch
            {
                Animal.Cat => Colors.Coral,
                Animal.Dog => Colors.Gray,
                Animal.Bunny => Colors.Green,
                Animal.Parrot => Colors.YellowGreen,
                Animal.Squirrel => Colors.SaddleBrown,
                _ => throw new ArgumentException("Invalid value", nameof(value))
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

#pragma warning restore SA1403
