//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "Interaction.h"

using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;

BEGIN_NAMESPACE_GAZE_INPUT

ref class GazeElement;
ref class GazePointer;

/// <summary>
/// Static class primarily providing access to attached properties controlling gaze behavior.
/// </summary>
[Windows::Foundation::Metadata::WebHostHidden]
public ref class GazeInput sealed
{
public:

    /// <summary>
    /// Identifyes the Interaction dependency property
    /// </summary>
    static property DependencyProperty^ InteractionProperty { DependencyProperty^ get(); }

    /// <summary>
    /// Identifyes the IsCursorVisible dependency property
    /// </summary>
    static property DependencyProperty^ IsCursorVisibleProperty { DependencyProperty^ get(); }

    /// <summary>
    /// Identifyes the CursorRadius dependency property
    /// </summary>
    static property DependencyProperty^ CursorRadiusProperty { DependencyProperty^ get(); }

    /// <summary>
    /// Identifyes the GazeElement dependency property
    /// </summary>
    static property DependencyProperty^ GazeElementProperty { DependencyProperty^ get(); }

    /// <summary>
    /// Identifyes the FixationDuration dependency property
    /// </summary>
    static property DependencyProperty^ FixationDurationProperty { DependencyProperty^ get(); }

    /// <summary>
    /// Identifies the DwellDuration dependency property
    /// </summary>
    static property DependencyProperty^ DwellDurationProperty { DependencyProperty^ get(); }

    /// <summary>
    /// Identifies the RepeatDelayDuration dependency property
    /// </summary>
    static property DependencyProperty^ RepeatDelayDurationProperty { DependencyProperty^ get(); }

    /// <summary>
    /// Identifies the DwellRepeatDuration dependency property
    /// </summary>
    static property DependencyProperty^ DwellRepeatDurationProperty { DependencyProperty^ get(); }

    /// <summary>
    /// Identifies the ThresholdDuration dependency property
    /// </summary>
    static property DependencyProperty^ ThresholdDurationProperty { DependencyProperty^ get(); }

    /// <summary>
    /// Identifies the MaxDwellRepeatCount dependency property
    /// </summary>
    static property DependencyProperty^ MaxDwellRepeatCountProperty { DependencyProperty^ get(); }

    /// <summary>
    /// Gets or sets the brush to use when displaying the default indication that gaze entered a control
    /// </summary>
    static property Brush^ DwellFeedbackEnterBrush { Brush^ get(); void set(Brush^ value); }

    /// <summary>
    /// Gets or sets the brush to use when displaying the default animation for dwell press
    /// </summary>
    static property Brush^ DwellFeedbackProgressBrush { Brush^ get(); void set(Brush^ value); }

    /// <summary>
    /// Gets or sets the brush to use when displaying the default animation for dwell complete
    /// </summary>
    static property Brush^ DwellFeedbackCompleteBrush { Brush^ get(); void set(Brush^ value); }

    /// <summary>
    /// Gets or sets the interaction default
    /// </summary>
    static property GazeInteraction::Interaction Interaction { GazeInteraction::Interaction get(); void set(GazeInteraction::Interaction value); }

    /// <summary>
    /// Gets the status of gaze interaction over that particular XAML element.
    /// </summary>
    static GazeInteraction::Interaction GetInteraction(UIElement^ element);

    /// <summary>
    /// Gets Boolean indicating whether cursor is shown while user is looking at the school.
    /// </summary>
    static bool GetIsCursorVisible(UIElement^ element);

    /// <summary>
    /// Gets the size of the gaze cursor radius.
    /// </summary>
    static int GetCursorRadius(UIElement^ element);
    
    /// <summary>
    /// Gets the GazeElement associated with an UIElement.
    /// </summary>
    static GazeElement^ GetGazeElement(UIElement^ element);

    /// <summary>
    /// Gets the duration for the control to transition from the Enter state to the Fixation state. At this point, a StateChanged event is fired with PointerState set to Fixation. This event should be used to control the earliest visual feedback the application needs to provide to the user about the gaze location. The default is 350ms.
    /// </summary>
    static TimeSpan GetFixationDuration(UIElement^ element);

    /// <summary>
    /// Gets the duration for the control to transition from the Fixation state to the Dwell state. At this point, a StateChanged event is fired with PointerState set to Dwell. The Enter and Fixation states are typicaly achieved too rapidly for the user to have much control over. In contrast Dwell is conscious event. This is the point at which the control is invoked, e.g. a button click. The application can modify this property to control when a gaze enabled UI element gets invoked after a user starts looking at it.
    /// </summary>
    static TimeSpan GetDwellDuration(UIElement^ element);

    /// <summary>
    /// Gets the additional duration for the first repeat to occur.This prevents inadvertent repeated invocation.
    /// </summary>
    static TimeSpan GetRepeatDelayDuration(UIElement^ element);

    /// <summary>
    /// Gets the duration of repeated dwell invocations, should the user continue to dwell on the control. The first repeat will occur after an additional delay specified by RepeatDelayDuration. Subsequent repeats happen after every period of DwellRepeatDuration. A control is invoked repeatedly only if MaxDwellRepeatCount is set to greater than zero.
    /// </summary>
    static TimeSpan GetDwellRepeatDuration(UIElement^ element);
    
    /// <summary>
    /// Gets the duration that controls when the PointerState moves to either the Enter state or the Exit state. When this duration has elapsed after the user's gaze first enters a control, the PointerState is set to Enter. And when this duration has elapsed after the user's gaze has left the control, the PointerState is set to Exit. In both cases, a StateChanged event is fired. The default is 50ms.
    /// </summary>
    static TimeSpan GetThresholdDuration(UIElement^ element);
	
    /// <summary>
    /// Gets the maximum times the control will invoked repeatedly without the user's gaze having to leave and re-enter the control. The default value is zero which disables repeated invocation of a control. Developers can set a higher value to enable repeated invocation.
    /// </summary>
    static int GetMaxDwellRepeatCount(UIElement^ element);

    /// <summary>
    /// Sets the status of gaze interaction over that particular XAML element.
    /// </summary>
    static void SetInteraction(UIElement^ element, GazeInteraction::Interaction value);
    
    /// <summary>
    /// Sets Boolean indicating whether cursor is shown while user is looking at the school.
    /// </summary>
    static void SetIsCursorVisible(UIElement^ element, bool value);
	
    /// <summary>
    /// Sets the size of the gaze cursor radius.
    /// </summary>
    static void SetCursorRadius(UIElement^ element, int value);

    /// <summary>
    /// Sets the GazeElement associated with an UIElement.
    /// </summary>
    static void SetGazeElement(UIElement^ element, GazeElement^ value);

    /// <summary>
    /// Sets the duration for the control to transition from the Enter state to the Fixation state. At this point, a StateChanged event is fired with PointerState set to Fixation. This event should be used to control the earliest visual feedback the application needs to provide to the user about the gaze location. The default is 350ms.
    /// </summary>
    static void SetFixationDuration(UIElement^ element, TimeSpan span);

    /// <summary>
    /// Sets the duration for the control to transition from the Fixation state to the Dwell state. At this point, a StateChanged event is fired with PointerState set to Dwell. The Enter and Fixation states are typicaly achieved too rapidly for the user to have much control over. In contrast Dwell is conscious event. This is the point at which the control is invoked, e.g. a button click. The application can modify this property to control when a gaze enabled UI element gets invoked after a user starts looking at it.
    /// </summary>
    static void SetDwellDuration(UIElement^ element, TimeSpan span);

    /// <summary>
    /// Sets the additional duration for the first repeat to occur.This prevents inadvertent repeated invocation.
    /// </summary>
    static void SetRepeatDelayDuration(UIElement^ element, TimeSpan span);

    /// <summary>
    /// Sets the duration of repeated dwell invocations, should the user continue to dwell on the control. The first repeat will occur after an additional delay specified by RepeatDelayDuration. Subsequent repeats happen after every period of DwellRepeatDuration. A control is invoked repeatedly only if MaxDwellRepeatCount is set to greater than zero.
    /// </summary>
    static void SetDwellRepeatDuration(UIElement^ element, TimeSpan span);

    /// <summary>
    /// Sets the duration that controls when the PointerState moves to either the Enter state or the Exit state. When this duration has elapsed after the user's gaze first enters a control, the PointerState is set to Enter. And when this duration has elapsed after the user's gaze has left the control, the PointerState is set to Exit. In both cases, a StateChanged event is fired. The default is 50ms.
    /// </summary>
    static void SetThresholdDuration(UIElement^ element, TimeSpan span);

    /// <summary>
    /// Sets the maximum times the control will invoked repeatedly without the user's gaze having to leave and re-enter the control. The default value is zero which disables repeated invocation of a control. Developers can set a higher value to enable repeated invocation.
    /// </summary>
    static void SetMaxDwellRepeatCount(UIElement^ element, int value);

    /// <summary>
    /// Gets the GazePointer object.
    /// </summary>
    static GazePointer^ GetGazePointer(Page^ page);

    /// <summary>
    /// Invoke the default action of the specified UIElement.
    /// </summary>
    static void Invoke(UIElement^ element);

    /// <summary>
    /// Reports whether a gaze input device is available, and hence whether there is any possibility of gaze events occurring in the application.
    /// </summary>
    static property bool IsDeviceAvailable { bool get(); }

    /// <summary>
    /// Event triggered whenever IsDeviceAvailable changes value.
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