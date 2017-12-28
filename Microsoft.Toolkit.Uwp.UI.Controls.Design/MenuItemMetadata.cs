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
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;
using System.ComponentModel;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design
{
	internal class MenuItemMetadata : AttributeTableBuilder
	{
        public MenuItemMetadata()
			: base()
		{
			AddCallback(typeof(MenuItem),
				b =>
				{   
					b.AddCustomAttributes(nameof(MenuItem.Header),
						new PropertyOrderAttribute(PropertyOrder.Early),
						new CategoryAttribute(Properties.Resources.CategoryCommon)
					);
                    b.AddCustomAttributes(nameof(MenuItem.IsOpened),
                        new CategoryAttribute(Properties.Resources.CategoryCommon)
                    );
                    b.AddCustomAttributes(nameof(MenuItem.HeaderTemplate),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced),
						new CategoryAttribute(Properties.Resources.CategoryCommon)
                    );
                    b.AddCustomAttributes(nameof(MenuItem.Items),
                        new PropertyOrderAttribute(PropertyOrder.Early),
                        new CategoryAttribute(Properties.Resources.CategoryCommon),
                        //The following is necessary because this is a collection of an abstract type, so we help
                        //the designer with populating supported types that can be added to the collection
                        new NewItemTypesAttribute(new System.Type[] {
                            typeof(MenuItem),
                        }),
                        new AlternateContentPropertyAttribute()
                    );

                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
                    b.AddCustomAttributes(new ToolboxBrowsableAttribute(false));
				}
			);
		}
	}
}
