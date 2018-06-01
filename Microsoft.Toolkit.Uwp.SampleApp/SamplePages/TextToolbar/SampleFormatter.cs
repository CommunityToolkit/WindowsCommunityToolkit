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

using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons.Common;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages.TextToolbarSamples
{
    public class SampleFormatter : Formatter
    {
        public SampleFormatter(TextToolbar model)
            : base(model)
        {
            CommonButtons = new CommonButtons(model);
        }

        public override ButtonMap DefaultButtons
        {
            get
            {
                var bold = CommonButtons.Bold;
                bold.Activation = item => Selected.Text = "BOLD!!!";

                return new ButtonMap
                {
                    bold
                };
            }
        }

        private CommonButtons CommonButtons { get; }
    }
}