// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Graph;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Arguments relating to the people selected event of <see cref="PeoplePicker"/> control
    /// </summary>
    public class PeopleSelectionChangedEventArgs
    {
        /// <summary>
        /// Gets selected file
        /// </summary>
        public ObservableCollection<Person> Selections { get; private set; }

        internal PeopleSelectionChangedEventArgs(ObservableCollection<Person> selections)
        {
            Selections = selections;
        }
    }
}
