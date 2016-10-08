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

using System.Collections.ObjectModel;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// MicrosoftGraphUIExtensions Extension
    /// </summary>
    public static class MicrosoftGraphUIExtensions
    {
        /// <summary>
        /// Add a source collection of items to a destination collection
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="itemsSource">Source Collection</param>
        /// <param name="itemsDest">Destination Collection</param>
        public static void AddTo<T>(this ObservableCollection<T> itemsSource, ObservableCollection<T> itemsDest)
        {
            foreach (var item in itemsSource)
            {
                itemsDest.Add(item);
            }
        }
    }
}
