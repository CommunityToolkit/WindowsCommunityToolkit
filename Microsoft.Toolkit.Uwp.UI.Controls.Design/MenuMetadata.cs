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
	internal class MenuMetadata : AttributeTableBuilder
	{
        public MenuMetadata()
			: base()
		{
#pragma warning disable 0618 //Ignore obsolete warning
			AddCallback(typeof(Microsoft.Toolkit.Uwp.UI.Controls.Menu),
#pragma warning restore 0618 
				b =>
				{   
					b.AddCustomAttributes(nameof(Menu.Items),
						new PropertyOrderAttribute(PropertyOrder.Early),
						new CategoryAttribute(Properties.Resources.CategoryCommon),
						//The following is necessary because this is a collection of an abstract type, so we help
						//the designer with populating supported types that can be added to the collection
                        new NewItemTypesAttribute(new System.Type[] {
#pragma warning disable 0618 //Ignore obsolete warning
                            typeof(Microsoft.Toolkit.Uwp.UI.Controls.MenuItem),
#pragma warning restore 0618 
                        }),
						new AlternateContentPropertyAttribute()
					);
					b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
				}
			);
		}
	}
}
