//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

#pragma once

BEGIN_NAMESPACE_GAZE_INPUT

using namespace Windows::Foundation::Collections;

/// <summary>
/// A helper class to read a ValueSet and retrieve relevant settings
/// </summary>
public ref class GazeSettingsHelper sealed
{
public:

    /// <summary>
    /// Retrieves settings as a ValueSet from a shared store.
    /// </summary>
    static Windows::Foundation::IAsyncAction^ RetrieveSharedSettings(ValueSet^ settings);

private:
    GazeSettingsHelper();
};

END_NAMESPACE_GAZE_INPUT
