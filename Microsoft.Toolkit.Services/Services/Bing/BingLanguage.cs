// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Parsers.Core;

namespace Microsoft.Toolkit.Services.Bing
{
    /// <summary>
    /// Country filter for search query.
    /// </summary>
    public enum BingLanguage
    {
        /// <summary>
        /// None
        /// </summary>
        [StringValue("")]
        None,

        /// <summary>
        /// Afar
        /// </summary>
        [StringValue("aa")]
        Afar,

        /// <summary>
        /// Abkhazian
        /// </summary>
        [StringValue("ab")]
        Abkhazian,

        /// <summary>
        /// Avestan
        /// </summary>
        [StringValue("ae")]
        Avestan,

        /// <summary>
        /// Afrikaans
        /// </summary>
        [StringValue("af")]
        Afrikaans,

        /// <summary>
        /// Akan
        /// </summary>
        [StringValue("ak")]
        Akan,

        /// <summary>
        /// Amharic
        /// </summary>
        [StringValue("am")]
        Amharic,

        /// <summary>
        /// Aragonese
        /// </summary>
        [StringValue("an")]
        Aragonese,

        /// <summary>
        /// Arabic
        /// </summary>
        [StringValue("ar")]
        Arabic,

        /// <summary>
        /// Assamese
        /// </summary>
        [StringValue("as")]
        Assamese,

        /// <summary>
        /// Avaric
        /// </summary>
        [StringValue("av")]
        Avaric,

        /// <summary>
        /// Aymara
        /// </summary>
        [StringValue("ay")]
        Aymara,

        /// <summary>
        /// Azerbaijani
        /// </summary>
        [StringValue("az")]
        Azerbaijani,

        /// <summary>
        /// Bashkir
        /// </summary>
        [StringValue("ba")]
        Bashkir,

        /// <summary>
        /// Belarusian
        /// </summary>
        [StringValue("be")]
        Belarusian,

        /// <summary>
        /// Bulgarian
        /// </summary>
        [StringValue("bg")]
        Bulgarian,

        /// <summary>
        /// Bihari
        /// </summary>
        [StringValue("bh")]
        Bihari,

        /// <summary>
        /// Bislama
        /// </summary>
        [StringValue("bi")]
        Bislama,

        /// <summary>
        /// Bambara
        /// </summary>
        [StringValue("bm")]
        Bambara,

        /// <summary>
        /// Bengali
        /// </summary>
        [StringValue("bn")]
        Bengali,

        /// <summary>
        /// Tibetan
        /// </summary>
        [StringValue("bo")]
        Tibetan,

        /// <summary>
        /// Breton
        /// </summary>
        [StringValue("br")]
        Breton,

        /// <summary>
        /// Bosnian
        /// </summary>
        [StringValue("bs")]
        Bosnian,

        /// <summary>
        /// Catalan
        /// </summary>
        [StringValue("ca")]
        Catalan,

        /// <summary>
        /// Chechen
        /// </summary>
        [StringValue("ce")]
        Chechen,

        /// <summary>
        /// Chamorro
        /// </summary>
        [StringValue("ch")]
        Chamorro,

        /// <summary>
        /// Corsican
        /// </summary>
        [StringValue("co")]
        Corsican,

        /// <summary>
        /// Cree
        /// </summary>
        [StringValue("cr")]
        Cree,

        /// <summary>
        /// Czech
        /// </summary>
        [StringValue("cs")]
        Czech,

        /// <summary>
        /// Church Slavic
        /// </summary>
        [StringValue("cu")]
        ChurchSlavic,

        /// <summary>
        /// Chuvash
        /// </summary>
        [StringValue("cv")]
        Chuvash,

        /// <summary>
        /// Welsh
        /// </summary>
        [StringValue("cy")]
        Welsh,

        /// <summary>
        /// Danish
        /// </summary>
        [StringValue("da")]
        Danish,

        /// <summary>
        /// German
        /// </summary>
        [StringValue("de")]
        German,

        /// <summary>
        /// Divehi
        /// </summary>
        [StringValue("dv")]
        Divehi,

        /// <summary>
        /// Dzongkha
        /// </summary>
        [StringValue("dz")]
        Dzongkha,

