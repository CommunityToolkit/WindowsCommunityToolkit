// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

namespace Microsoft.Toolkit.Uwp.Services.Bing
{
    using System;
    using Microsoft.Toolkit.Uwp.Services.Core;

    /// <summary>
    /// Country filter for search query.
    /// </summary>
    [Obsolete("This class is being deprecated. Please use the .NET Standard Library counterpart found in Microsoft.Toolkit.Services.")]
    public enum BingCountry
    {
        /// <summary>
        /// None
        /// </summary>
        [StringValue("")]
        None,

        /// <summary>
        /// United Arab Emirates
        /// </summary>
        [StringValue("ae")]
        UnitedArabEmirates,

        /// <summary>
        /// Albania
        /// </summary>
        [StringValue("al")]
        Albania,

        /// <summary>
        /// Armenia
        /// </summary>
        [StringValue("am")]
        Armenia,

        /// <summary>
        /// Argentina
        /// </summary>
        [StringValue("ar")]
        Argentina,

        /// <summary>
        /// Austria
        /// </summary>
        [StringValue("at")]
        Austria,

        /// <summary>
        /// Australia
        /// </summary>
        [StringValue("au")]
        Australia,

        /// <summary>
        /// Azerbaijan
        /// </summary>
        [StringValue("az")]
        Azerbaijan,

        /// <summary>
        /// Bosnia Herzegovina
        /// </summary>
        [StringValue("ba")]
        BosniaHerzegovina,

        /// <summary>
        /// Belgium
        /// </summary>
        [StringValue("be")]
        Belgium,

        /// <summary>
        /// Bulgaria
        /// </summary>
        [StringValue("bg")]
        Bulgaria,

        /// <summary>
        /// Bahrain
        /// </summary>
        [StringValue("bh")]
        Bahrain,

        /// <summary>
        /// Bolivia
        /// </summary>
        [StringValue("bo")]
        Bolivia,

        /// <summary>
        /// Brazil
        /// </summary>
        [StringValue("br")]
        Brazil,

        /// <summary>
        /// Canada
        /// </summary>
        [StringValue("ca")]
        Canada,

        /// <summary>
        /// Switzerland
        /// </summary>
        [StringValue("ch")]
        Switzerland,

        /// <summary>
        /// Chile
        /// </summary>
        [StringValue("cl")]
        Chile,

        /// <summary>
        /// China
        /// </summary>
        [StringValue("cn")]
        China,

        /// <summary>
        /// Costa Rica
        /// </summary>
        [StringValue("cr")]
        CostaRica,

        /// <summary>
        /// Germany
        /// </summary>
        [StringValue("de")]
        Germany,

        /// <summary>
        /// Dominican Republic
        /// </summary>
        [StringValue("do")]
        DominicanRepublic,

        /// <summary>
        /// Ecuador
        /// </summary>
        [StringValue("ec")]
        Ecuador,

        /// <summary>
        /// France
        /// </summary>
        [StringValue("fr")]
        France,

        /// <summary>
        /// United Kingdom
        /// </summary>
        [StringValue("uk")]
        UnitedKingdom,

        /// <summary>
        /// Georgia
        /// </summary>
        [StringValue("ge")]
        Georgia,

        /// <summary>
        /// Greece
        /// </summary>
        [StringValue("gr")]
        Greece,

        /// <summary>
        /// Guatemala
        /// </summary>
        [StringValue("gt")]
        Guatemala,

        /// <summary>
        /// Hong Kong
        /// </summary>
        [StringValue("hk")]
        HongKong,

        /// <summary>
        /// Honduras
        /// </summary>
        [StringValue("hn")]
        Honduras,

        /// <summary>
        /// Croatia
        /// </summary>
        [StringValue("hr")]
        Croatia,

        /// <summary>
        /// Hungary
        /// </summary>
        [StringValue("hu")]
        Hungary,

        /// <summary>
        /// Indonesia
        /// </summary>
        [StringValue("id")]
        Indonesia,

        /// <summary>
        /// Ireland
        /// </summary>
        [StringValue("ie")]
        Ireland,

        /// <summary>
        /// Israel
        /// </summary>
        [StringValue("i")]
        Israel,

        /// <summary>
        /// India
        /// </summary>
        [StringValue("in")]
        India,

        /// <summary>
        /// Iraq
        /// </summary>
        [StringValue("iq")]
        Iraq,

        /// <summary>
        /// Iran
        /// </summary>
        [StringValue("ir")]
        Iran,

        /// <summary>
        /// Iceland
        /// </summary>
        [StringValue("is")]
        Iceland,

        /// <summary>
        /// Italy
        /// </summary>
        [StringValue("it")]
        Italy,

        /// <summary>
        /// Jordan
        /// </summary>
        [StringValue("jo")]
        Jordan,

        /// <summary>
        /// Japan
        /// </summary>
        [StringValue("jp")]
        Japan,

        /// <summary>
        /// Kenya
        /// </summary>
        [StringValue("ke")]
        Kenya,

