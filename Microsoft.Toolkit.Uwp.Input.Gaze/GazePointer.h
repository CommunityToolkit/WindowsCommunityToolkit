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

ref class GazePage;
ref class GazeElement;
ref class GazePointer;

public ref class GazeApi sealed
{
public:
    static property DependencyProperty^ IsGazeEnabledProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ IsGazeCursorVisibleProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ GazePageProperty { DependencyProperty^ get(); }

    static property DependencyProperty^ GazeElementProperty { DependencyProperty^ get(); }

    static property DependencyProperty^ FixationProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ DwellProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ DwellRepeatProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ EnterProperty { DependencyProperty^ get(); }
    static property DependencyProperty^ ExitProperty { DependencyProperty^ get(); }

	static property DependencyProperty^ MaxRepeatCountProperty { DependencyProperty^ get(); }

    static bool GetIsGazeEnabled(Page^ page);
    static bool GetIsGazeCursorVisible(Page^ page);
    static GazePage^ GetGazePage(Page^ page);
    static GazeElement^ GetGazeElement(UIElement^ element);
    static TimeSpan GetFixation(UIElement^ element);
    static TimeSpan GetDwell(UIElement^ element);
    static TimeSpan GetDwellRepeat(UIElement^ element);
    static TimeSpan GetEnter(UIElement^ element);
    static TimeSpan GetExit(UIElement^ element);
	static int GetMaxRepeatCount(UIElement^ element);

    static void SetIsGazeEnabled(Page^ page, bool value);
    static void SetIsGazeCursorVisible(Page^ page, bool value);
    static void SetGazePage(Page^ page, GazePage^ value);
    static void SetGazeElement(UIElement^ element, GazeElement^ value);
    static void SetFixation(UIElement^ element, TimeSpan span);
    static void SetDwell(UIElement^ element, TimeSpan span);
    static void SetDwellRepeat(UIElement^ element, TimeSpan span);
    static void SetEnter(UIElement^ element, TimeSpan span);
    static void SetExit(UIElement^ element, TimeSpan span);
	static void SetMaxRepeatCount(UIElement^ element, int value);

	static GazePointer^ GetGazePointer(Page^ page);
};

// units in microseconds
const int DEFAULT_FIXATION_DELAY = 400000;
const int DEFAULT_DWELL_DELAY = 800000;
const int DEFAULT_REPEAT_DELAY = 1600000;
const int DEFAULT_ENTER_EXIT_DELAY = 50000;
const int DEFAULT_MAX_HISTORY_DURATION = 3000000;
const int MAX_SINGLE_SAMPLE_DURATION = 100000;

const int GAZE_IDLE_TIME = 2500000;

public enum class GazePointerState
{
    Exit,

    // The order of the following elements is important because
    // they represent states that linearly transition to their
    // immediate successors. 
    PreEnter,
    Enter,
    Fixation,
    Dwell,
    //FixationRepeat,
    DwellRepeat
};

ref struct GazeHistoryItem
{
    property UIElement^ HitTarget;
    property int64 Timestamp;
    property int Duration;
};

ref struct GazeTargetItem sealed
{
    property int ElapsedTime;
    property int NextStateTime;
    property int64 LastTimestamp;
    property GazePointerState ElementState;
    property UIElement^ TargetElement;
	property int RepeatCount;
	property int MaxRepeatCount;

    GazeTargetItem(UIElement^ target)
    {
        TargetElement = target;
    }

    void Reset(int nextStateTime)
    {
        ElementState = GazePointerState::PreEnter;
        ElapsedTime = 0;
        NextStateTime = nextStateTime;
		RepeatCount = 0;
		MaxRepeatCount = GazeApi::GetMaxRepeatCount(TargetElement);
    }
};

public ref struct GazePointerEventArgs sealed
{
    property UIElement^ HitTarget;
    property GazePointerState PointerState;
    property int ElapsedTime;

    GazePointerEventArgs(UIElement^ target, GazePointerState state, int elapsedTime)
    {
        HitTarget = target;
        PointerState = state;
        ElapsedTime = elapsedTime;
    }
};

ref class GazePointer;
public delegate void GazePointerEvent(GazePointer^ sender, GazePointerEventArgs^ ea);

public delegate bool GazeIsInvokableDelegate(UIElement^ target);
public delegate void GazeInvokeTargetDelegate(UIElement^ target);

public ref class GazePage sealed
{
public:
    event GazePointerEvent^ GazePointerEvent;

    void RaiseGazePointerEvent(GazePointer^ sender, GazePointerEventArgs^ args) { GazePointerEvent(sender, args); }
};

public ref class GazeInvokedRoutedEventArgs : public RoutedEventArgs
{
public:

    property bool Handled;
};

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

    void RaiseInvoked(Object^ sender, GazeInvokedRoutedEventArgs^ args)
    {
        Invoked(sender, args);
    }
};

