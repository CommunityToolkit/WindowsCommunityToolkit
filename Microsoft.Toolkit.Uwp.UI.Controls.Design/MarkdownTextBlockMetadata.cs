// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Controls.Design.Common;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;
using System.ComponentModel;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design
{
	internal class MarkdownTextBlockMetadata : AttributeTableBuilder
	{
        public MarkdownTextBlockMetadata()
			: base()
		{
			AddCallback(typeof(Microsoft.Toolkit.Uwp.UI.Controls.MarkdownTextBlock),
				b =>
				{
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.ImageStretch), new CategoryAttribute(Properties.Resources.CategoryMarkdownStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.Text), new CategoryAttribute(Properties.Resources.CategoryMarkdownStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.IsTextSelectionEnabled), new CategoryAttribute(Properties.Resources.CategoryMarkdownStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.LinkForeground), new CategoryAttribute(Properties.Resources.CategoryMarkdownStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.TextWrapping), new CategoryAttribute(Properties.Resources.CategoryMarkdownStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.ParagraphMargin), new CategoryAttribute(Properties.Resources.CategoryMarkdownStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.HorizontalRuleBrush), new CategoryAttribute(Properties.Resources.CategoryMarkdownStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.HorizontalRuleMargin), new CategoryAttribute(Properties.Resources.CategoryMarkdownStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.HorizontalRuleThickness), new CategoryAttribute(Properties.Resources.CategoryMarkdownStyle));

                    b.AddCustomAttributes(nameof(MarkdownTextBlock.CodeBackground), new CategoryAttribute(Properties.Resources.CategoryMarkdownCodeStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.CodeBorderBrush), new CategoryAttribute(Properties.Resources.CategoryMarkdownCodeStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.CodeBorderThickness), new CategoryAttribute(Properties.Resources.CategoryMarkdownCodeStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.CodeForeground), new CategoryAttribute(Properties.Resources.CategoryMarkdownCodeStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.CodeBackground), new CategoryAttribute(Properties.Resources.CategoryMarkdownCodeStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.CodeFontFamily), new CategoryAttribute(Properties.Resources.CategoryMarkdownCodeStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.CodeMargin), new CategoryAttribute(Properties.Resources.CategoryMarkdownCodeStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.CodePadding), new CategoryAttribute(Properties.Resources.CategoryMarkdownCodeStyle));

                    for (int i = 1; i <= 6; i++)
                    {
                        b.AddCustomAttributes($"Header{i}FontWeight", new CategoryAttribute(string.Format(Properties.Resources.CategoryMarkdownHeaderStyle, i)));
                        b.AddCustomAttributes($"Header{i}FontSize", new CategoryAttribute(string.Format(Properties.Resources.CategoryMarkdownHeaderStyle, i)));
                        b.AddCustomAttributes($"Header{i}Margin", new CategoryAttribute(string.Format(Properties.Resources.CategoryMarkdownHeaderStyle, i)));
                        b.AddCustomAttributes($"Header{i}Foreground", new CategoryAttribute(string.Format(Properties.Resources.CategoryMarkdownHeaderStyle, i)));
                    }

                    b.AddCustomAttributes(nameof(MarkdownTextBlock.ListMargin), new CategoryAttribute(Properties.Resources.CategoryMarkdownListStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.ListGutterWidth), new CategoryAttribute(Properties.Resources.CategoryMarkdownListStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.ListBulletSpacing), new CategoryAttribute(Properties.Resources.CategoryMarkdownListStyle));

                    b.AddCustomAttributes(nameof(MarkdownTextBlock.QuoteBackground), new CategoryAttribute(Properties.Resources.CategoryMarkdownQuoteStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.QuoteBorderBrush), new CategoryAttribute(Properties.Resources.CategoryMarkdownQuoteStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.QuoteBorderThickness), new CategoryAttribute(Properties.Resources.CategoryMarkdownQuoteStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.QuoteForeground), new CategoryAttribute(Properties.Resources.CategoryMarkdownQuoteStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.QuoteMargin), new CategoryAttribute(Properties.Resources.CategoryMarkdownQuoteStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.QuotePadding), new CategoryAttribute(Properties.Resources.CategoryMarkdownQuoteStyle));

                    b.AddCustomAttributes(nameof(MarkdownTextBlock.TableBorderBrush), new CategoryAttribute(Properties.Resources.CategoryMarkdownTableStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.TableBorderThickness), new CategoryAttribute(Properties.Resources.CategoryMarkdownTableStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.TableCellPadding), new CategoryAttribute(Properties.Resources.CategoryMarkdownTableStyle));
                    b.AddCustomAttributes(nameof(MarkdownTextBlock.TableMargin), new CategoryAttribute(Properties.Resources.CategoryMarkdownTableStyle));

                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
				}
			);
		}
	}
}
