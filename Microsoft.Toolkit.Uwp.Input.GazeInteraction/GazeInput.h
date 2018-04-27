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

public ref class GazeInput sealed
{
public:
    static property DependencyProperty^ InteractionProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ IsCursorVisibleProperty { DependencyProperty^ get(); }
	static property DependencyProperty^ CursorRadiusProperty { DependencyProperty^ get(); }

    static property DependencyProperty^ GazeElementProperty { DependencyProperty^ get(); }

    static property DependencyProperty^ FixationDurationProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ DwellDurationProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ RepeatDelayDurationProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ DwellRepeatDurationProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ ThresholdDurationProperty { DependencyProperty^ get(); }

	static property DependencyProperty^ MaxDwellRepeatCountProperty { DependencyProperty^ get(); }

    static property Brush^ DwellFeedbackProgressBrush { Brush^ get(); void set(Brush^ value); }
    static property Brush^ DwellFeedbackCompleteBrush { Brush^ get(); void set(Brush^ value); }

    static Interaction GetInteraction(UIElement^ element);
    static bool GetIsCursorVisible(UIElement^ element);
	static int GetCursorRadius(UIElement^ element);
    static GazeElement^ GetGazeElement(UIElement^ element);
    static TimeSpan GetFixationDuration(UIElement^ element);
    static TimeSpan GetDwellDuration(UIElement^ element);
    static TimeSpan GetRepeatDelayDuration(UIElement^ element);
    static TimeSpan GetDwellRepeatDuration(UIElement^ element);
    static TimeSpan GetThresholdDuration(UIElement^ element);
	static int GetMaxDwellRepeatCount(UIElement^ element);

    static void SetInteraction(UIElement^ element, Interaction value);
    static void SetIsCursorVisible(UIElement^ element, bool value);
	static void SetCursorRadius(UIElement^ element, int value);
    static void SetGazeElement(UIElement^ element, GazeElement^ value);
    static void SetFixationDuration(UIElement^ element, TimeSpan span);
    static void SetDwellDuration(UIElement^ element, TimeSpan span);
    static void SetRepeatDelayDuration(UIElement^ element, TimeSpan span);
    static void SetDwellRepeatDuration(UIElement^ element, TimeSpan span);
    static void SetThresholdDuration(UIElement^ element, TimeSpan span);
	static void SetMaxDwellRepeatCount(UIElement^ element, int value);

	static GazePointer^ GetGazePointer(Page^ page);

    static property bool IsDeviceAvailable { bool get(); }
    static event EventHandler<Object^>^ IsDeviceAvailableChanged
    {
        EventRegistrationToken add(EventHandler<Object^>^ handler);
        void remove(EventRegistrationToken token);
    }

internal:

    static TimeSpan UnsetTimeSpan;
};

END_NAMESPACE_GAZE_INPUT