        /// <summary>
        /// Ewe
        /// </summary>
        [StringValue("ee")]
        Ewe,

        /// <summary>
        /// Greek
        /// </summary>
        [StringValue("el")]
        Greek,

        /// <summary>
        /// English
        /// </summary>
        [StringValue("en")]
        English,

        /// <summary>
        /// Esperanto
        /// </summary>
        [StringValue("eo")]
        Esperanto,

        /// <summary>
        /// Spanish
        /// </summary>
        [StringValue("es")]
        Spanish,

        /// <summary>
        /// Estonian
        /// </summary>
        [StringValue("et")]
        Estonian,

        /// <summary>
        /// Basque
        /// </summary>
        [StringValue("eu")]
        Basque,

        /// <summary>
        /// Persian
        /// </summary>
        [StringValue("fa")]
        Persian,

        /// <summary>
        /// Fulah
        /// </summary>
        [StringValue("ff")]
        Fulah,

        /// <summary>
        /// Finnish
        /// </summary>
        [StringValue("fi")]
        Finnish,

        /// <summary>
        /// Fijian
        /// </summary>
        [StringValue("fj")]
        Fijian,

        /// <summary>
        /// Faroese
        /// </summary>
        [StringValue("fo")]
        Faroese,

        /// <summary>
        /// French
        /// </summary>
        [StringValue("fr")]
        French,

        /// <summary>
        /// Western Frisian
        /// </summary>
        [StringValue("fy")]
        WesternFrisian,

        /// <summary>
        /// Irish
        /// </summary>
        [StringValue("ga")]
        Irish,

        /// <summary>
        /// Scottish Gaelic
        /// </summary>
        [StringValue("gd")]
        ScottishGaelic,

        /// <summary>
        /// Galician
        /// </summary>
        [StringValue("gl")]
        Galician,

        /// <summary>
        /// Guaraní
        /// </summary>
        [StringValue("gn")]
        Guaraní,

        /// <summary>
        /// Gujarati
        /// </summary>
        [StringValue("gu")]
        Gujarati,

        /// <summary>
        /// Manx
        /// </summary>
        [StringValue("gv")]
        Manx,

        /// <summary>
        /// Hausa
        /// </summary>
        [StringValue("ha")]
        Hausa,

        /// <summary>
        /// Hebrew
        /// </summary>
        [StringValue("he")]
        Hebrew,

        /// <summary>
        /// Hindi
        /// </summary>
        [StringValue("hi")]
        Hindi,

        /// <summary>
        /// Hiri Motu
        /// </summary>
        [StringValue("ho")]
        HiriMotu,

        /// <summary>
        /// Croatian
        /// </summary>
        [StringValue("hr")]
        Croatian,

        /// <summary>
        /// Haitian
        /// </summary>
        [StringValue("ht")]
        Haitian,

        /// <summary>
        /// Hungarian
        /// </summary>
        [StringValue("hu")]
        Hungarian,

        /// <summary>
        /// Armenian
        /// </summary>
        [StringValue("hy")]
        Armenian,

        /// <summary>
        /// Herero
        /// </summary>
        [StringValue("hz")]
        Herero,

        /// <summary>
        /// Interlingua (International Auxiliary Language Association)
        /// </summary>
        [StringValue("ia")]
        Interlingua,

        /// <summary>
        /// Indonesian
        /// </summary>
        [StringValue("id")]
        Indonesian,

        /// <summary>
        /// Interlingue
        /// </summary>
        [StringValue("ie")]
        Interlingue,

        /// <summary>
        /// Igbo
        /// </summary>
        [StringValue("ig")]
        Igbo,

        /// <summary>
        /// Yi
        /// </summary>
        [StringValue("ii")]
        Yi,

        /// <summary>
        /// Inupiaq
        /// </summary>
        [StringValue("ik")]
        Inupiaq,

        /// <summary>
        /// Ido
        /// </summary>
        [StringValue("io")]
        Ido,

        /// <summary>
        /// Icelandic
        /// </summary>
        [StringValue("is")]
        Icelandic,

        /// <summary>
        /// Italian
        /// </summary>
        [StringValue("it")]
        Italian,

