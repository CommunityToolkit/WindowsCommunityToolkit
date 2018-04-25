#pragma once
#include "pch.h"

#include "GazeEnablement.h"

BEGIN_NAMESPACE_GAZE_INPUT

private ref class GazePointerProxy sealed
{
public:

    static property DependencyProperty^ GazePointerProxyProperty { DependencyProperty^ get(); };

internal:

    static void SetGazeEnabled(FrameworkElement^ element, GazeEnablement value);

private:

    GazePointerProxy(FrameworkElement^ element);

    property GazeEnablement IsEnabled
    {
        GazeEnablement get() { return _isEnabled; }
        void set(GazeEnablement value);
    }

    void OnPageLoaded(Object^ sender, RoutedEventArgs^ args);

    void OnPageUnloaded(Object^ sender, RoutedEventArgs^ args);

private:

    FrameworkElement^ const _element;
    EventRegistrationToken _loadedToken;
    EventRegistrationToken _unloadedToken;
    bool _isLoaded;
    GazeEnablement _isEnabled;
};

END_NAMESPACE_GAZE_INPUT


