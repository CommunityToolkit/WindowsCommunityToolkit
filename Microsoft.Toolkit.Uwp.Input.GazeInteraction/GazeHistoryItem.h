//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

using namespace winrt::Windows::Foundation;

BEGIN_NAMESPACE_GAZE_INPUT

class GazeTargetItem;

struct GazeHistoryItem
{
    GazeTargetItem HitTarget;
    TimeSpan Timestamp;
    TimeSpan Duration;
};

END_NAMESPACE_GAZE_INPUT