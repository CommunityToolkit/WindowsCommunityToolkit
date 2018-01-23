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

namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// Specify what person this toast is related to. For more info, see the My People documentation. New in Fall Creators Update.
    /// </summary>
    public sealed class ToastPeople
    {
        /// <summary>
        /// Gets or sets a remote identifier that corresponds with the RemoteId you set on a Contact you created
        /// with the ContactStore APIs. For more info, see the My People documentation.
        /// </summary>
        public string RemoteId { get; set; }

        /// <summary>
        /// Gets or sets an email address that corresponds with a contact in the local Windows ContactStore. Note
        /// that if <see cref="ToastPeople.RemoteId"/> is specified, this property will be ignored. For more info,
        /// see the My People documentation.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets a phone number that corresponds with a contact in the local Windows ContactStore. Note
        /// that if <see cref="ToastPeople.EmailAddress"/> is specified, this property will be ignored.
        /// For more info, see the My People documentation.
        /// </summary>
        public string PhoneNumber { get; set; }

        internal void PopulateToastElement(Element_Toast toast)
        {
            string hintPeople = null;

            if (RemoteId != null)
            {
                hintPeople = "remoteid:" + RemoteId;
            }
            else if (EmailAddress != null)
            {
                hintPeople = "mailto:" + EmailAddress;
            }
            else if (PhoneNumber != null)
            {
                hintPeople = "tel:" + PhoneNumber;
            }
            else
            {
                return;
            }

            toast.HintPeople = hintPeople;
        }
    }
}