        /// <summary>
        /// Inuktitut
        /// </summary>
        [StringValue("iu")]
        Inuktitut,

        /// <summary>
        /// Japanese
        /// </summary>
        [StringValue("ja")]
        Japanese,

        /// <summary>
        /// Javanese
        /// </summary>
        [StringValue("jv")]
        Javanese,

        /// <summary>
        /// Georgian
        /// </summary>
        [StringValue("ka")]
        Georgian,

        /// <summary>
        /// Kongo
        /// </summary>
        [StringValue("kg")]
        Kongo,

        /// <summary>
        /// Kikuyu
        /// </summary>
        [StringValue("ki")]
        Kikuyu,

        /// <summary>
        /// Kwanyama
        /// </summary>
        [StringValue("kj")]
        Kwanyama,

        /// <summary>
        /// Kazakh
        /// </summary>
        [StringValue("kk")]
        Kazakh,

        /// <summary>
        /// Kalaallisut
        /// </summary>
        [StringValue("kl")]
        Kalaallisut,

        /// <summary>
        /// Khmer
        /// </summary>
        [StringValue("km")]
        Khmer,

        /// <summary>
        /// Kannada
        /// </summary>
        [StringValue("kn")]
        Kannada,

        /// <summary>
        /// Korean
        /// </summary>
        [StringValue("ko")]
        Korean,

        /// <summary>
        /// Kanuri
        /// </summary>
        [StringValue("kr")]
        Kanuri,

        /// <summary>
        /// Kurdish
        /// </summary>
        [StringValue("ku")]
        Kurdish,

        /// <summary>
        /// Komi
        /// </summary>
        [StringValue("kv")]
        Komi,

        /// <summary>
        /// Cornish
        /// </summary>
        [StringValue("kw")]
        Cornish,

        /// <summary>
        /// Kirghiz
        /// </summary>
        [StringValue("ky")]
        Kirghiz,

        /// <summary>
        /// Latin
        /// </summary>
        [StringValue("la")]
        Latin,

        /// <summary>
        /// Luxembourgish
        /// </summary>
        [StringValue("lb")]
        Luxembourgish,

        /// <summary>
        /// Ganda
        /// </summary>
        [StringValue("lg")]
        Ganda,

        /// <summary>
        /// Limburgish
        /// </summary>
        [StringValue("li")]
        Limburgish,

        /// <summary>
        /// Lingala
        /// </summary>
        [StringValue("ln")]
        Lingala,

        /// <summary>
        /// Lao
        /// </summary>
        [StringValue("lo")]
        Lao,

        /// <summary>
        /// Lithuanian
        /// </summary>
        [StringValue("lt")]
        Lithuanian,

        /// <summary>
        /// Luba-Katanga
        /// </summary>
        [StringValue("lu")]
        LubaKatanga,

        /// <summary>
        /// Latvian
        /// </summary>
        [StringValue("lv")]
        Latvian,

        /// <summary>
        /// Malagasy
        /// </summary>
        [StringValue("mg")]
        Malagasy,

        /// <summary>
        /// Marshallese
        /// </summary>
        [StringValue("mh")]
        Marshallese,

        /// <summary>
        /// Māori
        /// </summary>
        [StringValue("mi")]
        Māori,

        /// <summary>
        /// Macedonian
        /// </summary>
        [StringValue("mk")]
        Macedonian,

        /// <summary>
        /// Malayalam
        /// </summary>
        [StringValue("ml")]
        Malayalam,

        /// <summary>
        /// Mongolian
        /// </summary>
        [StringValue("mn")]
        Mongolian,

        /// <summary>
        /// Marathi
        /// </summary>
        [StringValue("mr")]
        Marathi,

        /// <summary>
        /// Malay
        /// </summary>
        [StringValue("ms")]
        Malay,

        /// <summary>
        /// Maltese
        /// </summary>
        [StringValue("mt")]
        Maltese,

        /// <summary>
        /// Burmese
        /// </summary>
        [StringValue("my")]
        Burmese,

        /// <summary>
        /// Nauru
        /// </summary>
        [StringValue("na")]
        Nauru,

