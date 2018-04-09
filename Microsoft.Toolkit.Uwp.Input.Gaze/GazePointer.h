//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once
#pragma warning(disable:4453)

#include "IGazeFilter.h"
#include <map>
#include "GazeCursor.h"
#include "GazeSettings.h"

using namespace Platform::Collections;
using namespace Windows::Foundation;
using namespace Windows::Devices::Enumeration;
using namespace Windows::Devices::HumanInterfaceDevice;
using namespace Windows::UI::Core;
using namespace Windows::Devices::Input::Preview;

namespace Shapes = Windows::UI::Xaml::Shapes;

BEGIN_NAMESPACE_GAZE_INPUT

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

// assocate a particular GazePointerState with a duration
typedef Map<GazePointerState, int> GazeInvokeParams;

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
    // used to keep track of when the next DwellRepeat event is to be fired
    property int NextDwellRepeatTime;

    GazeTargetItem(UIElement^ target, int64 timestamp, int nextStateTime, int nextRepeatTime)
    {
        TargetElement = target;
        ElementState = GazePointerState::PreEnter;
        ElapsedTime = 0;
        LastTimestamp = timestamp;
        NextStateTime = nextStateTime;
        NextDwellRepeatTime = nextRepeatTime;
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
public delegate void GazeInputEvent(GazePointer^ sender, GazeEventArgs^ ea);

public delegate bool GazeIsInvokableDelegate(UIElement^ target);
public delegate void GazeInvokeTargetDelegate(UIElement^ target);

public ref class GazePointer sealed
{
public:
    GazePointer(UIElement^ root);
    virtual ~GazePointer();

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

    event GazePointerEvent^ OnGazePointerEvent;
    event GazeInputEvent^ OnGazeInputEvent;

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

    property bool InputEventForwardingEnabled;

private:
    void    InitializeHistogram();
    void    InitializeGazeInputSource();

    GazeInvokeParams^   GetGazeInvokeParams(UIElement^ target);
    UIElement^          GetHitTarget(Point gazePoint);
    UIElement^          ResolveHitTarget(Point gazePoint, long long timestamp);

    bool    IsInvokable(UIElement^ target);

    void    CheckIfExiting(long long curTimestamp);
    void    GotoState(UIElement^ control, GazePointerState state);
    void    RaiseGazePointerEvent(UIElement^ target, GazePointerState state, int elapsedTime);

    void OnGazeMoved(
        GazeInputSourcePreview^ provider,
        GazeMovedPreviewEventArgs^ args);

    void ProcessGazePoint(GazePointPreview^ gazePointPreview);

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
    GazeInvokeParams^                   _defaultInvokeParams;

    // Maps the hash code of passed in FrameworkElement to a particular set of GazeInvokeParams
    // This member is an std::map instead of a Platform::Collections::Map because GazeInvokeParams
    // isn't recognized as a WinRT compatible type
    std::map<int, GazeInvokeParams^>    _elementInvokeParams;

    // The key is the hash code of the FrameworkElemnt
    // The value is the total time that FrameworkElement has been gazed at
    Map<int, GazeTargetItem^>^          _hitTargetTimes;

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

    GazeSettings^                       _gazeSettings;
};

END_NAMESPACE_GAZE_INPUT