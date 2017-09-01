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

using Microsoft.Windows.Design.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design.Common
{
	public class MetadataRegistrationBase
	{
        AttributeTable masterMetadataTable;
        /// <summary>
		/// Build design time metadata attribute table.
		/// </summary>
		/// <returns>Custom attribute table.</returns>
		protected virtual AttributeTable BuildAttributeTable()
		{
			AttributeTableBuilder builder = new AttributeTableBuilder();

            AddDescriptions(builder);
            AddAttributes(builder);
            AddTables(builder, this);
            masterMetadataTable = builder.CreateTable();
			return masterMetadataTable;
		}

		/// <summary>
		/// Find all AttributeTableBuilder subclasses in the assembly 
		/// and add their attributes to the assembly attribute table.
		/// </summary>
		/// <param name="builder">The assembly attribute table builder.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Design time dll should not fail!")]
		private void AddTables(AttributeTableBuilder builder, object parent)
		{
			Debug.Assert(builder != null, "AddTables is called with null parameter!");

			Assembly asm = parent.GetType().Assembly;
			foreach (Type t in asm.GetTypes())
			{
				if (t.IsSubclassOf(typeof(AttributeTableBuilder)))
				{
					try
					{
                        AttributeTableBuilder atb = (AttributeTableBuilder)Activator.CreateInstance(t);
                        builder.AddTable(atb.CreateTable());
					}
					catch (Exception)
					{
                        //error loading design assembly
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the case sensitive resource name of the embedded XML file.
		/// </summary>
		protected string XmlResourceName { get; set; }

		/// <summary>
		/// Gets or sets the FullName of the corresponding run time assembly.
		/// </summary>
		protected string AssemblyFullName { get; set; }

		/// <summary>
		/// Create description attribute from run time assembly xml file.
		/// </summary>
		/// <param name="builder">The assembly attribute table builder.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Design time dll should not fail.")]
		private void AddDescriptions(AttributeTableBuilder builder)
		{
			Debug.Assert(builder != null, "AddDescriptions is called with null parameter!");

			if (string.IsNullOrEmpty(XmlResourceName) ||
				string.IsNullOrEmpty(AssemblyFullName))
			{
				return;
			}
			XDocument xdoc = null;
			try
			{
				xdoc = XDocument.Load(new StreamReader(
					Assembly.GetExecutingAssembly().GetManifestResourceStream(XmlResourceName)));
			}
			catch { return; }
			if (xdoc == null)
			{
				return;
			}

			foreach (XElement member in xdoc.Descendants("member"))
			{
				try
				{
					string name = (string)member.Attribute("name");
					if (name == null) 
						continue;
					bool isType = name.StartsWith("T:", StringComparison.OrdinalIgnoreCase);
					if (isType ||
						name.StartsWith("P:", StringComparison.OrdinalIgnoreCase))
					{
						int lastDot = name.Length;
						string typeName;
						if (isType)
						{
							typeName = name.Substring(2); // skip leading "T:"
						}
						else
						{
							lastDot = name.LastIndexOf('.');
							typeName = name.Substring(2, lastDot - 2);
						}
						typeName += AssemblyFullName;
						
						Type t = Type.GetType(typeName);
						if (t != null && t.IsPublic && t.IsClass &&
                            t.IsSubclassOf(Microsoft.Toolkit.Uwp.UI.Controls.Design.Types.PlatformTypes.DependencyObjectType))
						{
							string desc = ParseDescription(member);
                            if (desc == null)
                                continue;

							desc = desc.Trim();
							desc = string.Join(" ", desc.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
							if (isType)
							{
								bool isBrowsable = true;
								try
								{
									isBrowsable = IsBrowsable(t);
								}
								catch { isBrowsable = false; }
								if (isBrowsable)
									builder.AddCallback(t, b => b.AddCustomAttributes(new DescriptionAttribute(desc)));
								else //Hide from intellisense
								{
									builder.AddCallback(t, b => b.AddCustomAttributes(
										new BrowsableAttribute(false),
										new Microsoft.Windows.Design.ToolboxBrowsableAttribute(false),
										new ToolboxItemAttribute(false)));
								}
							}
							else
							{
								string propName = name.Substring(lastDot + 1);
								PropertyInfo pi = t.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
								if (pi != null)
								{
									bool isBrowsable = true;
									try
									{
										isBrowsable = IsBrowsable(pi);
									}
									catch { isBrowsable = false; }
									if(isBrowsable)
										builder.AddCallback(t, b => b.AddCustomAttributes(propName, new DescriptionAttribute(desc)));
									else //Hide from intellisense
										builder.AddCallback(t, b => b.AddCustomAttributes(new BrowsableAttribute(false)));
								}
							}
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}
		private static bool IsBrowsable(Type t)
		{
			var attrs = t.GetCustomAttributes(Microsoft.Toolkit.Uwp.UI.Controls.Design.Types.PlatformTypes.EditorBrowsableAttributeType, false);
			foreach (var attr in attrs)
			{
                return Microsoft.Toolkit.Uwp.UI.Controls.Design.Types.PlatformTypes.IsBrowsable(attr);
			}
			return true;
		}

		private static bool IsBrowsable(System.Reflection.PropertyInfo pi)
		{
            var attrs = pi.GetCustomAttributes(Microsoft.Toolkit.Uwp.UI.Controls.Design.Types.PlatformTypes.EditorBrowsableAttributeType, false);
			foreach (var attr in attrs)
			{
                return Microsoft.Toolkit.Uwp.UI.Controls.Design.Types.PlatformTypes.IsBrowsable(attr);
			}
			return true;
		}

		/// <summary>
		/// Create description string from xml doc summary tag.
		/// </summary>
		/// <param name="member">A single node of the xml doc.</param>
		/// <returns>Description string.</returns>
		private static string ParseDescription(XElement member)
		{
			string desc = null;
			XElement memberDesc = member.Descendants("summary").FirstOrDefault();

			if (memberDesc != null)
			{
				IEnumerable<XNode> nodes = memberDesc.DescendantNodes();

				if (nodes != null)
				{
					foreach (XNode node in nodes)
					{
						if (node.NodeType == System.Xml.XmlNodeType.Text)
						{
							desc += node.ToString();
						}
						else
						{
							string s = node.ToString();
							int i = s.LastIndexOf('.');
							int j = s.LastIndexOf('"');

							if ((i != -1 || j != -1) && j - i - 1 > 0)
							{
								try
								{
									desc += s.Substring(i + 1, j - i - 1);
								}
								catch { }
							}
						}
					}
				}
			}
			return desc;
		}

		/// <summary>
		/// Provide a place to add custom attributes without creating a AttributeTableBuilder subclass.
		/// </summary>
		/// <param name="builder">The assembly attribute table builder.</param>
		protected virtual void AddAttributes(AttributeTableBuilder builder)
		{
		}
	}
}