﻿//
// Copyright (c) 2013 Morten Nielsen
//
// Licensed under the Microsoft Public License (Ms-PL) (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://opensource.org/licenses/Ms-PL.html
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Reflection;
using Callisto.Design.Common;
using Microsoft.Windows.Design.Metadata;

[assembly: ProvideMetadata(typeof(Callisto.Design.MetadataRegistration))]

namespace Callisto.Design
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
            Type t = typeof(Callisto.WebViewExtension);
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