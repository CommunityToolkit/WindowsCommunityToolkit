#pragma once

#include "Interaction.h"

BEGIN_NAMESPACE_GAZE_INPUT

private ref class GazePointerProxy sealed
{
public:

    static property DependencyProperty^ GazePointerProxyProperty { DependencyProperty^ get(); };

internal:

    static void SetGazeInteraction(FrameworkElement^ element, Interaction value);

private:

    GazePointerProxy(FrameworkElement^ element);

    property Interaction IsEnabled
    {
        Interaction get() { return _isEnabled; }
        void set(Interaction value);
    }

    void OnPageLoaded(Object^ sender, RoutedEventArgs^ args);

    void OnPageUnloaded(Object^ sender, RoutedEventArgs^ args);

private:

    FrameworkElement^ const _element;
    bool _isLoaded;
    Interaction _isEnabled;
};

END_NAMESPACE_GAZE_INPUT


