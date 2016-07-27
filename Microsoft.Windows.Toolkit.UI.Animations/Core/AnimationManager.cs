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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Windows.Toolkit.UI.Animations
{
    public class AnimationManager
    {
        public Visual Visual { get; private set; }
        public UIElement Element { get; private set; }

        private Dictionary<string, CompositionAnimation> _animations;

        private Compositor _compositor;
        private CompositionScopedBatch _batch;

        private System.Threading.ManualResetEvent _manualResetEvent;
        
        public AnimationManager(UIElement element)
        {
            if (element == null)
            {
                throw new NullReferenceException("element must not be null");
            }

            var visual = ElementCompositionPreview.GetElementVisual(element);

            if (visual == null)
            {
                throw new NullReferenceException("visual must not be null");
            }

            Visual = visual;
            if (Visual.Compositor == null)
            {
                throw new NullReferenceException("Visual must have a compositor");
            }

            Element = element;
            _compositor = Visual.Compositor;
            _animations = new Dictionary<string, CompositionAnimation>();
            _manualResetEvent = new System.Threading.ManualResetEvent(false);
        }
        
        public void Start()
        {
            StartAsync();
        }

        public Task StartAsync()
        {
            if (_batch != null)
            {
                _batch.End();
                _batch.Completed -= _batch_Completed;
            }

            _batch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            _batch.Completed += _batch_Completed;

            foreach (var anim in _animations)
            {
                Visual.StartAnimation(anim.Key, anim.Value);
            }

            Task t = Task.Run(() =>
            {
                _manualResetEvent.Reset();
                _manualResetEvent.WaitOne();
            });

            _batch.End();

            return t;
        }

        private void _batch_Completed(object sender, CompositionBatchCompletedEventArgs args)
        {
            _manualResetEvent.Set();
            AnimationsCompleted?.Invoke(this, new EventArgs());
        }

        public event EventHandler AnimationsCompleted;

        public void AddAnimation(string propertyName, CompositionAnimation animation)
        {
            _animations[propertyName] = animation;
        }

        public void RemoveAnimation(string propertyName)
        {
            if (_animations.ContainsKey(propertyName))
            {
                _animations.Remove(propertyName);
            }
        }
    }
}
