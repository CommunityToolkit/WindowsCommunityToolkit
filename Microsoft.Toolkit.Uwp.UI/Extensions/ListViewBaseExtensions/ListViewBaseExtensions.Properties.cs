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

using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Provides attached dependency properties for the <see cref="ListViewBase"/>
    /// </summary>
    public partial class ListViewBaseExtensions
    {
        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding an <see cref="ICommand"/> instance to a <see cref="ListViewBase"/>
        /// This ICommand is executed when ListViewBase Item receives interaction by means of ItemClick. This requires IsItemClickEnabled to set to true.
        /// The ICommand is passed the Item that received interaction as a parameter
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ListViewBaseExtensions), new PropertyMetadata(null, OnCommandPropertyChanged));

        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding a <see cref="Brush"/> as an alternate background color to a <see cref="ListViewBase"/>
        /// </summary>
        public static readonly DependencyProperty AlternateColorProperty = DependencyProperty.RegisterAttached("AlternateColor", typeof(Brush), typeof(ListViewBaseExtensions), new PropertyMetadata(null, OnAlternateColorPropertyChanged));

        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding a <see cref="DataTemplate"/> as an alternate template to a <see cref="ListViewBase"/>
        /// </summary>
        public static readonly DependencyProperty AlternateItemTemplateProperty = DependencyProperty.RegisterAttached("AlternateItemTemplate", typeof(DataTemplate), typeof(ListViewBaseExtensions), new PropertyMetadata(null, OnAlternateItemTemplatePropertyChanged));

        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for setting the container content stretch direction on the <see cref="ListViewBase"/>
        /// </summary>
        public static readonly DependencyProperty StretchItemContainerDirectionProperty = DependencyProperty.RegisterAttached("StretchItemContainerDirection", typeof(StretchDirection), typeof(ListViewBaseExtensions), new PropertyMetadata(null, OnStretchItemContainerDirectionPropertyChanged));

        /// <summary>
        /// Gets the <see cref="ICommand"/> instance assocaited with the specified <see cref="ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="ListViewBase"/> from which to get the associated <see cref="ICommand"/> instance</param>
        /// <returns>The <see cref="ICommand"/> instance associated with the the <see cref="ListViewBase"/> or null</returns>
        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        /// <summary>
        /// Sets the <see cref="ICommand"/> instance assocaited with the specified <see cref="ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="ListViewBase"/> to associated the <see cref="ICommand"/> instance to</param>
        /// <param name="value">The <see cref="ICommand"/> instance to bind to the <see cref="ListViewBase"/></param>
        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Gets the alternate <see cref="Brush"/> associated with the specified <see cref="ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="ListViewBase"/> to get the associated <see cref="Brush"/> from</param>
        /// <returns>The <see cref="Brush"/> associated with the <see cref="ListViewBase"/></returns>
        public static Brush GetAlternateColor(ListViewBase obj)
        {
            return (Brush)obj.GetValue(AlternateColorProperty);
        }

        /// <summary>
        /// Sets the alternate <see cref="Brush"/> associated with the specified <see cref="DependencyObject"/>
        /// </summary>
        /// <param name="obj">The <see cref="ListViewBase"/> to associate the <see cref="Brush"/> with</param>
        /// <param name="value">The <see cref="Brush"/> for binding to the <see cref="ListViewBase"/></param>
        public static void SetAlternateColor(ListViewBase obj, Brush value)
        {
            obj.SetValue(AlternateColorProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="DataTemplate"/> associated with the specified <see cref="ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="ListViewBase"/> to get the associated <see cref="DataTemplate"/> from</param>
        /// <returns>The <see cref="DataTemplate"/> associated with the <see cref="ListViewBase"/></returns>
        public static DataTemplate GetAlternateItemTemplate(ListViewBase obj)
        {
            return (DataTemplate)obj.GetValue(AlternateItemTemplateProperty);
        }

        /// <summary>
        /// Sets the <see cref="DataTemplate"/> associated with the specified <see cref="ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="ListViewBase"/> to associate the <see cref="DataTemplate"/> with</param>
        /// <param name="value">The <see cref="DataTemplate"/> for binding to the <see cref="ListViewBase"/></param>
        public static void SetAlternateItemTemplate(ListViewBase obj, DataTemplate value)
        {
            obj.SetValue(AlternateItemTemplateProperty, value);
        }

        /// <summary>
        /// Gets the stretch <see cref="StretchDirection"/> associated with the specified <see cref="ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="ListViewBase"/> to get the associated <see cref="StretchDirection"/> from</param>
        /// <returns>The <see cref="StretchDirection"/> associated with the <see cref="ListViewBase"/></returns>
        public static StretchDirection GetStretchItemContainerDirection(ListViewBase obj)
        {
            return (StretchDirection)obj.GetValue(StretchItemContainerDirectionProperty);
        }

        /// <summary>
        /// Sets the stretch <see cref="StretchDirection"/> associated with the specified <see cref="ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="ListViewBase"/> to associate the <see cref="StretchDirection"/> with</param>
        /// <param name="value">The <see cref="StretchDirection"/> for binding to the <see cref="ListViewBase"/></param>
        public static void SetStretchItemContainerDirection(ListViewBase obj, StretchDirection value)
        {
            obj.SetValue(StretchItemContainerDirectionProperty, value);
        }

    }
}
