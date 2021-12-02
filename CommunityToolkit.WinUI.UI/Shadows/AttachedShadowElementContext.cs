// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Hosting;

namespace CommunityToolkit.WinUI.UI
{
    /// <summary>
    /// Class which maintains the context of a <see cref="DropShadow"/> for a particular <see cref="UIElement"/> linked to the definition of that shadow provided by the <see cref="AttachedShadowBase"/> implementation being used.
    /// </summary>
    public sealed class AttachedShadowElementContext
    {
        private bool _isConnected;

        private Dictionary<string, object> _resources;

        internal long? VisibilityToken { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not this <see cref="AttachedShadowElementContext"/> has been initialized.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets the <see cref="AttachedShadowBase"/> that contains this <see cref="AttachedShadowElementContext"/>.
        /// </summary>
        public AttachedShadowBase Parent { get; private set; }

        /// <summary>
        /// Gets the <see cref="FrameworkElement"/> this instance is attached to
        /// </summary>
        public FrameworkElement Element { get; private set; }

        /// <summary>
        /// Gets the <see cref="Visual"/> for the <see cref="FrameworkElement"/> this instance is attached to.
        /// </summary>
        public Visual ElementVisual { get; private set; }

        /// <summary>
        /// Gets the <see cref="Windows.UI.Composition.Compositor"/> for this instance.
        /// </summary>
        public Compositor Compositor { get; private set; }

        /// <summary>
        /// Gets the <see cref="SpriteVisual"/> that contains the <see cref="DropShadow">shadow</see> for this instance
        /// </summary>
        public SpriteVisual SpriteVisual { get; private set; }

        /// <summary>
        /// Gets the <see cref="DropShadow"/> that is rendered on this instance's <see cref="Element"/>
        /// </summary>
        public DropShadow Shadow { get; private set; }

        /// <summary>
        /// Connects a <see cref="FrameworkElement"/> to its parent <see cref="AttachedShadowBase"/> definition.
        /// </summary>
        /// <param name="parent">The <see cref="AttachedShadowBase"/> that is using this context.</param>
        /// <param name="element">The <see cref="FrameworkElement"/> that a shadow is being attached to.</param>
        internal void ConnectToElement(AttachedShadowBase parent, FrameworkElement element)
        {
            if (_isConnected)
            {
                throw new InvalidOperationException("This AttachedShadowElementContext has already been connected to an element");
            }

            _isConnected = true;
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Element = element ?? throw new ArgumentNullException(nameof(element));
            Element.Loaded += OnElementLoaded;
            Element.Unloaded += OnElementUnloaded;
            Initialize();
        }

        internal void DisconnectFromElement()
        {
            if (!_isConnected)
            {
                return;
            }

            Uninitialize();

            Element.Loaded -= OnElementLoaded;
            Element.Unloaded -= OnElementUnloaded;
            Element = null;

            Parent = null;

            _isConnected = false;
        }

        /// <summary>
        /// Force early creation of this instance's resources, otherwise they will be created automatically when <see cref="Element"/> is loaded.
        /// </summary>
        public void CreateResources() => Initialize(true);

        private void Initialize(bool forceIfNotLoaded = false)
        {
            if (IsInitialized || !_isConnected || (!Element.IsLoaded && !forceIfNotLoaded))
            {
                return;
            }

            IsInitialized = true;

            ElementVisual = ElementCompositionPreview.GetElementVisual(Element);
            Compositor = ElementVisual.Compositor;

            Shadow = Compositor.CreateDropShadow();

            SpriteVisual = Compositor.CreateSpriteVisual();
            SpriteVisual.RelativeSizeAdjustment = Vector2.One;
            SpriteVisual.Shadow = Shadow;

            if (Parent.SupportsOnSizeChangedEvent)
            {
                Element.SizeChanged += OnElementSizeChanged;
            }

            Parent?.OnElementContextInitialized(this);
        }

        private void Uninitialize()
        {
            if (!IsInitialized)
            {
                return;
            }

            IsInitialized = false;

            Parent.OnElementContextUninitialized(this);

            SpriteVisual.Shadow = null;
            SpriteVisual.Dispose();

            Shadow.Dispose();

            ElementCompositionPreview.SetElementChildVisual(Element, null);

            Element.SizeChanged -= OnElementSizeChanged;

            SpriteVisual = null;
            Shadow = null;
            ElementVisual = null;
        }

        private void OnElementUnloaded(object sender, RoutedEventArgs e)
        {
            Uninitialize();
        }

        private void OnElementLoaded(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        private void OnElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Parent?.OnSizeChanged(this, e.NewSize, e.PreviousSize);
        }

        /// <summary>
        /// Adds a resource to this instance's resource dictionary with the specified key
        /// </summary>
        /// <typeparam name="T">The type of the resource being added.</typeparam>
        /// <param name="key">Key to use to lookup the resource later.</param>
        /// <param name="resource">Object to store within the resource dictionary.</param>
        /// <returns>The added resource</returns>
        public T AddResource<T>(string key, T resource)
        {
            _resources = _resources ?? new Dictionary<string, object>();
            if (_resources.ContainsKey(key))
            {
                _resources[key] = resource;
            }
            else
            {
                _resources.Add(key, resource);
            }

            return resource;
        }

        /// <summary>
        /// Retrieves a resource with the specified key and type if it exists
        /// </summary>
        /// <typeparam name="T">The type of the resource being retrieved.</typeparam>
        /// <param name="key">Key to use to lookup the resource.</param>
        /// <param name="resource">Object to retrieved from the resource dictionary or default value.</param>
        /// <returns>True if the resource exists, false otherwise</returns>
        public bool TryGetResource<T>(string key, out T resource)
        {
            if (_resources != null && _resources.TryGetValue(key, out var objResource) && objResource is T tResource)
            {
                resource = tResource;
                return true;
            }

            resource = default;
            return false;
        }

        /// <summary>
        /// Retries a resource with the specified key and type
        /// </summary>
        /// <typeparam name="T">The type of the resource being retrieved.</typeparam>
        /// <param name="key">Key to use to lookup the resource.</param>
        /// <returns>The resource if available, otherwise default value.</returns>
        public T GetResource<T>(string key)
        {
            if (TryGetResource(key, out T resource))
            {
                return resource;
            }

            return default;
        }

        /// <summary>
        /// Removes an existing resource with the specified key and type
        /// </summary>
        /// <typeparam name="T">The type of the resource being removed.</typeparam>
        /// <param name="key">Key to use to lookup the resource.</param>
        /// <returns>The resource that was removed, if any</returns>
        public T RemoveResource<T>(string key)
        {
            if (_resources.TryGetValue(key, out var objResource))
            {
                _resources.Remove(key);
                if (objResource is T resource)
                {
                    return resource;
                }
            }

            return default;
        }

        /// <summary>
        /// Removes an existing resource with the specified key and type, and <see cref="IDisposable.Dispose">disposes</see> it
        /// </summary>
        /// <typeparam name="T">The type of the resource being removed.</typeparam>
        /// <param name="key">Key to use to lookup the resource.</param>
        /// <returns>The resource that was removed, if any</returns>
        public T RemoveAndDisposeResource<T>(string key)
            where T : IDisposable
        {
            if (_resources.TryGetValue(key, out var objResource))
            {
                _resources.Remove(key);
                if (objResource is T resource)
                {
                    resource.Dispose();
                    return resource;
                }
            }

            return default;
        }

        /// <summary>
        /// Adds a resource to this instance's collection with the specified key
        /// </summary>
        /// <typeparam name="T">The type of the resource being added.</typeparam>
        /// <returns>The resource that was added</returns>
        internal T AddResource<T>(TypedResourceKey<T> key, T resource) => AddResource(key.Key, resource);

        /// <summary>
        /// Retrieves a resource with the specified key and type if it exists
        /// </summary>
        /// <typeparam name="T">The type of the resource being retrieved.</typeparam>
        /// <returns>True if the resource exists, false otherwise</returns>
        internal bool TryGetResource<T>(TypedResourceKey<T> key, out T resource) => TryGetResource(key.Key, out resource);

        /// <summary>
        /// Retries a resource with the specified key and type
        /// </summary>
        /// <typeparam name="T">The type of the resource being retrieved.</typeparam>
        /// <returns>The resource if it exists or a default value.</returns>
        internal T GetResource<T>(TypedResourceKey<T> key) => GetResource<T>(key.Key);

        /// <summary>
        /// Removes an existing resource with the specified key and type
        /// </summary>
        /// <typeparam name="T">The type of the resource being removed.</typeparam>
        /// <returns>The resource that was removed, if any</returns>
        internal T RemoveResource<T>(TypedResourceKey<T> key) => RemoveResource<T>(key.Key);

        /// <summary>
        /// Removes an existing resource with the specified key and type, and <see cref="IDisposable.Dispose">disposes</see> it
        /// </summary>
        /// <typeparam name="T">The type of the resource being removed.</typeparam>
        /// <returns>The resource that was removed, if any</returns>
        internal T RemoveAndDisposeResource<T>(TypedResourceKey<T> key)
            where T : IDisposable => RemoveAndDisposeResource<T>(key.Key);

        /// <summary>
        /// Disposes of any resources that implement <see cref="IDisposable"/> and then clears all resources
        /// </summary>
        public void ClearAndDisposeResources()
        {
            if (_resources != null)
            {
                foreach (var kvp in _resources)
                {
                    (kvp.Value as IDisposable)?.Dispose();
                }

                _resources.Clear();
            }
        }
    }
}