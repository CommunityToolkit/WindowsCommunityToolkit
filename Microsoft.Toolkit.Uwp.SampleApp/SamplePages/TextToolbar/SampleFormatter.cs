// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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