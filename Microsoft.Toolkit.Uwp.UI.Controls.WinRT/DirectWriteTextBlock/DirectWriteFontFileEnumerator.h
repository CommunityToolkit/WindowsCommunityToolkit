// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma once
#include "DirectWriteUniversalPackageFontData.h"

BEGIN_NAMESPACE_CONTROLS_WINRT

/// <summary>
/// This is an enumerator that basically just enumerates 1 custom font into DWrite from the Universal Windows App package.
/// It's extremely simplistic and basically just connects the Universal App Package files to DWrite.
/// </summary>
struct DirectWriteFontFileEnumerator : winrt::implements<DirectWriteFontFileEnumerator, IDWriteFontFileEnumerator>
{
public:
    DirectWriteFontFileEnumerator();
    virtual ~DirectWriteFontFileEnumerator();

    /// <summary>
    /// This is called consecutively by DWrite until we return FALSE to hasCurrentFile to enumerate the custom
    /// font
    /// </summary>
    IFACEMETHOD(MoveNext)(_Out_ BOOL* hasCurrentFile);

    /// <summary>
    /// This is called by DWrite to get the custom font file.
    /// </summary>
    IFACEMETHOD(GetCurrentFontFile)(_Outptr_ IDWriteFontFile** fontFile);

    /// <summary>
    /// Initializes the enumerator
    /// </summary>
    HRESULT Initialize(_In_ IDWriteFactory* factory, const DirectWriteUniversalPackageFontData& packageFont);

private:
    winrt::com_ptr<IDWriteFactory> m_factory;
    winrt::com_ptr<IDWriteFontFile> m_currentFontFile;
    DirectWriteUniversalPackageFontData m_packageFontArgs = {};
    bool m_enumerated = false;
};

END_NAMESPACE_CONTROLS_WINRT