#pragma once
#include "UniversalPackageFontData.h"

namespace winrt::Microsoft_Toolkit_Uwp_UI_Controls_WinRT::implementation::UniversalWindowsAppPackageFontLoader
{
    /// <summary>
    /// This is an enumerator that basically just enumerates 1 custom font into DWrite from the Universal Windows App package.
    /// It's extremely simplistic and basically just connects the Universal App Package files to DWrite.
    /// </summary>
    struct FontFileEnumerator : winrt::implements<FontFileEnumerator, IDWriteFontFileEnumerator>
    {
    public:
        FontFileEnumerator();
        virtual ~FontFileEnumerator();

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
        HRESULT Initialize(_In_ IDWriteFactory* factory, const UniversalPackageFontData& packageFont);

    private:
        winrt::com_ptr<IDWriteFactory> m_factory;
        winrt::com_ptr<IDWriteFontFile> m_currentFontFile;
        UniversalPackageFontData m_packageFontArgs = {};
        bool m_enumerated = false;
    };
}