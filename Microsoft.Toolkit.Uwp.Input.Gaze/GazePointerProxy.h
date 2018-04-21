#pragma once
#include "pch.h"

BEGIN_NAMESPACE_GAZE_INPUT

ref class GazePointerProxy sealed
{
public:

    static property DependencyProperty^ GazePointerProxyProperty { DependencyProperty^ get(); };

internal:

    static void SetGazeEnabled(FrameworkElement^ element, bool value);

private:

    GazePointerProxy(FrameworkElement^ element);

    property bool IsEnabled
    {
        bool get() { return _isEnabled; }
        void set(bool value);
    }

    void OnPageLoaded(Object^ sender, RoutedEventArgs^ args);

    void OnPageUnloaded(Object^ sender, RoutedEventArgs^ args);

private:

    FrameworkElement^ const _element;
    EventRegistrationToken _loadedToken;
    EventRegistrationToken _unloadedToken;
    bool _isLoaded;
    bool _isEnabled;
};

END_NAMESPACE_GAZE_INPUT


