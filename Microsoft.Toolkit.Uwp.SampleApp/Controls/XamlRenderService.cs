﻿// ******************************************************************
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
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.SampleApp.Controls
{
    /// <summary>
    /// Class to assist in parsing a Xaml string and returning an UIElement.
    ///
    /// Wrapper around XamlReader.Load* with extra pre/post processing to support more features like loading images from an external source.
    ///
    /// References:
    ///     https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.markup.xamlreader
    ///     https://docs.microsoft.com/en-us/windows/uwp/xaml-platform/xaml-namespaces-and-namespace-mapping
    ///     https://docs.microsoft.com/en-us/windows/uwp/data-binding/data-binding-in-depth
    ///     https://blogs.msdn.microsoft.com/mcsuksoldev/2010/08/27/designdata-mvvm-support-in-blend-vs2010-and-wpfsilverlight/
    /// </summary>
    public class XamlRenderService
    {
        /// <summary>
        /// Gets the last error(s) from call to Render.
        /// </summary>
        public IList<XamlExceptionRange> Errors { get; private set; } = new List<XamlExceptionRange>();

        /// <summary>
        /// Gets or sets the explicit DataContext used on the root UIElement.
        /// </summary>
        public object DataContext { get; set; }

        public UIElement Render(string content)
        {
            Errors.Clear();

            if (string.IsNullOrWhiteSpace(content))
            {
                // Nothing to do!
                return null;
            }

            // TODO: add flag about using pre-parsing or not.
            // Pre-parse
            if (!content.Contains("xmlns"))
            {
                // Find the end of the first tag
                var oti = content.IndexOf(">");
                if (oti != -1)
                {
                    content = content.Substring(0, oti) + @" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""" + content.Substring(oti);
                }
            }

            // Attempt Render
            UIElement element = null;
            try
            {
                var obj = XamlReader.LoadWithInitialTemplateValidation(content); // TODO: Add Flag to change which function to use.
                if (!(obj is UIElement))
                {
                    throw new NotSupportedException("Content must be a UIElement.");
                }

                element = obj as UIElement;
            }
            catch (Exception e)
            {
                // Highlight Error (we'll only get one at a time).
                string msg = e.Message;

                msg = msg.Replace("The text associated with this error code could not be found.", string.Empty).Trim();

                uint line = 1;
                uint column = 1;

                // No default namespace has been declared. [Line: 1 Position: 2]
                int il = msg.IndexOf("Line: ");
                if (il >= 0)
                {
                    line = uint.Parse(msg.Substring(il + 6, msg.IndexOf("P", il) - il - 7));
                }

                int pl = msg.IndexOf("Position: ");
                if (pl >= 0)
                {
                    column = uint.Parse(msg.Substring(pl + 9, msg.IndexOf("]", pl) - pl - 9));
                }

                // TODO: Should I just throw this nicely parsed message?
                Errors.Add(new XamlExceptionRange(msg, e, line, column, line, column + 8)); // TODO: Inspect Content at this position and go until space / EOL
            }

            // Set DataContext to root element or to provided DataContext (if it exists).
            // May get overwritten by d:DesignData loading later.
            if (element is FrameworkElement)
            {
                var fwe = element as FrameworkElement;
                fwe.DataContext = this.DataContext == null ? element : this.DataContext;
            }

            return element;
        }
    }
}