public ref class GazePointer sealed
{
public:
    virtual ~GazePointer();

    void LoadSettings(ValueSet^ settings);

    property GazeIsInvokableDelegate^ IsInvokableImpl
    {
        GazeIsInvokableDelegate^ get()
        {
            return _isInvokableImpl;
        }
        void set(GazeIsInvokableDelegate^ value)
        {
            _isInvokableImpl = value;
        }
    }

    property GazeInvokeTargetDelegate^ InvokeTargetImpl
    {
        GazeInvokeTargetDelegate^ get()
        {
            return _invokeTargetImpl;
        }
        void set(GazeInvokeTargetDelegate^ value)
        {
            _invokeTargetImpl = value;
        }
    }

    void InvokeTarget(UIElement^ target);
    void Reset();
    void SetElementStateDelay(UIElement ^element, GazePointerState pointerState, int stateDelay);
    int GetElementStateDelay(UIElement^ element, GazePointerState pointerState);

    // Provide a configurable delay for when the EyesOffDelay event is fired
    // GOTCHA: this value requires that _eyesOffTimer is instantiated so that it
    // can update the timer interval 
    property int64 EyesOffDelay
    {
        int64 get() { return _eyesOffDelay; }
        void set(int64 value)
        {
            _eyesOffDelay = value;

            // convert GAZE_IDLE_TIME units (microseconds) to 100-nanosecond units used
            // by TimeSpan struct
            _eyesOffTimer->Interval = TimeSpan{ EyesOffDelay * 10 };
        }
    }

    // Pluggable filter for eye tracking sample data. This defaults to being set to the
    // NullFilter which performs no filtering of input samples.
    property IGazeFilter^ Filter;

    property bool IsCursorVisible
    {
        bool get() { return _gazeCursor->IsCursorVisible; }
        void set(bool value) { _gazeCursor->IsCursorVisible = value; }
    }

    property int CursorRadius
    {
        int get() { return _gazeCursor->CursorRadius; }
        void set(int value) { _gazeCursor->CursorRadius = value; }
    }

internal:

	GazePointer(UIElement^ root);
    void OnPageUnloaded(Object^ sender, RoutedEventArgs^ args);
    EventRegistrationToken _unloadedToken;

private:

    bool _isShuttingDown;

	TimeSpan GetDefaultPropertyValue(GazePointerState state);

    void    InitializeHistogram();
    void    InitializeGazeInputSource();

    GazeTargetItem^     GetOrCreateGazeTargetItem(UIElement^ target);
    GazeTargetItem^     GetGazeTargetItem(UIElement^ target);
    UIElement^          GetHitTarget(Point gazePoint);
    UIElement^          ResolveHitTarget(Point gazePoint, long long timestamp);

    bool    IsInvokable(UIElement^ target);

    void    CheckIfExiting(long long curTimestamp);
    void    GotoState(UIElement^ control, GazePointerState state);
    void    RaiseGazePointerEvent(UIElement^ target, GazePointerState state, int elapsedTime);

    void OnGazeMoved(
        GazeInputSourcePreview^ provider,
        GazeMovedPreviewEventArgs^ args);

    void ProcessGazePoint(long long timestamp, Point position);

    void    OnEyesOff(Object ^sender, Object ^ea);


private:
    UIElement ^                         _rootElement;

    int64                               _eyesOffDelay;

    GazeCursor^                         _gazeCursor;
    DispatcherTimer^                    _eyesOffTimer;

    // _offScreenElement is a pseudo-element that represents the area outside
    // the screen so we can track how long the user has been looking outside
    // the screen and appropriately trigger the EyesOff event
    Control^                            _offScreenElement;

    // The value is the total time that FrameworkElement has been gazed at
    Vector<GazeTargetItem^>^            _activeHitTargetTimes;

    // A vector to track the history of observed gaze targets
    Vector<GazeHistoryItem^>^           _gazeHistory;
    int64                               _maxHistoryTime;

    // Used to determine if exit events need to be fired by adding GAZE_IDLE_TIME to the last 
    // saved timestamp
    long long                           _lastTimestamp;

    GazeInputSourcePreview^             _gazeInputSource;
    EventRegistrationToken              _gazeMovedToken;
    CoreDispatcher^                     _coreDispatcher;
    GazeIsInvokableDelegate^            _isInvokableImpl;
    GazeInvokeTargetDelegate^           _invokeTargetImpl;

	int _defaultFixation = DEFAULT_FIXATION_DELAY;
	int _defaultDwell = DEFAULT_DWELL_DELAY;
	int _defaultRepeat = DEFAULT_REPEAT_DELAY;
	int _defaultEnter = DEFAULT_ENTER_EXIT_DELAY;
	int _defaultExit = DEFAULT_ENTER_EXIT_DELAY;
};

END_NAMESPACE_GAZE_INPUT