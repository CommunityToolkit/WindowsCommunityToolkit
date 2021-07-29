// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;

namespace CommunityToolkit.Mvvm.ComponentModel
{
    /// <summary>
    /// An attribute that indicates that a given field should be wrapped by a generated observable property.
    /// In order to use this attribute, the containing type has to implement the <see cref="INotifyPropertyChanged"/> interface
    /// and expose a method with the same signature as <see cref="ObservableObject.OnPropertyChanged(string?)"/>. If the containing
    /// type also implements the <see cref="INotifyPropertyChanging"/> interface and exposes a method with the same signature as
    /// <see cref="ObservableObject.OnPropertyChanging(string?)"/>, then this method will be invoked as well by the property setter.
    /// <para>
    /// This attribute can be used as follows:
    /// <code>
    /// partial class MyViewModel : ObservableObject
    /// {
    ///     [ObservableProperty]
    ///     private string name;
    ///
    ///     [ObservableProperty]
    ///     private bool isEnabled;
    /// }
    /// </code>
    /// </para>
    /// And with this, code analogous to this will be generated:
    /// <code>
    /// partial class MyViewModel
    /// {
    ///     public string Name
    ///     {
    ///         get => name;
    ///         set => SetProperty(ref name, value);
    ///     }
    ///
    ///     public bool IsEnabled
    ///     {
    ///         get => name;
    ///         set => SetProperty(ref isEnabled, value);
    ///     }
    /// }
    /// </code>
    /// </summary>
    /// <remarks>
    /// The generated properties will automatically use the <c>UpperCamelCase</c> format for their names,
    /// which will be derived from the field names. The generator can also recognize fields using either
    /// the <c>_lowerCamel</c> or <c>m_lowerCamel</c> naming scheme. Otherwise, the first character in the
    /// source field name will be converted to uppercase (eg. <c>isEnabled</c> to <c>IsEnabled</c>).
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class ObservablePropertyAttribute : Attribute
    {
    }
}
