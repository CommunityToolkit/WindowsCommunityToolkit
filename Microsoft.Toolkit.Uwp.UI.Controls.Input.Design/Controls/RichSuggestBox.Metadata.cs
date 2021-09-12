// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

using Microsoft.Toolkit.Uwp.UI.Controls.Design.Properties;

using Microsoft.VisualStudio.DesignTools.Extensibility;
using Microsoft.VisualStudio.DesignTools.Extensibility.Metadata;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design
{
    internal class RichSuggestBoxMetadata : AttributeTableBuilder
    {
        public RichSuggestBoxMetadata()
            : base()
        {
            AddCallback(ControlTypes.RichSuggestBox,
                b =>
                {
                    b.AddCustomAttributes(nameof(RichSuggestBox.ClipboardCopyFormat), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RichSuggestBox.ClipboardPasteFormat), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RichSuggestBox.Description), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RichSuggestBox.DisabledFormattingAccelerators), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RichSuggestBox.Header), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RichSuggestBox.HeaderTemplate),
                        new CategoryAttribute(Resources.CategoryAppearance),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced)
                    );
                    b.AddCustomAttributes(nameof(RichSuggestBox.PlaceholderText), new CategoryAttribute(Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(RichSuggestBox.PopupCornerRadius), new CategoryAttribute(Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(RichSuggestBox.PopupFooter), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RichSuggestBox.PopupFooterTemplate),
                        new CategoryAttribute(Resources.CategoryAppearance),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced)
                    );
                    b.AddCustomAttributes(nameof(RichSuggestBox.PopupHeader), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RichSuggestBox.PopupHeaderTemplate),
                        new CategoryAttribute(Resources.CategoryAppearance),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced)
                    );
                    b.AddCustomAttributes(nameof(RichSuggestBox.PopupPlacement), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RichSuggestBox.Prefixes), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RichSuggestBox.RichEditBoxStyle), new CategoryAttribute(Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(RichSuggestBox.TokenBackground), new CategoryAttribute(Resources.CategoryBrush));
                    b.AddCustomAttributes(nameof(RichSuggestBox.TokenForeground), new CategoryAttribute(Resources.CategoryBrush));
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
                }
            );
        }
    }
}