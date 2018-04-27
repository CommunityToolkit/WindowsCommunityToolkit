//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#pragma warning(disable:4453)

#include "IGazeFilter.h"
#include "GazeCursor.h"
#include "Interaction.h"

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

ref class GazeElement;
ref class GazePointer;

/// <summary>
/// TODO: harishsk
/// </summary>
public ref class GazeInput sealed
{
public:

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static property DependencyProperty^ InteractionProperty { DependencyProperty^ get(); }

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static property DependencyProperty^ IsCursorVisibleProperty { DependencyProperty^ get(); }

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static property DependencyProperty^ CursorRadiusProperty { DependencyProperty^ get(); }

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static property DependencyProperty^ GazeElementProperty { DependencyProperty^ get(); }

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static property DependencyProperty^ FixationDurationProperty { DependencyProperty^ get(); }

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static property DependencyProperty^ DwellDurationProperty { DependencyProperty^ get(); }

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static property DependencyProperty^ RepeatDelayDurationProperty { DependencyProperty^ get(); }

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static property DependencyProperty^ DwellRepeatDurationProperty { DependencyProperty^ get(); }

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static property DependencyProperty^ ThresholdDurationProperty { DependencyProperty^ get(); }

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static property DependencyProperty^ MaxDwellRepeatCountProperty { DependencyProperty^ get(); }

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static property Brush^ DwellFeedbackProgressBrush { Brush^ get(); void set(Brush^ value); }

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static property Brush^ DwellFeedbackCompleteBrush { Brush^ get(); void set(Brush^ value); }

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static Interaction GetInteraction(UIElement^ element);

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static bool GetIsCursorVisible(UIElement^ element);

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static int GetCursorRadius(UIElement^ element);
    
    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static GazeElement^ GetGazeElement(UIElement^ element);

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static TimeSpan GetFixationDuration(UIElement^ element);

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static TimeSpan GetDwellDuration(UIElement^ element);

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static TimeSpan GetRepeatDelayDuration(UIElement^ element);

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static TimeSpan GetDwellRepeatDuration(UIElement^ element);
    
    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static TimeSpan GetThresholdDuration(UIElement^ element);
	
    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static int GetMaxDwellRepeatCount(UIElement^ element);

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static void SetInteraction(UIElement^ element, Interaction value);
    
    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static void SetIsCursorVisible(UIElement^ element, bool value);
	
    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static void SetCursorRadius(UIElement^ element, int value);

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static void SetGazeElement(UIElement^ element, GazeElement^ value);

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static void SetFixationDuration(UIElement^ element, TimeSpan span);

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static void SetDwellDuration(UIElement^ element, TimeSpan span);

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static void SetRepeatDelayDuration(UIElement^ element, TimeSpan span);

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static void SetDwellRepeatDuration(UIElement^ element, TimeSpan span);

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static void SetThresholdDuration(UIElement^ element, TimeSpan span);

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static void SetMaxDwellRepeatCount(UIElement^ element, int value);

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static GazePointer^ GetGazePointer(Page^ page);

    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static property bool IsDeviceAvailable { bool get(); }


    /// <summary>
    /// TODO: harishsk
    /// </summary>
    static event EventHandler<Object^>^ IsDeviceAvailableChanged
    {
        EventRegistrationToken add(EventHandler<Object^>^ handler);
        void remove(EventRegistrationToken token);
    }

internal:

    static TimeSpan UnsetTimeSpan;
};

END_NAMESPACE_GAZE_INPUT