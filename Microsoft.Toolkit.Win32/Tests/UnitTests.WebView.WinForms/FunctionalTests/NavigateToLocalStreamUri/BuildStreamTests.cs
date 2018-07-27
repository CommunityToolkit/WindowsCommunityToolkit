// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;
using System;
using System.Reflection;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.NavigateToLocalStreamUri
{
    public abstract class BuildStreamContextSpecification : HostFormWebViewContextSpecification
    {
        public Uri Actual { get; set; }
        public Uri Expected { get; set; }
        public string RelativePath { get; set; }

        protected override void When()
        {
            var type = WebView.GetType();
            var hostPropertyInfo = type.GetProperty(
                "Host",
                BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.NonPublic);

            Assert.IsNotNull(hostPropertyInfo);

            var host = hostPropertyInfo.GetValue(WebView);

            Assert.IsNotNull(host);

            var streamMethodInfo = host
                                    .GetType()
                                    .GetMethod("BuildStream", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.IsNotNull(streamMethodInfo);

            try
            {
                Actual = (Uri) streamMethodInfo.Invoke(host, new[] {"Test", RelativePath});
            }
            catch (TargetInvocationException tie)
            {
                if (tie.InnerException != null)
                {
                    throw tie.InnerException;
                }

                throw;
            }
        }

        [TestMethod]
        public virtual void GeneratedStreamUriIsExpectedValue()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    public class Given_a_relative_path_referencing_two_ancestors : BuildStreamContextSpecification
    {
        protected override void Given()
        {
            base.Given();
            Expected = new Uri("ms-local-stream://microsoft.win32webviewhost_cw5n1h2txyewy_54657374/foo.htm");
            RelativePath = @"..\..\foo.htm";
        }
    }

    [TestClass]
    public class Given_a_relative_path_referencing_sibling : BuildStreamContextSpecification
    {
        protected override void Given()
        {
            base.Given();
            Expected = new Uri("ms-local-stream://microsoft.win32webviewhost_cw5n1h2txyewy_54657374/foo.htm");
            RelativePath = @"foo.htm";
        }
    }

    [TestClass]
    public class Given_a_relative_path_referencing_sibling_backslash : BuildStreamContextSpecification
    {
        protected override void Given()
        {
            base.Given();
            Expected = new Uri("ms-local-stream://microsoft.win32webviewhost_cw5n1h2txyewy_54657374//foo.htm");
            RelativePath = @"\foo.htm";
        }
    }

    [TestClass]
    public class Given_a_relative_path_referencing_sibling_dotbackslash : BuildStreamContextSpecification
    {
        protected override void Given()
        {
            base.Given();
            Expected = new Uri("ms-local-stream://microsoft.win32webviewhost_cw5n1h2txyewy_54657374/foo.htm");
            RelativePath = @".\foo.htm";
        }
    }

    [TestClass]
    public class Given_a_relative_path_referencing_sibling_frontslash : BuildStreamContextSpecification
    {
        protected override void Given()
        {
            base.Given();
            Expected = new Uri("ms-local-stream://microsoft.win32webviewhost_cw5n1h2txyewy_54657374/foo.htm");
            RelativePath = @"/foo.htm";
        }
    }

    [TestClass]
    public class Given_a_relative_path_referencing_sibling_dotfrontslash : BuildStreamContextSpecification
    {
        protected override void Given()
        {
            base.Given();
            Expected = new Uri("ms-local-stream://microsoft.win32webviewhost_cw5n1h2txyewy_54657374/foo.htm");
            RelativePath = @"./foo.htm";
        }
    }

    [TestClass]
    public class Given_a_relative_path_referencing_descendant : BuildStreamContextSpecification
    {
        protected override void Given()
        {
            base.Given();
            Expected = new Uri("ms-local-stream://microsoft.win32webviewhost_cw5n1h2txyewy_54657374/bar/foo.htm");
            RelativePath = @"bar/foo.htm";
        }
    }

    [TestClass]
    public class Given_a_relative_path_referencing_descendant_backslash : BuildStreamContextSpecification
    {
        protected override void Given()
        {
            base.Given();
            Expected = new Uri("ms-local-stream://microsoft.win32webviewhost_cw5n1h2txyewy_54657374//bar/foo.htm");
            RelativePath = @"\bar\foo.htm";
        }
    }

    [TestClass]
    public class Given_a_relative_path_referencing_descendant_dotbackslash : BuildStreamContextSpecification
    {
        protected override void Given()
        {
            base.Given();
            Expected = new Uri("ms-local-stream://microsoft.win32webviewhost_cw5n1h2txyewy_54657374/bar/foo.htm");
            RelativePath = @".\bar\foo.htm";
        }
    }

    [TestClass]
    public class Given_a_relative_path_referencing_descendant_frontslash : BuildStreamContextSpecification
    {
        protected override void Given()
        {
            base.Given();
            Expected = new Uri("ms-local-stream://microsoft.win32webviewhost_cw5n1h2txyewy_54657374/bar/foo.htm");
            RelativePath = @"/bar/foo.htm";
        }
    }

    [TestClass]
    public class Given_a_relative_path_referencing_descendant_dotfrontslash : BuildStreamContextSpecification
    {
        protected override void Given()
        {
            base.Given();
            Expected = new Uri("ms-local-stream://microsoft.win32webviewhost_cw5n1h2txyewy_54657374/bar/foo.htm");
            RelativePath = @"./bar/foo.htm";
        }
    }

    [TestClass]
    public class Given_an_absolute_path_referencing_two_ancestors : BuildStreamContextSpecification
    {
        protected override void Given()
        {
            base.Given();

            RelativePath = @"C:\foo.htm";
        }

        protected override void When()
        {
            // Intentionally blank
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public override void GeneratedStreamUriIsExpectedValue()
        {
            // The error actually occurs in the BuildStream(String, String) method
            base.When();
        }
    }

    [TestClass]
    public class Given_an_UNC_path_referencing_two_ancestors : BuildStreamContextSpecification
    {
        protected override void Given()
        {
            base.Given();

            RelativePath = @"\\machine\share\foo.htm";
        }

        protected override void When()
        {
            // Intentionally blank
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public override void GeneratedStreamUriIsExpectedValue()
        {
            // The error actually occurs in the BuildStream(String, String) method
            base.When();
        }
    }
}
