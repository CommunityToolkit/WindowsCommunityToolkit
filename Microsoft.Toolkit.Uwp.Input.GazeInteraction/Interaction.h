//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

BEGIN_NAMESPACE_GAZE_INPUT

/// <summary>
/// This enum indicates the current state of gaze interaction. 
/// </summary>
public enum class Interaction
{

    /// <summary>
    /// The state of gaze interaction is inherited from the nearest parent
    /// </summary>
    Inherited,

    /// <summary>
    /// Gaze interaction is enabled
    /// </summary>
    Enabled,

    /// <summary>
    /// Gaze interaction is disabled
    /// </summary>
    Disabled
};

END_NAMESPACE_GAZE_INPUT
