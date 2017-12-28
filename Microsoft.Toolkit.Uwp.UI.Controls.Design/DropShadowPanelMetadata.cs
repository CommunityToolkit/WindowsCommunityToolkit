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

using Microsoft.Toolkit.Uwp.UI.Controls.Design.Common;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Features;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.Model;
using Microsoft.Windows.Design.PropertyEditing;
using System.ComponentModel;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design

{
    internal class DropShadowPanelDefaults : DefaultInitializer
    {
        public override void InitializeDefaults(ModelItem item)
        {
            item.Properties[nameof(DropShadowPanel.BlurRadius)].SetValue(5d);
        }
    }

	internal class DropShadowPanelMetadata : AttributeTableBuilder
	{
		public DropShadowPanelMetadata()
			: base()
		{
            AddCallback(typeof(DropShadowPanel),
                b =>
                {
                    b.AddCustomAttributes(new FeatureAttribute(typeof(DropShadowPanelDefaults)));

                    b.AddCustomAttributes(nameof(DropShadowPanel.BlurRadius),
                        new PropertyOrderAttribute(PropertyOrder.Early),
                        new CategoryAttribute(Properties.Resources.CategoryDropShadow)
                        );
                    b.AddCustomAttributes(nameof(DropShadowPanel.ShadowOpacity),
                       new PropertyOrderAttribute(PropertyOrder.Early),
                       new CategoryAttribute(Properties.Resources.CategoryDropShadow)
                       );
                    b.AddCustomAttributes(nameof(DropShadowPanel.Color),
                        new CategoryAttribute(Properties.Resources.CategoryDropShadow)
                        );
                    b.AddCustomAttributes(nameof(DropShadowPanel.OffsetX),
                       new PropertyOrderAttribute(PropertyOrder.Late),
                        new CategoryAttribute(Properties.Resources.CategoryDropShadow)
                        );
                    b.AddCustomAttributes(nameof(DropShadowPanel.OffsetY),
                       new PropertyOrderAttribute(PropertyOrder.Late),
                       new CategoryAttribute(Properties.Resources.CategoryDropShadow)
                       );
                    b.AddCustomAttributes(nameof(DropShadowPanel.OffsetZ),
                       new PropertyOrderAttribute(PropertyOrder.Late),
                       new CategoryAttribute(Properties.Resources.CategoryDropShadow)
                       );

                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
                }
            );
		}
	}
}