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

using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Controls.Primitives;
using Windows.UI.Xaml.Automation.Peers;

namespace Microsoft.Toolkit.Uwp.Automation.Peers
{
    /// <summary>
    /// AutomationPeer for the <see cref="DataGridRowsPresenter"/> class.
    /// </summary>
    public class DataGridRowsPresenterAutomationPeer : FrameworkElementAutomationPeer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridRowsPresenterAutomationPeer"/> class.
        /// </summary>
        /// <param name="owner">Owning DataGridRowsPresenter</param>
        public DataGridRowsPresenterAutomationPeer(DataGridRowsPresenter owner)
            : base(owner)
        {
        }

        private DataGridAutomationPeer GridPeer
        {
            get
            {
                if (this.OwningRowsPresenter.OwningGrid != null)
                {
                    return CreatePeerForElement(this.OwningRowsPresenter.OwningGrid) as DataGridAutomationPeer;
                }

                return null;
            }
        }

        private DataGridRowsPresenter OwningRowsPresenter
        {
            get
            {
                return Owner as DataGridRowsPresenter;
            }
        }

        /// <summary>
        /// Gets the control type for the element that is associated with the UI Automation peer.
        /// </summary>
        /// <returns>The control type.</returns>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Custom;
        }

        /// <summary>
        /// Gets the collection of elements that are represented in the UI Automation tree as immediate
        /// child elements of the automation peer.
        /// </summary>
        /// <returns>The children elements.</returns>
        protected override IList<AutomationPeer> GetChildrenCore()
        {
            if (this.OwningRowsPresenter.OwningGrid == null)
            {
                return new List<AutomationPeer>();
            }

            return this.GridPeer.GetChildPeers();
        }

        /// <summary>
        /// Called by GetClassName that gets a human readable name that, in addition to AutomationControlType,
        /// differentiates the control represented by this AutomationPeer.
        /// </summary>
        /// <returns>The string that contains the name.</returns>
        protected override string GetClassNameCore()
        {
            return Owner.GetType().Name;
        }

        /// <summary>
        /// Gets a value that specifies whether the element is a content element.
        /// </summary>
        /// <returns>True if the element is a content element; otherwise false</returns>
        protected override bool IsContentElementCore()
        {
            return false;
        }
    }
}
