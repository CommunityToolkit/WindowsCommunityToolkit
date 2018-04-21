//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#pragma warning(disable:4453)

#include "IGazeFilter.h"
#include "GazeCursor.h"

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

public ref class GazeApi sealed
{
public:
    static property DependencyProperty^ IsGazeEnabledProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ IsGazeCursorVisibleProperty { DependencyProperty^ get(); }
	static property DependencyProperty^ GazeCursorRadiusProperty { DependencyProperty^ get(); }

    static property DependencyProperty^ GazeElementProperty { DependencyProperty^ get(); }

    static property DependencyProperty^ FixationProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ DwellProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ DwellRepeatProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ EnterProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ ExitProperty { DependencyProperty^ get(); }

	static property DependencyProperty^ MaxRepeatCountProperty { DependencyProperty^ get(); }

    static bool GetIsGazeEnabled(UIElement^ element);
    static bool GetIsGazeCursorVisible(UIElement^ element);
	static int GetGazeCursorRadius(UIElement^ element);
    static GazeElement^ GetGazeElement(UIElement^ element);
    static TimeSpan GetFixation(UIElement^ element);
    static TimeSpan GetDwell(UIElement^ element);
    static TimeSpan GetDwellRepeat(UIElement^ element);
    static TimeSpan GetEnter(UIElement^ element);
    static TimeSpan GetExit(UIElement^ element);
	static int GetMaxRepeatCount(UIElement^ element);

    static void SetIsGazeEnabled(UIElement^ element, bool value);
    static void SetIsGazeCursorVisible(UIElement^ element, bool value);
	static void SetGazeCursorRadius(UIElement^ element, int value);
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