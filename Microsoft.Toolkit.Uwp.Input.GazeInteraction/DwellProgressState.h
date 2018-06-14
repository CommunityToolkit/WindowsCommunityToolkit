#pragma once

BEGIN_NAMESPACE_GAZE_INPUT

/// <summary>
/// An enum that reflects the current state of progress towards dwell when a user is focused on a control
/// </summary>
public enum class DwellProgressState
{
    /// <summary>
    /// User is not looking at the control
    /// </summary>
    Idle,

    /// <summary>
    /// Gaze has entered control but we're not yet showing progress.
    /// </summary>
    Fixating,

    /// <summary>
    /// User is continuing to focus on a control with an intent to dwell and invoke
    /// </summary>
    Progressing,

    /// <summary>
    /// User has completed dwelling on a control
    /// </summary>
    Complete
};

END_NAMESPACE_GAZE_INPUT