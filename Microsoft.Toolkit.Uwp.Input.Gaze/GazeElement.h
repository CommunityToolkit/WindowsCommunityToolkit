//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#pragma warning(disable:4453)

#include "IGazeFilter.h"
#include "GazeCursor.h"
#include "GazePointerEvent.h"
#include "GazeInvokedRoutedEventArgs.h"

using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Devices::Enumeration;
using namespace Windows::Devices::HumanInterfaceDevice;
using namespace Windows::UI::Core;
using namespace Windows::Devices::Input::Preview;

namespace Shapes = Windows::UI::Xaml::Shapes;

BEGIN_NAMESPACE_GAZE_INPUT

public ref class GazeElement sealed : public DependencyObject
{
private:
    static DependencyProperty^ const s_hasAttentionProperty;
    static DependencyProperty^ const s_invokeProgressProperty;
public:
    static property DependencyProperty^ HasAttentionProperty { DependencyProperty^ get() { return s_hasAttentionProperty; } }
    static property DependencyProperty^ InvokeProgressProperty { DependencyProperty^ get() { return s_invokeProgressProperty; } }

    property bool HasAttention { bool get() { return safe_cast<bool>(GetValue(s_hasAttentionProperty)); } void set(bool value) { SetValue(s_hasAttentionProperty, value); } }
    property double InvokeProgress { double get() { return safe_cast<double>(GetValue(s_invokeProgressProperty)); } void set(double value) { SetValue(s_invokeProgressProperty, value); } }

    event GazePointerEvent^ GazePointerEvent;
    event EventHandler<GazeInvokedRoutedEventArgs^>^ Invoked;

    void RaiseGazePointerEvent(GazePointer^ sender, GazePointerEventArgs^ args) { GazePointerEvent(sender, args); }

    void RaiseInvoked(Object^ sender, GazeInvokedRoutedEventArgs^ args)
    {
        Invoked(sender, args);
    }
};

END_NAMESPACE_GAZE_INPUT