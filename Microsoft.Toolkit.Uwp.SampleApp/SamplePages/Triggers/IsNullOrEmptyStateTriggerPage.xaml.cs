// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IsNullOrEmptyStateTriggerPage : Page, IXamlRenderListener
    {
        private Button _addButton;
        private Button _removeButton;
        private ListBox _listBox;
        private Button _unselectButton;
        private ListView _selectListView;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsNullOrEmptyStateTriggerPage"/> class.
        /// </summary>
        public IsNullOrEmptyStateTriggerPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            if (_addButton != null)
            {
                _addButton.Click -= this.AddButton_Click;
            }

            if (control.FindDescendant("AddButton") is Button btn)
            {
                _addButton = btn;

                _addButton.Click += this.AddButton_Click;
            }

            if (_removeButton != null)
            {
                _removeButton.Click -= this.RemoveButton_Click;
            }

            if (control.FindDescendant("RemoveButton") is Button btn2)
            {
                _removeButton = btn2;

                _removeButton.Click += this.RemoveButton_Click;
            }

            _listBox = control.FindDescendant("OurList") as ListBox;

            _selectListView = control.FindDescendant("SelectList") as ListView;
            
            // Hack to programmatically set the SelectedItem to *any* initial value other than null,
            //      to prime the trigger to self-reflect when control initially loads, and apply appropriate trigger logic
            _selectListView.SelectedItem = string.Empty;

            if (control.FindDescendant("RemoveSelection") is Button btn3)
            {
                _unselectButton = btn3;

                _unselectButton.Click += this.UnselectButton_Click;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_listBox != null)
            {
                _listBox.Items.Add("Item");
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_listBox != null && _listBox.Items.Count > 0)
            {
                _listBox.Items.RemoveAt(0);
            }
        }

        private void UnselectButton_Click(object sender, RoutedEventArgs e)
        {
            if(_selectListView != null && _selectListView.SelectedItem != null)
            {
                _selectListView.SelectedItem = null;
            }
        }
    }
}