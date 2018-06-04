// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Microsoft.Toolkit.Parsers.Rss
{
    /// <summary>
    /// Class with utilities for Rss related works.
    /// </summary>
    internal static class RssHelper
    {
        /// <summary>
        /// String for regular expression for image pattern.
        /// </summary>
        private const string ImagePattern = @"<img.*?src=[\""'](.+?)[\""'].*?>";

        /// <summary>
        /// String for regular xpression for hyperlink pattern.
        /// </summary>
        private const string HiperlinkPattern = @"<a\s+(?:[^>]*?\s+)?href=""([^ ""]*)""";

        /// <summary>
        /// String for regular expression for height pattern.
        /// </summary>
        private const string HeightPattern = @"height=(?:(['""])(?<height>(?:(?!\1).)*)\1|(?<height>\S+))";

        /// <summary>
        /// String for regular expression for width pattern.
        /// </summary>
        private const string WidthPattern = @"width=(?:(['""])(?<width>(?:(?!\1).)*)\1|(?<width>\S+))";

        /// <summary>
        /// Regular expression for image pattern.
        /// </summary>
        private static readonly Regex RegexImages = new Regex(ImagePattern, RegexOptions.IgnoreCase);

        /// <summary>
        /// Regular expression for hyperlink pattern.
        /// </summary>
        private static readonly Regex RegexLinks = new Regex(HiperlinkPattern, RegexOptions.IgnoreCase);

        /// <summary>
        /// Regular expression for height pattern.
        /// </summary>
        private static readonly Regex RegexHeight = new Regex(HeightPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        /// Regular expression for width pattern.
        /// </summary>
        private static readonly Regex RegexWidth = new Regex(WidthPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        /// Removes \t characters in the string and trim additional space and carriage returns.
        /// </summary>
        /// <param name="text">Text string.</param>
        /// <returns>Sanitized string.</returns>
        public static string SanitizeString(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var textArray = text.Split(new[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
            string sanitizedText = string.Empty;
            foreach (var item in textArray.ToList())
            {
                sanitizedText += item.Trim();
            }

            sanitizedText = string.Join(" ", Regex.Split(sanitizedText, @"(?:\r\n|\n|\r)"));

            return sanitizedText;
        }

        /// <summary>
        /// Get item date from xelement and element name.
        /// </summary>
        /// <param name="item">XElement item.</param>
        /// <param name="elementName">Name of element.</param>
        /// <returns>Item date.</returns>
        public static DateTime GetSafeElementDate(this XElement item, string elementName)
        {
            return GetSafeElementDate(item, elementName, item.GetDefaultNamespace());
        }

        /// <summary>
        /// Get item date from xelement, element name and namespace.
        /// </summary>
        /// <param name="item">XElement item.</param>
        /// <param name="elementName">Name of element.</param>
        /// <param name="xNamespace">XNamespace namespace.</param>
        /// <returns>Item date.</returns>
        public static DateTime GetSafeElementDate(this XElement item, string elementName, XNamespace xNamespace)
        {
            DateTime date;
            XElement element = item.Element(xNamespace + elementName);
            if (element == null)
            {
                return DateTime.Now;
            }

            if (TryParseDateTime(element.Value, out date))
            {
                return date;
            }

            return DateTime.Now;
        }

        /// <summary>
        /// Get item string value for xelement and element name.
        /// </summary>
        /// <param name="item">XElement item.</param>
        /// <param name="elementName">Name of element.</param>
        /// <returns>Safe string.</returns>
        public static string GetSafeElementString(this XElement item, string elementName)
        {
            if (item == null)
            {
                return string.Empty;
            }

            return GetSafeElementString(item, elementName, item.GetDefaultNamespace());
        }

        /// <summary>
        /// Get item string values for xelement and element name.
        /// </summary>
        /// <param name="item">XElement item.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <returns>Safe list of string values.</returns>
        public static IEnumerable<string> GetSafeElementsString(this XElement item, string elementName)
        {
            return GetSafeElementsString(item, elementName, item.GetDefaultNamespace());
        }

        /// <summary>
        /// Get item string values for xelement, element name and namespace.
        /// </summary>
        /// <param name="item">XELement item.</param>
        /// <param name="elementName">Name of element.</param>
        /// <param name="xNamespace">XNamespace namespace.</param>
        /// <returns>Safe list of string values.</returns>
        public static IEnumerable<string> GetSafeElementsString(this XElement item, string elementName, XNamespace xNamespace)
        {
            if (item != null)
            {
                IEnumerable<XElement> values = item.Elements(xNamespace + elementName);
                return values.Where(f => !string.IsNullOrEmpty(f.Value))
                    .Select(f => f.Value);
            }

            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Get item string value for xelement, element name and namespace.
        /// </summary>
        /// <param name="item">XElement item.</param>
        /// <param name="elementName">Name of element.</param>
        /// <param name="xNamespace">XNamespace namespace.</param>
        /// <returns>Safe string.</returns>
        public static string GetSafeElementString(this XElement item, string elementName, XNamespace xNamespace)
        {
            if (item == null)
            {
                return string.Empty;
            }

            XElement value = item.Element(xNamespace + elementName);
            if (value != null)
            {
                return value.Value;
            }

            return string.Empty;
        }

        /// <summary>
        /// Get feed url to see full original information.
        /// </summary>
        /// <param name="item">XElement item.</param>
        /// <param name="rel">rel attribute value.</param>
        /// <returns>String link.</returns>
        public static string GetLink(this XElement item, string rel)
        {
            IEnumerable<XElement> links = item.Elements(item.GetDefaultNamespace() + "link");
            var xElements = links as XElement[] ?? links.ToArray();
            IEnumerable<string> link = from l in xElements
                                       let xAttribute = l.Attribute("rel")
                                       where xAttribute != null && xAttribute.Value == rel
                                       let attribute = l.Attribute("href")
                                       where attribute != null
                                       select attribute.Value;
            var enumerable = link as string[] ?? link.ToArray();
            if (!enumerable.Any() && xElements.Any())
            {
                return xElements.FirstOrDefault().Attributes().First().Value;
            }

            return enumerable.FirstOrDefault();
        }

        /// <summary>
        /// Get feed image.
        /// </summary>
        /// <param name="item">XElement item.</param>
        /// <returns>Feed data image.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "The general catch is intended to avoid breaking the Data Provider by a Html decode exception")]
        public static string GetImage(this XElement item)
        {
            string feedDataImage = null;
            try
            {
                feedDataImage = GetImagesInHTMLString(item.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(feedDataImage) && feedDataImage.EndsWith("'"))
                {
                    feedDataImage = feedDataImage.Remove(feedDataImage.Length - 1);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return feedDataImage;
        }

        /// <summary>
        /// Get the item image from the enclosure element http://www.w3schools.com/rss/rss_tag_enclosure.asp
        /// </summary>
        /// <param name="item">XElement item.</param>
        /// <returns>Feed data image.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "The general catch is intended to avoid breaking the Data Provider by a Html decode exception")]
        public static string GetImageFromEnclosure(this XElement item)
        {
            string feedDataImage = null;
            try
            {
                XElement element = item.Element(item.GetDefaultNamespace() + "enclosure");
                if (element == null)
                {
                    return string.Empty;
                }

                var typeAttribute = element.Attribute("type");
                if (!string.IsNullOrEmpty(typeAttribute?.Value) && typeAttribute.Value.StartsWith("image"))
                {
                    var urlAttribute = element.Attribute("url");
                    feedDataImage = (!string.IsNullOrEmpty(urlAttribute?.Value)) ?
                        urlAttribute.Value : string.Empty;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return feedDataImage;
        }

        /// <summary>
        /// Tries to parse the original string to a datetime format.
        /// </summary>
        /// <param name="s">Input string.</param>
        /// <param name="result">Parsed datetime.</param>
        /// <returns>True if success</returns>
        public static bool TryParseDateTime(string s, out DateTime result)
        {
            if (DateTime.TryParse(s, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowWhiteSpaces, out result))
            {
                return true;
            }

            int tzIndex = s.LastIndexOf(" ");
            if (tzIndex >= 0)
            {
                string tz = s.Substring(tzIndex, s.Length - tzIndex);
                string offset = TimeZoneToOffset(tz);
                if (offset != null)
                {
                    string offsetDate = string.Format("{0} {1}", s.Substring(0, tzIndex), offset);
                    return TryParseDateTime(offsetDate, out result);
                }
            }

            result = default(DateTime);
            return false;
        }

        /// <summary>
        /// Calculate and return timezone.
        /// </summary>
        /// <param name="tz">Input string.</param>
        /// <returns>Parsed timezone.</returns>
        public static string TimeZoneToOffset(string tz)
        {
            if (tz == null)
            {
                return null;
            }

            tz = tz.ToUpper().Trim();

            if (timeZones.ContainsKey(tz))
            {
                return timeZones[tz].First();
            }

            return null;
        }

        /// <summary>
        /// Retrieve images from HTML string.
        /// </summary>
        /// <param name="htmlString">String of HTML.</param>
        /// <returns>List of images.</returns>
        private static IEnumerable<string> GetImagesInHTMLString(string htmlString)
        {
            var images = new List<string>();
            foreach (Match match in RegexImages.Matches(htmlString))
            {
                bool include = true;
                string tag = match.Value;

                // Ignores images with low size
                var matchHeight = RegexHeight.Match(tag);
                if (matchHeight.Success)
                {
                    var heightValue = matchHeight.Groups["height"].Value;
                    int size = 0;
                    if (int.TryParse(heightValue, out size) && size < 10)
                    {
                        include = false;
                    }
                }

                var matchWidth = RegexWidth.Match(tag);
                if (matchWidth.Success)
                {
                    var widthValue = matchWidth.Groups["width"].Value;
                    int size = 0;
                    if (int.TryParse(widthValue, out size) && size < 10)
                    {
                        include = false;
                    }
                }

                if (include)
                {
                    images.Add(match.Groups[1].Value);
                }
            }

            foreach (Match match in RegexLinks.Matches(htmlString))
            {
                var value = match.Groups[1].Value;
                if (value.Contains(".jpg") || value.Contains(".png"))
                {
                    images.Add(value);
                }
            }

            return images;
        }

        /// <summary>
        /// Dictionary of timezones.
        /// </summary>
        private static Dictionary<string, string[]> timeZones = new Dictionary<string, string[]>
        {
            { "ACDT", new[] { "-1030", "Australian Central Daylight" } },
            { "ACST", new[] { "-0930", "Australian Central Standard" } },
            { "ADT", new[] { "+0300", "(US) Atlantic Daylight" } },
            { "AEDT", new[] { "-1100", "Australian East Daylight" } },
            { "AEST", new[] { "-1000", "Australian East Standard" } },
            { "AHDT", new[] { "+0900", string.Empty } },
            { "AHST", new[] { "+1000", string.Empty } },
            { "AST", new[] { "+0400", "(US) Atlantic Standard" } },
            { "AT", new[] { "+0200", "Azores" } },
            { "AWDT", new[] { "-0900", "Australian West Daylight" } },
            { "AWST", new[] { "-0800", "Australian West Standard" } },
            { "BAT", new[] { "-0300", "Bhagdad" } },
            { "BDST", new[] { "-0200", "British Double Summer" } },
            { "BET", new[] { "+1100", "Bering Standard" } },
            { "BST", new[] { "+0300", "Brazil Standard" } },
            { "BT", new[] { "-0300", "Baghdad" } },
            { "BZT2", new[] { "+0300", "Brazil Zone 2" } },
            { "CADT", new[] { "-1030", "Central Australian Daylight" } },
            { "CAST", new[] { "-0930", "Central Australian Standard" } },
            { "CAT", new[] { "+1000", "Central Alaska" } },
            { "CCT", new[] { "-0800", "China Coast" } },
            { "CDT", new[] { "+0500", "(US) Central Daylight" } },
            { "CED", new[] { "-0200", "Central European Daylight" } },
            { "CET", new[] { "-0100", "Central European" } },
            { "CST", new[] { "+0600", "(US) Central Standard" } },
            { "EAST", new[] { "-1000", "Eastern Australian Standard" } },
            { "EDT", new[] { "+0400", "(US) Eastern Daylight" } },
            { "EED", new[] { "-0300", "Eastern European Daylight" } },
            { "EET", new[] { "-0200", "Eastern Europe" } },
            { "EEST", new[] { "-0300", "Eastern Europe Summer" } },
            { "EST", new[] { "+0500", "(US) Eastern Standard" } },
            { "FST", new[] { "-0200", "French Summer" } },
            { "FWT", new[] { "-0100", "French Winter" } },
            { "GMT", new[] { "+0000", "Greenwich Mean" } },
            { "GST", new[] { "-1000", "Guam Standard" } },
            { "HDT", new[] { "+0900", "Hawaii Daylight" } },
            { "HST", new[] { "+1000", "Hawaii Standard" } },
            { "IDLE", new[] { "-1200", "Internation Date Line East" } },
            { "IDLW", new[] { "+1200", "Internation Date Line West" } },
            { "IST", new[] { "-0530", "Indian Standard" } },
            { "IT", new[] { "-0330", "Iran" } },
            { "JST", new[] { "-0900", "Japan Standard" } },
            { "JT", new[] { "-0700", "Java" } },
            { "MDT", new[] { "+0600", "(US) Mountain Daylight" } },
            { "MED", new[] { "-0200", "Middle European Daylight" } },
            { "MET", new[] { "-0100", "Middle European" } },
            { "MEST", new[] { "-0200", "Middle European Summer" } },
            { "MEWT", new[] { "-0100", "Middle European Winter" } },
            { "MST", new[] { "+0700", "(US) Mountain Standard" } },
            { "MT", new[] { "-0800", "Moluccas" } },
            { "NDT", new[] { "+0230", "Newfoundland Daylight" } },
            { "NFT", new[] { "+0330", "Newfoundland" } },
            { "NT", new[] { "+1100", "Nome" } },
            { "NST", new[] { "-0630", "North Sumatra" } },
            { "NZ", new[] { "-1100", "New Zealand " } },
            { "NZST", new[] { "-1200", "New Zealand Standard" } },
            { "NZDT", new[] { "-1300", "New Zealand Daylight" } },
            { "NZT", new[] { "-1200", "New Zealand" } },
            { "PDT", new[] { "+0700", "(US) Pacific Daylight" } },
            { "PST", new[] { "+0800", "(US) Pacific Standard" } },
            { "ROK", new[] { "-0900", "Republic of Korea" } },
            { "SAD", new[] { "-1000", "South Australia Daylight" } },
            { "SAST", new[] { "-0900", "South Australia Standard" } },
            { "SAT", new[] { "-0900", "South Australia Standard" } },
            { "SDT", new[] { "-1000", "South Australia Daylight" } },
            { "SST", new[] { "-0200", "Swedish Summer" } },
            { "SWT", new[] { "-0100", "Swedish Winter" } },
            { "USZ3", new[] { "-0400", "Volga Time (Russia)" } },
            { "USZ4", new[] { "-0500", "Ural Time (Russia)" } },
            { "USZ5", new[] { "-0600", "West-Siberian Time (Russia) " } },
            { "USZ6", new[] { "-0700", "Yenisei Time (Russia)" } },
            { "UT", new[] { "+0000", "Universal Coordinated" } },
            { "UTC", new[] { "+0000", "Universal Coordinated" } },
            { "UZ10", new[] { "-1100", "Okhotsk Time (Russia)" } },
            { "WAT", new[] { "+0100", "West Africa" } },
            { "WET", new[] { "+0000", "West European" } },
            { "WST", new[] { "-0800", "West Australian Standard" } },
            { "YDT", new[] { "+0800", "Yukon Daylight" } },
            { "YST", new[] { "+0900", "Yukon Standard" } }
        };
    }
}