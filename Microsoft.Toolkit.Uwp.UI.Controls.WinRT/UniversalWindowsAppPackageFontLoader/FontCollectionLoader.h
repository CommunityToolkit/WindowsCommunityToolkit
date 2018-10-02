#pragma once
#include "UniversalPackageFontData.h"

BEGIN_NAMESPACE_CONTROLS_WINRT

namespace UniversalWindowsAppPackageFontLoader
{
    /// <summary>
    /// This is just to glue together a custom font file in the Universal Windows app package
    /// to a custom DWrite Font. All it does is provide an enumerator that returns 1 font (the custom font)
    /// and doesn't support things like font fallbacks.
    /// </summary>
    struct FontCollectionLoader : winrt::implements<FontCollectionLoader, IDWriteFontCollectionLoader>
    {
    public:
        FontCollectionLoader();
        virtual ~FontCollectionLoader();

        /// <summary>
        /// Create the enumerator to the Universal Windows Application package.
        /// </summary>
        IFACEMETHOD(CreateEnumeratorFromKey)(
            _In_ IDWriteFactory* factory,
            void const* collectionKey,                      // XAML FontFamily Syntax (something.ttf)#font
            unsigned int collectionKeySize,
            _Outptr_ IDWriteFontFileEnumerator** fontFileEnumerator
            );

        /// <summary>
        /// This sees if the incoming XAML FontFamily value has something that looks like
        /// a custom font file like foo.ttf#bar
        /// </summary>
        static bool HasCustomFontFamily(Platform::String^ xamlFontFamily);

        /// <summary>
        /// This parses something that looks like /foo/bar.ttf#baz into the ttf path and the font name (baz).
        /// </summary>
        static void ParseXamlFontFamily(Platform::String^ xamlFontFamily, _Out_ UniversalPackageFontData& parsedFont);

        /// <summary>
        /// Get the singleton loader.
        /// </summary>
        static winrt::com_ptr<IDWriteFontCollectionLoader>& GetInstance();

    private:
        struct FontEnumeratorEntry
        {
            Platform::String^ customFont;
            winrt::com_ptr<IDWriteFontFileEnumerator> enumerator;
        };

        // enumerators are cached due to memory usages.
        bool FindCachedEnumerator(Platform::String^ xamlFontFamily, winrt::com_ptr<IDWriteFontFileEnumerator>& enumerator);

        std::vector<FontEnumeratorEntry> m_fontEnumerators;
        static winrt::com_ptr<IDWriteFontCollectionLoader> s_comInstance;
    };
}

END_NAMESPACE_CONTROLS_WINRT