        /// <summary>
        /// Norwegian Bokmål
        /// </summary>
        [StringValue("nb")]
        NorwegianBokmål,

        /// <summary>
        /// North Ndebele
        /// </summary>
        [StringValue("nd")]
        NorthNdebele,

        /// <summary>
        /// Nepali
        /// </summary>
        [StringValue("ne")]
        Nepali,

        /// <summary>
        /// Ndonga
        /// </summary>
        [StringValue("ng")]
        Ndonga,

        /// <summary>
        /// Dutch
        /// </summary>
        [StringValue("nl")]
        Dutch,

        /// <summary>
        /// Norwegian Nynorsk
        /// </summary>
        [StringValue("nn")]
        NorwegianNynorsk,

        /// <summary>
        /// Norwegian
        /// </summary>
        [StringValue("no")]
        Norwegian,

        /// <summary>
        /// South Ndebele
        /// </summary>
        [StringValue("nr")]
        SouthNdebele,

        /// <summary>
        /// Navajo
        /// </summary>
        [StringValue("nv")]
        Navajo,

        /// <summary>
        /// Chichewa
        /// </summary>
        [StringValue("ny")]
        Chichewa,

        /// <summary>
        /// Occitan
        /// </summary>
        [StringValue("oc")]
        Occitan,

        /// <summary>
        /// Ojibwa
        /// </summary>
        [StringValue("oj")]
        Ojibwa,

        /// <summary>
        /// Oromo
        /// </summary>
        [StringValue("om")]
        Oromo,

        /// <summary>
        /// Odia
        /// </summary>
        [StringValue("or")]
        Odia,

        /// <summary>
        /// Ossetian
        /// </summary>
        [StringValue("os")]
        Ossetian,

        /// <summary>
        /// Panjabi
        /// </summary>
        [StringValue("pa")]
        Panjabi,

        /// <summary>
        /// Pāli
        /// </summary>
        [StringValue("pi")]
        Pāli,

        /// <summary>
        /// Polish
        /// </summary>
        [StringValue("pl")]
        Polish,

        /// <summary>
        /// Pashto
        /// </summary>
        [StringValue("ps")]
        Pashto,

        /// <summary>
        /// Portuguese
        /// </summary>
        [StringValue("pt")]
        Portuguese,

        /// <summary>
        /// Quechua
        /// </summary>
        [StringValue("qu")]
        Quechua,

        /// <summary>
        /// Raeto-Romance
        /// </summary>
        [StringValue("rm")]
        RaetoRomance,

        /// <summary>
        /// Kirundi
        /// </summary>
        [StringValue("rn")]
        Kirundi,

        /// <summary>
        /// Romanian
        /// </summary>
        [StringValue("ro")]
        Romanian,

        /// <summary>
        /// Russian
        /// </summary>
        [StringValue("ru")]
        Russian,

        /// <summary>
        /// Kinyarwanda
        /// </summary>
        [StringValue("rw")]
        Kinyarwanda,

        /// <summary>
        /// Sanskrit
        /// </summary>
        [StringValue("sa")]
        Sanskrit,

        /// <summary>
        /// Sardinian
        /// </summary>
        [StringValue("sc")]
        Sardinian,

        /// <summary>
        /// Sindhi
        /// </summary>
        [StringValue("sd")]
        Sindhi,

        /// <summary>
        /// Northern Sami
        /// </summary>
        [StringValue("se")]
        NorthernSami,

        /// <summary>
        /// Sango
        /// </summary>
        [StringValue("sg")]
        Sango,

        /// <summary>
        /// Serbian, Croation, Bosnian, Montenegrin
        /// </summary>
        [StringValue("sh")]
        SerboCroatian,

        /// <summary>
        /// Sinhala
        /// </summary>
        [StringValue("si")]
        Sinhala,

        /// <summary>
        /// Slovak
        /// </summary>
        [StringValue("sk")]
        Slovak,

        /// <summary>
        /// Slovenian
        /// </summary>
        [StringValue("sl")]
        Slovenian,

        /// <summary>
        /// Samoan
        /// </summary>
        [StringValue("sm")]
        Samoan,

