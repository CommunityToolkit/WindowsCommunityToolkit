// ------------------------------------------------------
// Copyright (C) Microsoft. All rights reserved.
// ------------------------------------------------------

using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI
{
    public interface IAttachedShadow
    {
        double BlurRadius { get; set; }
        double Opacity { get; set; }
        Vector3 Offset { get; set; }
        Color Color { get; set; }
        bool SupportsOnSizeChangedEvent { get; }
        void ConnectElement(FrameworkElement element);
        void DisconnectElement(FrameworkElement element);
        void OnElementContextInitialized(AttachedShadowElementContext context);
        void OnElementContextUninitialized(AttachedShadowElementContext context);
        void OnSizeChanged(AttachedShadowElementContext context, Size newSize, Size previousSize);
        AttachedShadowElementContext GetElementContext(FrameworkElement element);
    }
}