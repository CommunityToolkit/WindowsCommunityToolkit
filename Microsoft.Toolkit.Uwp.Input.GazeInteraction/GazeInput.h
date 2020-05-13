//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "GazeInput.g.h"
#include "Interaction.h"
#include <winrt/Microsoft.UI.Xaml.Media.h>
#include <winrt/Microsoft.UI.Xaml.Interop.h>
#include <winrt/Microsoft.UI.Xaml.Automation.h>
#include <winrt/Microsoft.UI.Xaml.Automation.Provider.h>
#include <winrt/Windows.Foundation.Collections.h>
#include <GazePointerProxy.h>
#include <GazePointer.h>

using namespace winrt::Windows::Foundation::Collections;
using namespace winrt::Microsoft::UI::Xaml;
using namespace winrt::Microsoft::UI::Xaml::Controls;
using namespace winrt::Microsoft::UI::Xaml::Media;

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    /// <summary>
    /// Static class primarily providing access to attached properties controlling gaze behavior.
    /// </summary>
    //[Windows::Foundation::Metadata::WebHostHidden]
    struct GazeInput : GazeInputT<GazeInput>
    {
    public:
        GazeInput() = default;

        /// <summary>
        /// Reports whether a gaze input device is available, and hence whether there is any possibility of gaze events occurring in the application.
        /// </summary>
        static bool IsDeviceAvailable();

        /// <summary>
        /// Identifyes the Interaction dependency property
        /// </summary>
        static Microsoft::UI::Xaml::DependencyProperty InteractionProperty();

        /// <summary>
        /// Identifyes the IsCursorVisible dependency property
        /// </summary>
        static Microsoft::UI::Xaml::DependencyProperty IsCursorVisibleProperty();

        /// <summary>
        /// Identifyes the CursorRadius dependency property
        /// </summary>
        static Microsoft::UI::Xaml::DependencyProperty CursorRadiusProperty();

        /// <summary>
        /// Identifyes the GazeElement dependency property
        /// </summary>
        static Microsoft::UI::Xaml::DependencyProperty GazeElementProperty();

        /// <summary>
        /// Identifyes the FixationDuration dependency property
        /// </summary>
        static Microsoft::UI::Xaml::DependencyProperty FixationDurationProperty();

        /// <summary>
        /// Identifies the DwellDuration dependency property
        /// </summary>
        static Microsoft::UI::Xaml::DependencyProperty DwellDurationProperty();

        /// <summary>
        /// Identifies the RepeatDelayDuration dependency property
        /// </summary>
        static Microsoft::UI::Xaml::DependencyProperty RepeatDelayDurationProperty();

        /// <summary>
        /// Identifies the DwellRepeatDuration dependency property
        /// </summary>
        static Microsoft::UI::Xaml::DependencyProperty DwellRepeatDurationProperty();

        /// <summary>
        /// Identifies the ThresholdDuration dependency property
        /// </summary>
        static Microsoft::UI::Xaml::DependencyProperty ThresholdDurationProperty();

        /// <summary>
        /// Identifies the MaxDwellRepeatCount dependency property
        /// </summary>
        static Microsoft::UI::Xaml::DependencyProperty MaxDwellRepeatCountProperty();

        /// <summary>
        /// Identifyes the IsSwitchEnabled dependency property
        /// </summary>
        static Microsoft::UI::Xaml::DependencyProperty IsSwitchEnabledProperty();

        /// <summary>
        /// Gets the interaction default
        /// </summary>
        static Microsoft::Toolkit::Uwp::Input::GazeInteraction::Interaction Interaction();

        /// <summary>
        /// Sets the interaction default
        /// </summary>
        static void Interaction(Microsoft::Toolkit::Uwp::Input::GazeInteraction::Interaction const& value);

        /// <summary>
        /// Gets the brush to use when displaying the default indication that gaze entered a control
        /// </summary>
        static Microsoft::UI::Xaml::Media::Brush DwellFeedbackEnterBrush();

        /// <summary>
        /// Sets the brush to use when displaying the default indication that gaze entered a control
        /// </summary>
        static void DwellFeedbackEnterBrush(Microsoft::UI::Xaml::Media::Brush const& value);

        /// <summary>
        /// Gets the brush to use when displaying the default animation for dwell press
        /// </summary>
        static Microsoft::UI::Xaml::Media::Brush DwellFeedbackProgressBrush();

        /// <summary>
        /// Sets the brush to use when displaying the default animation for dwell press
        /// </summary>
        static void DwellFeedbackProgressBrush(Microsoft::UI::Xaml::Media::Brush const& value);

        /// <summary>
        /// Gets the brush to use when displaying the default animation for dwell complete
        /// </summary>
        static Microsoft::UI::Xaml::Media::Brush DwellFeedbackCompleteBrush();

        /// <summary>
        /// Sets the brush to use when displaying the default animation for dwell complete
        /// </summary>
        static void DwellFeedbackCompleteBrush(Microsoft::UI::Xaml::Media::Brush const& value);

        /// <summary>
        /// Gets the status of gaze interaction over that particular XAML element.
        /// </summary>
        static Microsoft::Toolkit::Uwp::Input::GazeInteraction::Interaction GetInteraction(Microsoft::UI::Xaml::UIElement const& element);

        /// <summary>
        /// Gets the thickness of the lines animated for dwell.
        /// </summary>
        static double DwellStrokeThickness();

        /// <summary>
        /// Sets the thickness of the lines animated for dwell.
        /// </summary>
        static void DwellStrokeThickness(double value);

        /// <summary>
        /// Gets Boolean indicating whether cursor is shown while user is looking at the school.
        /// </summary>
        static bool GetIsCursorVisible(UIElement const& element);

        /// <summary>
        /// Gets the size of the gaze cursor radius.
        /// </summary>
        static int GetCursorRadius(UIElement const& element);

        /// <summary>
        /// Gets the GazeElement associated with an UIElement.
        /// </summary>
        static Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeElement GetGazeElement(UIElement const& element);

        /// <summary>
        /// Gets the duration for the control to transition from the Enter state to the Fixation state. At this point, a StateChanged event is fired with PointerState set to Fixation. This event should be used to control the earliest visual feedback the application needs to provide to the user about the gaze location. The default is 350ms.
        /// </summary>
        static TimeSpan GetFixationDuration(UIElement const& element);

        /// <summary>
        /// Gets the duration for the control to transition from the Fixation state to the Dwell state. At this point, a StateChanged event is fired with PointerState set to Dwell. The Enter and Fixation states are typicaly achieved too rapidly for the user to have much control over. In contrast Dwell is conscious event. This is the point at which the control is invoked, e.g. a button click. The application can modify this property to control when a gaze enabled UI element gets invoked after a user starts looking at it.
        /// </summary>
        static TimeSpan GetDwellDuration(UIElement const& element);

        /// <summary>
        /// Gets the additional duration for the first repeat to occur.This prevents inadvertent repeated invocation.
        /// </summary>
        static TimeSpan GetRepeatDelayDuration(UIElement const& element);

        /// <summary>
        /// Gets the duration of repeated dwell invocations, should the user continue to dwell on the control. The first repeat will occur after an additional delay specified by RepeatDelayDuration. Subsequent repeats happen after every period of DwellRepeatDuration. A control is invoked repeatedly only if MaxDwellRepeatCount is set to greater than zero.
        /// </summary>
        static TimeSpan GetDwellRepeatDuration(UIElement const& element);

        /// <summary>
        /// Gets the duration that controls when the PointerState moves to either the Enter state or the Exit state. When this duration has elapsed after the user's gaze first enters a control, the PointerState is set to Enter. And when this duration has elapsed after the user's gaze has left the control, the PointerState is set to Exit. In both cases, a StateChanged event is fired. The default is 50ms.
        /// </summary>
        static TimeSpan GetThresholdDuration(UIElement const& element);

        /// <summary>
        /// Gets the maximum times the control will invoked repeatedly without the user's gaze having to leave and re-enter the control. The default value is zero which disables repeated invocation of a control. Developers can set a higher value to enable repeated invocation.
        /// </summary>
        static int GetMaxDwellRepeatCount(UIElement const& element);

        /// <summary>
        /// Gets the Boolean indicating whether gaze plus switch is enabled.
        /// </summary>
        static bool GetIsSwitchEnabled(UIElement const& element);

        /// <summary>
        /// Sets the status of gaze interaction over that particular XAML element.
        /// </summary>
        static void SetInteraction(UIElement const& element, GazeInteraction::Interaction const& value);

        /// <summary>
        /// Sets Boolean indicating whether cursor is shown while user is looking at the school.
        /// </summary>
        static void SetIsCursorVisible(UIElement const& element, bool value);

        /// <summary>
        /// Sets the size of the gaze cursor radius.
        /// </summary>
        static void SetCursorRadius(UIElement const& element, int value);

        /// <summary>
        /// Sets the GazeElement associated with an UIElement.
        /// </summary>
        static void SetGazeElement(UIElement const& element, Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeElement const& value);

        /// <summary>
        /// Sets the duration for the control to transition from the Enter state to the Fixation state. At this point, a StateChanged event is fired with PointerState set to Fixation. This event should be used to control the earliest visual feedback the application needs to provide to the user about the gaze location. The default is 350ms.
        /// </summary>
        static void SetFixationDuration(UIElement const& element, TimeSpan const& span);

        /// <summary>
        /// Sets the duration for the control to transition from the Fixation state to the Dwell state. At this point, a StateChanged event is fired with PointerState set to Dwell. The Enter and Fixation states are typicaly achieved too rapidly for the user to have much control over. In contrast Dwell is conscious event. This is the point at which the control is invoked, e.g. a button click. The application can modify this property to control when a gaze enabled UI element gets invoked after a user starts looking at it.
        /// </summary>
        static void SetDwellDuration(UIElement const& element, TimeSpan const& span);

        /// <summary>
        /// Sets the additional duration for the first repeat to occur.This prevents inadvertent repeated invocation.
        /// </summary>
        static void SetRepeatDelayDuration(UIElement const& element, TimeSpan const& span);

        /// <summary>
        /// Sets the duration of repeated dwell invocations, should the user continue to dwell on the control. The first repeat will occur after an additional delay specified by RepeatDelayDuration. Subsequent repeats happen after every period of DwellRepeatDuration. A control is invoked repeatedly only if MaxDwellRepeatCount is set to greater than zero.
        /// </summary>
        static void SetDwellRepeatDuration(UIElement const& element, TimeSpan const& span);

        /// <summary>
        /// Sets the duration that controls when the PointerState moves to either the Enter state or the Exit state. When this duration has elapsed after the user's gaze first enters a control, the PointerState is set to Enter. And when this duration has elapsed after the user's gaze has left the control, the PointerState is set to Exit. In both cases, a StateChanged event is fired. The default is 50ms.
        /// </summary>
        static void SetThresholdDuration(UIElement const& element, TimeSpan const& span);

        /// <summary>
        /// Sets the maximum times the control will invoked repeatedly without the user's gaze having to leave and re-enter the control. The default value is zero which disables repeated invocation of a control. Developers can set a higher value to enable repeated invocation.
        /// </summary>
        static void SetMaxDwellRepeatCount(UIElement const& element, int value);

        /// <summary>
        /// Sets the Boolean indicating whether gaze plus switch is enabled.
        /// </summary>
        static void SetIsSwitchEnabled(UIElement const& element, bool value);

        /// <summary>
        /// Gets the GazePointer object.
        /// </summary>
        static Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazePointer GetGazePointer(Microsoft::UI::Xaml::Controls::Page const& page);

        /// <summary>
        /// Invoke the default action of the specified UIElement.
        /// </summary>
        static void Invoke(UIElement element);

        /// <summary>
        /// Event triggered whenever IsDeviceAvailable changes value.
        /// </summary>
        static winrt::event_token IsDeviceAvailableChanged(winrt::Windows::Foundation::EventHandler<IInspectable> const& handler);

        static void IsDeviceAvailableChanged(winrt::event_token const& token) noexcept;

        static TimeSpan UnsetTimeSpan;

        static void OnInteractionChanged(DependencyObject const& ob, DependencyPropertyChangedEventArgs const& args)
        {
            FrameworkElement element{ ob.try_as<FrameworkElement>() };
            auto interaction = winrt::unbox_value<GazeInteraction::Interaction>(args.NewValue());
            GazePointerProxy::SetInteraction(element, interaction);
        }

        static void OnIsCursorVisibleChanged(DependencyObject const& ob, DependencyPropertyChangedEventArgs const& args)
        {
            GazePointer::Instance().IsCursorVisible(winrt::unbox_value<bool>(args.NewValue()));
        }

        static void OnCursorRadiusChanged(DependencyObject const& ob, DependencyPropertyChangedEventArgs const& args)
        {
            GazePointer::Instance().CursorRadius(winrt::unbox_value<int>(args.NewValue()));
        }

        static void OnIsSwitchEnabledChanged(DependencyObject const& ob, DependencyPropertyChangedEventArgs const& args)
        {
            GazePointer::Instance().IsSwitchEnabled(winrt::unbox_value<bool>(args.NewValue()));
        }

        /// <summary>
        /// Loads a settings collection into GazeInput.
        /// Note: This must be loaded from a UI thread to be valid, since the GazeInput
        /// instance is tied to the UI thread.
        /// </summary>
        static void LoadSettings(ValueSet settings);
    };
}
namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::factory_implementation
{
    struct GazeInput : GazeInputT<GazeInput, implementation::GazeInput>
    {
    };
}