        /// <summary>
        /// Shona
        /// </summary>
        [StringValue("sn")]
        Shona,

        /// <summary>
        /// Somali
        /// </summary>
        [StringValue("so")]
        Somali,

        /// <summary>
        /// Albanian
        /// </summary>
        [StringValue("sq")]
        Albanian,

        /// <summary>
        /// Serbian
        /// </summary>
        [StringValue("sr")]
        Serbian,

        /// <summary>
        /// Swati
        /// </summary>
        [StringValue("ss")]
        Swati,

        /// <summary>
        /// Southern Sotho
        /// </summary>
        [StringValue("st")]
        SouthernSotho,

        /// <summary>
        /// Sundanese
        /// </summary>
        [StringValue("su")]
        Sundanese,

        /// <summary>
        /// Swedish
        /// </summary>
        [StringValue("sv")]
        Swedish,

        /// <summary>
        /// Swahili
        /// </summary>
        [StringValue("sw")]
        Swahili,

        /// <summary>
        /// Tamil
        /// </summary>
        [StringValue("ta")]
        Tamil,

        /// <summary>
        /// Telugu
        /// </summary>
        [StringValue("te")]
        Telugu,

        /// <summary>
        /// Tajik
        /// </summary>
        [StringValue("tg")]
        Tajik,

        /// <summary>
        /// Thai
        /// </summary>
        [StringValue("th")]
        Thai,

        /// <summary>
        /// Tigrinya
        /// </summary>
        [StringValue("ti")]
        Tigrinya,

        /// <summary>
        /// Turkmen
        /// </summary>
        [StringValue("tk")]
        Turkmen,

        /// <summary>
        /// Tagalog
        /// </summary>
        [StringValue("tl")]
        Tagalog,

        /// <summary>
        /// Tswana
        /// </summary>
        [StringValue("tn")]
        Tswana,

        /// <summary>
        /// Tonga
        /// </summary>
        [StringValue("to")]
        Tonga,

        /// <summary>
        /// Turkish
        /// </summary>
        [StringValue("tr")]
        Turkish,

        /// <summary>
        /// Tsonga
        /// </summary>
        [StringValue("ts")]
        Tsonga,

        /// <summary>
        /// Tatar
        /// </summary>
        [StringValue("tt")]
        Tatar,

        /// <summary>
        /// Twi
        /// </summary>
        [StringValue("tw")]
        Twi,

        /// <summary>
        /// Tahitian
        /// </summary>
        [StringValue("ty")]
        Tahitian,

        /// <summary>
        /// Uighur
        /// </summary>
        [StringValue("ug")]
        Uighur,

        /// <summary>
        /// Ukrainian
        /// </summary>
        [StringValue("uk")]
        Ukrainian,

        /// <summary>
        /// Urdu
        /// </summary>
        [StringValue("ur")]
        Urdu,

        /// <summary>
        /// Uzbek
        /// </summary>
        [StringValue("uz")]
        Uzbek,

        /// <summary>
        /// Venda
        /// </summary>
        [StringValue("ve")]
        Venda,

        /// <summary>
        /// Vietnamese
        /// </summary>
        [StringValue("vi")]
        Vietnamese,

        /// <summary>
        /// Volapük
        /// </summary>
        [StringValue("vo")]
        Volapük,

        /// <summary>
        /// Walloon
        /// </summary>
        [StringValue("wa")]
        Walloon,

        /// <summary>
        /// Wolof
        /// </summary>
        [StringValue("wo")]
        Wolof,

        /// <summary>
        /// Xhosa
        /// </summary>
        [StringValue("xh")]
        Xhosa,

        /// <summary>
        /// Yiddish
        /// </summary>
        [StringValue("yi")]
        Yiddish,

        /// <summary>
        /// Yoruba
        /// </summary>
        [StringValue("yo")]
        Yoruba,

        /// <summary>
        /// Zhuang
        /// </summary>
        [StringValue("za")]
        Zhuang,

        /// <summary>
        /// Chinese
        /// </summary>
        [StringValue("zh")]
        Chinese,

        /// <summary>
        /// Zulu
        /// </summary>
        [StringValue("zu")]
        Zulu
    }
}
