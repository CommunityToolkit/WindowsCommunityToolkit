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

using System;
using System.Reflection;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Toolkit.Uwp.Design.Common;

[assembly: ProvideMetadata(typeof(Microsoft.Toolkit.Uwp.UI.Controls.Design.MetadataRegistration))]

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design
{
	public class MetadataRegistration : MetadataRegistrationBase, IProvideAttributeTable
	{
		public MetadataRegistration() : base()
		{
			// Note:
			// The default constructor sets value of AssemblyFullName and 
			// XmlResourceName used by MetadataRegistrationBase.AddDescriptions().
			// The convention here is that the <RootNamespace> in .design.csproj
			// (or Default namespace in Project -> Properties -> Application tab)
			// must be the same as runtime assembly's main namespace (t.Namespace)
			// plus .Design.
            Type t = typeof(Microsoft.Toolkit.Uwp.UI.Controls.DataGrid);
			AssemblyName an = t.Assembly.GetName();
			AssemblyFullName = ", " + an.FullName;
			XmlResourceName = t.Namespace + ".Design." + an.Name + ".xml";
		}

		#region IProvideAttributeTable Members

		/// <summary>
		/// Gets the AttributeTable for design time metadata.
		/// </summary>
		public AttributeTable AttributeTable
		{
			get
			{
				return BuildAttributeTable();
			}
		}

		#endregion
	}
}