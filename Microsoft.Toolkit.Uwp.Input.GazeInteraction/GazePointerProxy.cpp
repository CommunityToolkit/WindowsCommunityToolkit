//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#include "pch.h"
#include "assert.h"
#include <winrt/Microsoft.UI.Xaml.Interop.h>
#include "GazePointerProxy.h"
#include "GazePointerProxy.g.cpp"
#include "GazePointer.h"

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    /// <summary>
    /// The IsLoaded heuristic for testing whether a FrameworkElement is in the visual tree.
    /// </summary>
    static bool IsLoadedHeuristic(FrameworkElement element)
    {
        bool isLoaded{ false };

        // element.Loaded has already happened if it is in the visual tree...
        auto parent = VisualTreeHelper::GetParent(element);
        if (parent != nullptr)
        {
            isLoaded = true;
        }
        // ...or...
        else
        {
            // ...if the element is a dynamically created Popup that has been opened.
            if (auto popup{ element.try_as<Popup>() })
            {
                isLoaded = popup.IsOpen();
            }
        }

        return isLoaded;
    }

    DependencyProperty GazePointerProxy::m_gazePointerProxyProperty = DependencyProperty::RegisterAttached(L"_GazePointerProxy", xaml_typename<GazePointerProxy>(), xaml_typename<GazePointerProxy>(), PropertyMetadata(nullptr));

    DependencyProperty GazePointerProxy::GazePointerProxyProperty()
    {
        return m_gazePointerProxyProperty;
    }

    GazePointerProxy::GazePointerProxy(Microsoft::UI::Xaml::FrameworkElement const& element)
    {
        static int lastId = 0;
        lastId++;
        _uniqueId = lastId;

        _isLoaded = IsLoadedHeuristic(element);

        // Start watching for the element to enter and leave the visual tree.
        element.Loaded({ this, &GazePointerProxy::OnLoaded });
        element.Unloaded({ this,&GazePointerProxy::OnUnloaded });
    }

    void GazePointerProxy::SetInteraction(Microsoft::UI::Xaml::FrameworkElement const& element, Microsoft::Toolkit::Uwp::Input::GazeInteraction::Interaction const& value)
    {
        // Get or create a GazePointerProxy for element.
        winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazePointerProxy proxy{ nullptr };
        if (!(element.GetValue(GazePointerProxyProperty()).try_as<winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazePointerProxy>(proxy)))
        {
            proxy = winrt::make<GazePointerProxy>(element);
            element.SetValue(GazePointerProxyProperty(), proxy);
        }

        // Set the proxy's _isEnabled value.
        proxy.SetIsEnabled(element, value == Interaction::Enabled);
    }

    void GazePointerProxy::SetIsEnabled(Windows::Foundation::IInspectable const& sender, bool value)
    {
        // If we have a new value...
        if (_isEnabled != value)
        {
            // ...record the new value.
            _isEnabled = value;

            // If we are in the visual tree...
            if (_isLoaded)
            {
                // ...if we're being enabled...
                if (value)
                {
                    // ...count the element in...
                    GazePointer::Instance().AddRoot(_uniqueId);
                }
                else
                {
                    // ...otherwise count the element out.
                    GazePointer::Instance().RemoveRoot(_uniqueId);
                }
            }
        }
    }

    void GazePointerProxy::OnLoaded(Windows::Foundation::IInspectable const& sender, Microsoft::UI::Xaml::RoutedEventArgs const& e)
    {
        //assert(!IsLoadedHeuristic(sender.try_as<FrameworkElement>()));

        if (!_isLoaded)
        {
            // Record that we are now loaded.
            _isLoaded = true;

            // If we were previously enabled...
            if (_isEnabled)
            {
                // ...we can now be counted as actively enabled.
                GazePointer::Instance().AddRoot(_uniqueId);
            }
        }
        else
        {
            Debug::WriteLine(L"Unexpected Load");
        }
    }

    void GazePointerProxy::OnUnloaded(Windows::Foundation::IInspectable const& sender, Microsoft::UI::Xaml::RoutedEventArgs const& e)
    {
        //assert(!IsLoadedHeuristic(sender.try_as<FrameworkElement>()));

        if (_isLoaded)
        {
            // Record that we have left the visual tree.
            _isLoaded = false;

            // If we are set as enabled...
            if (_isEnabled)
            {
                // ...we no longer count as being actively enabled (because we have fallen out the visual tree).
                GazePointer::Instance().RemoveRoot(_uniqueId);
            }
        }
        else
        {
            Debug::WriteLine(L"Unexpected unload");
        }
    }
}