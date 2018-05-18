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

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UnitTests.Extensions.Helpers
{
    [Bindable]
    public class ObjectWithNullableBoolProperty : DependencyObject
    {
        public bool? NullableBool
        {
            get { return (bool?)GetValue(NullableBoolProperty); }
            set { SetValue(NullableBoolProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NullableBool.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NullableBoolProperty =
            DependencyProperty.Register(nameof(NullableBool), typeof(bool?), typeof(ObjectWithNullableBoolProperty), new PropertyMetadata(null));
    }
}
