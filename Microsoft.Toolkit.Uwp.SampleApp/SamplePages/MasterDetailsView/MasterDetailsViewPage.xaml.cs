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
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MasterDetailsViewPage : Page
    {
        public MasterDetailsViewPage()
        {
            Emails = new List<Email>
            {
                new Email { From = "Steve Johnson", Subject = "Lunch Tomorrow", Body = "Are you available for lunch tomorrow? A client would like to discuss a project with you." },
                new Email { From = "Becky Davidson", Subject = "Kids game", Body = "Don't forget the kids have their soccer game this Friday. We have to supply end of game snacks." },
                new Email { From = "OneDrive", Subject = "Check out your event recap", Body = "Your new album.\r\nYou uploaded some photos to yuor OneDrive and automatically created an album for you." },
                new Email { From = "Twitter", Subject = "Follow randomPerson, APersonYouMightKnow", Body = "Here are some people we think you might like to follow:\r\n.@randomPerson\r\nAPersonYouMightKnow" },
            };
            InitializeComponent();
        }

        private ICollection<Email> Emails { get; set; }
    }
}