        /// <summary>
        /// Korea
        /// </summary>
        [StringValue("kr")]
        Korea,

        /// <summary>
        /// Kuwait
        /// </summary>
        [StringValue("kw")]
        Kuwait,

        /// <summary>
        /// Lebanon
        /// </summary>
        [StringValue("lb")]
        Lebanon,

        /// <summary>
        /// Lithuania
        /// </summary>
        [StringValue("lt")]
        Lithuania,

        /// <summary>
        /// Latvia
        /// </summary>
        [StringValue("lv")]
        Latvia,

        /// <summary>
        /// Luxembourg
        /// </summary>
        [StringValue("lu")]
        Luxembourg,

        /// <summary>
        /// Libya
        /// </summary>
        [StringValue("ly")]
        Libya,

        /// <summary>
        /// Morocco
        /// </summary>
        [StringValue("ma")]
        Morocco,

        /// <summary>
        /// Macedonia
        /// </summary>
        [StringValue("mk")]
        Macedonia,

        /// <summary>
        /// Malta
        /// </summary>
        [StringValue("mt")]
        Malta,

        /// <summary>
        /// Malaysia
        /// </summary>
        [StringValue("my")]
        Malaysia,

        /// <summary>
        /// Mexico
        /// </summary>
        [StringValue("mx")]
        Mexico,

        /// <summary>
        /// Nicaragua
        /// </summary>
        [StringValue("ni")]
        Nicaragua,

        /// <summary>
        /// Netherlands
        /// </summary>
        [StringValue("nl")]
        Netherlands,

        /// <summary>
        /// New Zealand
        /// </summary>
        [StringValue("nz")]
        NewZealand,

        /// <summary>
        /// Norway
        /// </summary>
        [StringValue("no")]
        Norway,

        /// <summary>
        /// Oman
        /// </summary>
        [StringValue("om")]
        Oman,

        /// <summary>
        /// Panama
        /// </summary>
        [StringValue("pa")]
        Panama,

        /// <summary>
        /// Peru
        /// </summary>
        [StringValue("pe")]
        Peru,

        /// <summary>
        /// Philippines
        /// </summary>
        [StringValue("ph")]
        Philippines,

        /// <summary>
        /// Poland
        /// </summary>
        [StringValue("pl")]
        Poland,

        /// <summary>
        /// Pakistan
        /// </summary>
        [StringValue("pk")]
        Pakistan,

        /// <summary>
        /// PuertoRico
        /// </summary>
        [StringValue("pr")]
        PuertoRico,

        /// <summary>
        /// Portugal
        /// </summary>
        [StringValue("ru")]
        Portugal,

        /// <summary>
        /// Paraguay
        /// </summary>
        [StringValue("py")]
        Paraguay,

        /// <summary>
        /// Qatar
        /// </summary>
        [StringValue("qa")]
        Qatar,

        /// <summary>
        /// Romania
        /// </summary>
        [StringValue("ro")]
        Romania,

        /// <summary>
        /// Russia
        /// </summary>
        [StringValue("ru")]
        Russia,

        /// <summary>
        /// Sweden
        /// </summary>
        [StringValue("se")]
        Sweden,

        /// <summary>
        /// Saudi Arabia
        /// </summary>
        [StringValue("sa")]
        SaudiArabia,

        /// <summary>
        /// Singapore
        /// </summary>
        [StringValue("sg")]
        Singapore,

        /// <summary>
        /// Slovakia
        /// </summary>
        [StringValue("sk")]
        Slovakia,

        /// <summary>
        /// Slovenia
        /// </summary>
        [StringValue("sl")]
        Slovenia,

        /// <summary>
        /// Spain
        /// </summary>
        [StringValue("es")]
        Spain,

        /// <summary>
        /// Serbia
        /// </summary>
        [StringValue("sp")]
        Serbia,

        /// <summary>
        /// El Salvador
        /// </summary>
        [StringValue("sv")]
        ElSalvador,

        /// <summary>
        /// Syria
        /// </summary>
        [StringValue("sy")]
        Syria,

        /// <summary>
        /// Taiwan
        /// </summary>
        [StringValue("tw")]
        Taiwan,

        /// <summary>
        /// Thailand
        /// </summary>
        [StringValue("th")]
        Thailand,

        /// <summary>
        /// Tunisia
        /// </summary>
        [StringValue("tn")]
        Tunisia,

        /// <summary>
        /// Turkey
        /// </summary>
        [StringValue("tr")]
        Turkey,

        /// <summary>
        /// Ukraine
        /// </summary>
        [StringValue("ua")]
        Ukraine,

        /// <summary>
        /// United States
        /// </summary>
        [StringValue("us")]
        UnitedStates,

        /// <summary>
        /// Vietnam
        /// </summary>
        [StringValue("vn")]
        Vietnam,

        /// <summary>
        /// Yemen
        /// </summary>
        [StringValue("ye")]
        Yemen,

        /// <summary>
        /// South Africa
        /// </summary>
        [StringValue("za")]
        SouthAfrica,
    }
}
