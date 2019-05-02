// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Controls.Graph;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the opacity behavior.
    /// </summary>
    public sealed partial class PeoplePickerPage : Page, IXamlRenderListener
    {
        private PeoplePicker peoplePickerControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeoplePickerPage"/> class.
        /// </summary>
        public PeoplePickerPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            peoplePickerControl = control.FindName("PeoplePicker") as PeoplePicker;
            if (peoplePickerControl != null)
            {
                peoplePickerControl.SelectionChanged += PeopleSelectionChanged;
            }
        }

        private async void PeopleSelectionChanged(object sender, PeopleSelectionChangedEventArgs e)
        {
            if (e.Selections != null)
            {
                await new MessageDialog($"Selected Person Counter {e.Selections.Count}", "Selection Changed").ShowAsync();
            }
        }
    }
}