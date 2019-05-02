// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
            Type t = typeof(Microsoft.Toolkit.Uwp.UI.Controls.GridSplitter);
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