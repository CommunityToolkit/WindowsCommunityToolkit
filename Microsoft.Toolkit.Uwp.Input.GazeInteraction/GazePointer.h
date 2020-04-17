//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

#include "GazePointer.g.h"
#include "GazeCursor.h"
#include "GazeFeedbackPopupFactory.h"
#include "GazeEventArgs.h"
#include "GazeFilter.h"
#include "PointerState.h"
#include "GazeTargetItem.h"
#include <Interaction.h>
#include <GazeEventArgs.h>
#include <winrt/Microsoft.UI.Xaml.Automation.Peers.h>
#include <winrt/Microsoft.UI.Xaml.h>

using namespace winrt;
using namespace winrt::Windows::Devices::Input::Preview;
using namespace winrt::Windows::Foundation;
using namespace winrt::Windows::Foundation::Collections;
using namespace winrt::Microsoft::UI::Xaml;

namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::implementation
{
    /// <summary>
    /// Class of singleton object coordinating gaze input.
    /// </summary>
    struct GazePointer : GazePointerT<GazePointer>
    {
    public:
        GazePointer();

        /// <summary>
        /// Loads a settings collection into GazePointer.
        /// </summary>
        void LoadSettings(Windows::Foundation::Collections::ValueSet const& settings);

        /// <summary>
        /// When in switch mode, will issue a click on the currently fixated element
        /// </summary>
        void Click();

        /// <summary>
        /// Run device calibration.
        /// </summary>
        Windows::Foundation::IAsyncOperation<bool> RequestCalibrationAsync();

        winrt::event_token GazeEvent(Windows::Foundation::EventHandler<Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeEventArgs> const& handler);

        void GazeEvent(winrt::event_token const& token) noexcept;

        /// <summary>
        /// The UIElement representing the cursor.
        /// </summary>
        Microsoft::UI::Xaml::UIElement CursorElement();

        bool IsAlwaysActivated();
        void IsAlwaysActivated(bool value);

        virtual ~GazePointer();

        static Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazePointer Instance();

        void AddRoot(int32_t proxyId);
        void RemoveRoot(int32_t proxyId);

        bool IsDeviceAvailable();

        winrt::event_token IsDeviceAvailableChanged(Windows::Foundation::EventHandler<Windows::Foundation::IInspectable> const& handler);
        void IsDeviceAvailableChanged(winrt::event_token const& token) noexcept;

        double _dwellStrokeThickness();
        void _dwellStrokeThickness(double const& value);

        bool IsCursorVisible();
        void IsCursorVisible(bool const& value);

        int CursorRadius();
        void CursorRadius(int value);

        bool IsSwitchEnabled();
        void IsSwitchEnabled(bool value);

        // Internal

        winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::Interaction _interaction = Interaction::Disabled;

        Brush _enterBrush = nullptr;

        Brush _progressBrush = SolidColorBrush(winrt::Microsoft::UI::Colors::Green());

        Brush _completeBrush = SolidColorBrush(winrt::Microsoft::UI::Colors::Red());

        double m_dwellStrokeThickness = 2;

        GazeFeedbackPopupFactory _gazeFeedbackPopupFactory;

        winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem _nonInvokeGazeTargetItem{ nullptr };

    private:

        static Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazePointer m_instance;

        // units in microseconds
        const TimeSpan DEFAULT_FIXATION_DELAY = TimeSpanFromMicroseconds(350000);
        const TimeSpan DEFAULT_DWELL_DELAY = TimeSpanFromMicroseconds(400000);
        const TimeSpan DEFAULT_DWELL_REPEAT_DELAY = TimeSpanFromMicroseconds(400000);
        const TimeSpan DEFAULT_REPEAT_DELAY = TimeSpanFromMicroseconds(400000);
        const TimeSpan DEFAULT_THRESHOLD_DELAY = TimeSpanFromMicroseconds(50000);
        const TimeSpan DEFAULT_MAX_HISTORY_DURATION = TimeSpanFromMicroseconds(3000000);
        const TimeSpan MAX_SINGLE_SAMPLE_DURATION = TimeSpanFromMicroseconds(100000);

        const TimeSpan GAZE_IDLE_TIME{ 25000000 };

        winrt::event<EventHandler<Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeEventArgs>> m_gazeEvent;
        winrt::event<EventHandler<IInspectable>> m_isDeviceAvailableChanged;
        int _gazeEventCount = 0;

        void Reset();
        void SetElementStateDelay(UIElement element, PointerState pointerState, TimeSpan stateDelay);
        TimeSpan GetElementStateDelay(UIElement element, DependencyProperty property, TimeSpan defaultValue);
        TimeSpan GetElementStateDelay(UIElement element, PointerState pointerState);

        // Provide a configurable delay for when the EyesOffDelay event is fired
        // GOTCHA: this value requires that _eyesOffTimer is instantiated so that it
        // can update the timer interval 
        TimeSpan EyesOffDelay() { return _eyesOffDelay; }
        void EyesOffDelay(TimeSpan value)
        {
            _eyesOffDelay = value;

            // convert GAZE_IDLE_TIME units (microseconds) to 100-nanosecond units used
            // by TimeSpan struct
            _eyesOffTimer.Interval(EyesOffDelay());
        }

        // Pluggable filter for eye tracking sample data. This defaults to being set to the
        // NullFilter which performs no filtering of input samples.
        std::unique_ptr<IGazeFilter> Filter{ nullptr };

        bool _initialized{ false };
        bool _isShuttingDown{ false };

        TimeSpan GetDefaultPropertyValue(PointerState state);

        void    InitializeHistogram();
        void    InitializeGazeInputSource();
        void    DeinitializeGazeInputSource();

        void ActivateGazeTargetItem(winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem target);
        winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem GetHitTarget(Point gazePoint);
        winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem ResolveHitTarget(Point gazePoint, TimeSpan timestamp);

        void    CheckIfExiting(TimeSpan curTimestamp);
        void    RaiseGazePointerEvent(winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem target, PointerState state, TimeSpan elapsedTime);

        void OnGazeEntered(
            GazeInputSourcePreview provider,
            GazeEnteredPreviewEventArgs args);
        void OnGazeMoved(
            GazeInputSourcePreview provider,
            GazeMovedPreviewEventArgs args);
        void OnGazeExited(
            GazeInputSourcePreview provider,
            GazeExitedPreviewEventArgs args);

        void ProcessGazePoint(TimeSpan timestamp, Point position);

        void OnEyesOff(IInspectable const& sender, IInspectable const& ea);

        void OnDeviceAdded(GazeDeviceWatcherPreview sender, GazeDeviceWatcherAddedPreviewEventArgs args);
        void OnDeviceRemoved(GazeDeviceWatcherPreview sender, GazeDeviceWatcherRemovedPreviewEventArgs args);

        std::vector<int> _roots;

        TimeSpan                               _eyesOffDelay;

        GazeCursor                         _gazeCursor;
        DispatcherTimer                    _eyesOffTimer{ nullptr };

        // _offScreenElement is a pseudo-element that represents the area outside
        // the screen so we can track how long the user has been looking outside
        // the screen and appropriately trigger the EyesOff event
        Control                            _offScreenElement{ nullptr };

        // The value is the total time that FrameworkElement has been gazed at
        std::vector<winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem>            _activeHitTargetTimes;

        // A vector to track the history of observed gaze targets
        std::vector<IGazeHistoryItem>           _gazeHistory;
        TimeSpan                               _maxHistoryTime;

        // Used to determine if exit events need to be fired by adding GAZE_IDLE_TIME to the last 
        // saved timestamp
        TimeSpan                           _lastTimestamp;

        GazeInputSourcePreview             _gazeInputSource;
        winrt::event_token              _gazeEnteredToken;
        winrt::event_token              _gazeMovedToken;
        winrt::event_token              _gazeExitedToken;

        GazeDeviceWatcherPreview _watcher;
        std::vector<GazeDevicePreview> _devices;
        winrt::event_token _deviceAddedToken;
        winrt::event_token _deviceRemovedToken;

        TimeSpan _defaultFixation = DEFAULT_FIXATION_DELAY;
        TimeSpan _defaultDwell = DEFAULT_DWELL_DELAY;
        TimeSpan _defaultDwellRepeatDelay = DEFAULT_DWELL_REPEAT_DELAY;
        TimeSpan _defaultRepeatDelay = DEFAULT_REPEAT_DELAY;
        TimeSpan _defaultThreshold = DEFAULT_THRESHOLD_DELAY;

        bool                                _isAlwaysActivated{ false };
        bool                                _isSwitchEnabled{ false };
        winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::GazeTargetItem     _currentlyFixatedElement{ nullptr };
    };
}
namespace winrt::Microsoft::Toolkit::Uwp::Input::GazeInteraction::factory_implementation
{
    struct GazePointer : GazePointerT<GazePointer, implementation::GazePointer>
    {
    };
}
