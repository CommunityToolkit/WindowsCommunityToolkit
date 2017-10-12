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
    internal class RadialProgressBarDefaults : DefaultInitializer
    {
        public override void InitializeDefaults(ModelItem item)
        {
            item.Properties[nameof(RadialProgressBar.Value)].SetValue(0d);
        }
    }

    internal class RadialProgressBarMetadata : AttributeTableBuilder
	{
        public RadialProgressBarMetadata()
			: base()
		{
			AddCallback(typeof(Microsoft.Toolkit.Uwp.UI.Controls.RadialProgressBar),
				b =>
				{
                    b.AddCustomAttributes(new FeatureAttribute(typeof(RadialProgressBarDefaults)));
                    b.AddCustomAttributes(nameof(RadialProgressBar.Thickness), new CategoryAttribute(Properties.Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(RadialProgressBar.Outline), new CategoryAttribute(Properties.Resources.CategoryBrush));
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
				}
			);
		}
	}
}
