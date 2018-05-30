// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Controls.Graph;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the opacity behavior.
    /// </summary>
    public sealed partial class PlannerTaskListPage : Page
    {
        private PlannerTaskList _plannerTaskListControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlannerTaskListPage"/> class.
        /// </summary>
        public PlannerTaskListPage()
        {
            InitializeComponent();
        }
    }
}
