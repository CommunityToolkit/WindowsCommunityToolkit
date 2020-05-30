// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Microsoft.Toolkit.Uwp.Design.Common;
using Microsoft.Toolkit.Uwp.UI.Controls.Design;

#if VS_DESIGNER_PROCESS_ISOLATION
using Microsoft.Windows.Design.Metadata;
#else
using Microsoft.VisualStudio.DesignTools.Extensibility.Metadata;
#endif

[assembly: ProvideMetadata(typeof(MetadataRegistration))]

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design
{
    public class MetadataRegistration : MetadataRegistrationBase
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
#if VS_DESIGNER_PROCESS_ISOLATION
            Type t = typeof(GridSplitter);
            AssemblyName an = t.Assembly.GetName();
            AssemblyFullName = an.FullName;
            XmlResourceName = t.Namespace + ".Design." + an.Name + ".xml";
#else
            var AssemblyName = ControlTypes.RootNamespace;
            AssemblyFullName = $"{AssemblyName}, Version=6.1.0.0, Culture=neutral, PublicKeyToken=null";
            XmlResourceName = $"{AssemblyName}.Design.{AssemblyName}.xml";
#endif
        }
    }
}