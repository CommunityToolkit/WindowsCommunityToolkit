//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#pragma warning(disable:4453)

#include "IGazeFilter.h"
#include "GazeCursor.h"
#include "GazeEnablement.h"

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
    static property DependencyProperty^ IsGazeEnabledProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ IsCursorVisibleProperty { DependencyProperty^ get(); }
	static property DependencyProperty^ CursorRadiusProperty { DependencyProperty^ get(); }

    static property DependencyProperty^ GazeElementProperty { DependencyProperty^ get(); }

    static property DependencyProperty^ FixationProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ DwellProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ DwellRepeatProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ EnterProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ ExitProperty { DependencyProperty^ get(); }

	static property DependencyProperty^ MaxRepeatCountProperty { DependencyProperty^ get(); }

    static property Brush^ GazeFeedbackProgressBrush { Brush^ get(); void set(Brush^ value); }
    static property Brush^ GazeFeedbackCompleteBrush { Brush^ get(); void set(Brush^ value); }

    static GazeEnablement GetIsGazeEnabled(UIElement^ element);
    static bool GetIsCursorVisible(UIElement^ element);
	static int GetCursorRadius(UIElement^ element);
    static GazeElement^ GetGazeElement(UIElement^ element);
    static TimeSpan GetFixation(UIElement^ element);
    static TimeSpan GetDwell(UIElement^ element);
    static TimeSpan GetDwellRepeat(UIElement^ element);
    static TimeSpan GetEnter(UIElement^ element);
    static TimeSpan GetExit(UIElement^ element);
	static int GetMaxRepeatCount(UIElement^ element);

    static void SetIsGazeEnabled(UIElement^ element, GazeEnablement value);
    static void SetIsCursorVisible(UIElement^ element, bool value);
	static void SetCursorRadius(UIElement^ element, int value);
    static void SetGazeElement(UIElement^ element, GazeElement^ value);
    static void SetFixation(UIElement^ element, TimeSpan span);
    static void SetDwell(UIElement^ element, TimeSpan span);
    static void SetDwellRepeat(UIElement^ element, TimeSpan span);
    static void SetEnter(UIElement^ element, TimeSpan span);
    static void SetExit(UIElement^ element, TimeSpan span);
	static void SetMaxRepeatCount(UIElement^ element, int value);

	static GazePointer^ GetGazePointer(Page^ page);

internal:

    static TimeSpan UnsetTimeSpan;
};

END_NAMESPACE_GAZE_INPUT