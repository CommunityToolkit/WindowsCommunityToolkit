// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Windows.Design.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.Design.Types
{
	internal class PlatformTypes
	{
		public static readonly Type DependencyObjectType = typeof(DependencyObject);
		public static readonly Type UIElementType = typeof(UIElement);
		public static readonly Type FrameworkElementType = typeof(FrameworkElement);
		public static readonly Type EditorBrowsableAttributeType = typeof(System.ComponentModel.EditorBrowsableAttribute);

		/// <summary>
		/// Used by MetadataRegistrationBase to get the browsable state
		/// </summary>
		/// <param name="editorBrowsableAttribute">This parameter must be of type 'System.ComponentModel.EditorBrowsableAttribute'</param>
		/// <returns></returns>
		public static bool IsBrowsable(object editorBrowsableAttribute)
		{
			if (editorBrowsableAttribute is System.ComponentModel.EditorBrowsableAttribute)
				return (editorBrowsableAttribute as System.ComponentModel.EditorBrowsableAttribute).State !=
					 System.ComponentModel.EditorBrowsableState.Never;
			return true;
		}

		public static class Control
		{
			public static readonly TypeIdentifier TypeId = new TypeIdentifier("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Control");
			public static readonly PropertyIdentifier BackgroundProperty = new PropertyIdentifier(TypeId, "Background");
			public static readonly PropertyIdentifier BorderBrushProperty = new PropertyIdentifier(TypeId, "BorderBrush");
			public static readonly PropertyIdentifier BorderThicknessProperty = new PropertyIdentifier(TypeId, "BorderThickness");
		}

		public static class FrameworkElement
		{
			public static readonly TypeIdentifier TypeId = new TypeIdentifier("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "FrameworkElement");
			public static readonly PropertyIdentifier MarginProperty = new PropertyIdentifier(TypeId, "Margin");
			public static readonly PropertyIdentifier HorizontalAlignmentProperty = new PropertyIdentifier(TypeId, "HorizontalAlignment");
			public static readonly PropertyIdentifier VerticalAlignmentProperty = new PropertyIdentifier(TypeId, "VerticalAlignment");
			public static readonly PropertyIdentifier HeightProperty = new PropertyIdentifier(TypeId, "Height");
			public static readonly PropertyIdentifier WidthProperty = new PropertyIdentifier(TypeId, "Width");
		}

	}
}
