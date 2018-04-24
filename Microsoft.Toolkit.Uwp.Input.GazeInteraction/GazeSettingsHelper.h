//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

BEGIN_NAMESPACE_GAZE_INPUT

using namespace Windows::Foundation::Collections;

public ref class GazeSettingsHelper sealed
{
public:
    virtual ~GazeSettingsHelper();

    static Windows::Foundation::IAsyncAction^ RetrieveSharedSettings(ValueSet^ settings);

private:
    GazeSettingsHelper();
};

END_NAMESPACE_GAZE_INPUT
