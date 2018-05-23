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
