using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using AppStudio.DataProviders.Core;

namespace AppStudio.DataProviders.Rss
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rss")]
    public class RssParser : IParser<RssSchema>
    {
        public IEnumerable<RssSchema> Parse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var doc = XDocument.Parse(data);
            var type = BaseRssParser.GetFeedType(doc);

            BaseRssParser rssParser;
            if (type == RssType.Rss)
            {
                rssParser = new Rss2Parser();
            }
            else
            {
                rssParser = new AtomParser();
            }

            return rssParser.LoadFeed(doc);
        }
    }

    internal abstract class BaseRssParser
    {
        /// <summary>
        /// Get the feed type: Rss, Atom or Unknown
        /// </summary>
        /// <param name="rss"></param>
        /// <returns></returns>
        public static RssType GetFeedType(XDocument doc)
        {
            if (doc.Root == null)
            {
                //AppLogs.WriteError("AtomReader.LoadFeed", "Not supported type");
                return RssType.Unknown;
            }
            XNamespace defaultNamespace = doc.Root.GetDefaultNamespace();
            return defaultNamespace.NamespaceName.EndsWith("Atom") ? RssType.Atom : RssType.Rss;
        }

        /// <summary>
        /// Abstract method to be override by specific implementations of the reader.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public abstract IEnumerable<RssSchema> LoadFeed(XDocument doc);

        internal protected static string ProcessHtmlContent(string htmlContent)
        {
            return htmlContent.FixHtml().SanitizeString();
        }

        internal protected static string ProcessHtmlSummary(string htmlContent)
        {
            return htmlContent.DecodeHtml().Trim().Truncate(500).SanitizeString();
        }
    }

    /// <summary>
    /// Rss reader implementation to parse Rss content.
    /// </summary>
    internal class Rss2Parser : BaseRssParser
    {
        private static readonly XNamespace NsRdfNamespaceUri = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
        private static readonly XNamespace NsRdfElementsNamespaceUri = "http://purl.org/dc/elements/1.1/";
        private static readonly XNamespace NsRdfContentNamespaceUri = "http://purl.org/rss/1.0/modules/content/";

        /// <summary>
        /// THis override load and parses the document and return a list of RssSchema values.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public override IEnumerable<RssSchema> LoadFeed(XDocument doc)
        {
            bool isRDF = false;
            var feed = new Collection<RssSchema>();
            XNamespace defaultNamespace = string.Empty;

            if (doc.Root != null)
            {
                isRDF = doc.Root.Name == (NsRdfNamespaceUri + "RDF");
                defaultNamespace = doc.Root.GetDefaultNamespace();
            }

            foreach (var item in doc.Descendants(defaultNamespace + "item"))
            {
                var rssItem = isRDF ? ParseRDFItem(item) : ParseRssItem(item);
                feed.Add(rssItem);
            }
            return feed;
        }

        /// <summary>
        /// RSS all versions
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static RssSchema ParseItem(XElement item)
        {
            var rssItem = new RssSchema();
            rssItem.Title = item.GetSafeElementString("title").Trim().DecodeHtml();
            rssItem.FeedUrl = item.GetSafeElementString("link");

            rssItem.Author = GetItemAuthor(item);

            string content = item.GetSafeElementString("encoded", NsRdfContentNamespaceUri);
            if (string.IsNullOrEmpty(content))
            {
                content = item.GetSafeElementString("description");
                if (string.IsNullOrEmpty(content))
                {
                    content = item.GetSafeElementString("content");
                }
            }

            var summary = item.GetSafeElementString("description");
            if (string.IsNullOrEmpty(summary))
            {
                summary = item.GetSafeElementString("encoded", NsRdfContentNamespaceUri);
            }

            //Removes scripts from html
            if (!string.IsNullOrEmpty(summary))
            {
                rssItem.Summary = ProcessHtmlSummary(summary);
            }

            if (!string.IsNullOrEmpty(content))
            {
                rssItem.Content = ProcessHtmlContent(content);
            }

            string id = item.GetSafeElementString("guid").Trim();
            if (string.IsNullOrEmpty(id))
            {
                id = item.GetSafeElementString("id").Trim();
                if (string.IsNullOrEmpty(id))
                {
                    id = rssItem.FeedUrl;
                }
            }
            rssItem._id = id;

            return rssItem;
        }

        /// <summary>
        /// RSS version 1.0
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static RssSchema ParseRDFItem(XElement item)
        {
            XNamespace ns = "http://search.yahoo.com/mrss/";
            var rssItem = ParseItem(item);

            rssItem.PublishDate = item.GetSafeElementDate("date", NsRdfElementsNamespaceUri);

            string image = item.GetSafeElementString("image");
            if (string.IsNullOrEmpty(image) && item.Elements(ns + "thumbnail").LastOrDefault() != null)
            {
                var element = item.Elements(ns + "thumbnail").Last();
                image = element.Attribute("url").Value;
            }
            if (string.IsNullOrEmpty(image) && item.ToString().Contains("thumbnail"))
            {
                image = item.GetSafeElementString("thumbnail");
            }
            if (string.IsNullOrEmpty(image))
            {
                image = item.GetImage();
            }

            rssItem.ImageUrl = image;

            return rssItem;
        }

        /// <summary>
        /// RSS version 2.0
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static RssSchema ParseRssItem(XElement item)
        {
            XNamespace ns = "http://search.yahoo.com/mrss/";
            var rssItem = ParseItem(item);

            rssItem.PublishDate = item.GetSafeElementDate("pubDate");

            string image = item.GetSafeElementString("image");
            if (string.IsNullOrEmpty(image))
            {
                image = item.GetImageFromEnclosure();
            }
            if (string.IsNullOrEmpty(image) && item.Elements(ns + "content").LastOrDefault() != null)
            {
                var element = item.Elements(ns + "content").Last();
                if (element.Attribute("type") != null && element.Attribute("type").Value.Contains("image/"))
                {
                    image = element.Attribute("url").Value;
                }
            }
            if (string.IsNullOrEmpty(image) && item.Elements(ns + "thumbnail").LastOrDefault() != null)
            {
                var element = item.Elements(ns + "thumbnail").Last();
                image = element.Attribute("url").Value;
            }
            if (string.IsNullOrEmpty(image) && item.ToString().Contains("thumbnail"))
            {
                image = item.GetSafeElementString("thumbnail");
            }
            if (string.IsNullOrEmpty(image))
            {
                image = item.GetImage();
            }

            rssItem.ImageUrl = image;

            return rssItem;
        }

        private static string GetItemAuthor(XElement item)
        {
            var content = item.GetSafeElementString("creator", NsRdfElementsNamespaceUri).Trim();
            if (string.IsNullOrEmpty(content))
            {
                content = item.GetSafeElementString("author");
            }

            return content;
        }
    }

    internal class AtomParser : BaseRssParser
    {
        /// <summary>
        /// Atom reader implementation to parse Atom content.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public override IEnumerable<RssSchema> LoadFeed(XDocument doc)
        {
            Collection<RssSchema> feed = new Collection<RssSchema>();

            if (doc.Root == null)
            {
                return feed;
            }

            var items = doc.Root.Elements(doc.Root.GetDefaultNamespace() + "entry").Select(item => GetRssSchema(item)).ToList<RssSchema>();

            feed = new Collection<RssSchema>(items);

            return feed;
        }

        private static RssSchema GetRssSchema(XElement item)
        {
            RssSchema rssItem = new RssSchema
            {
                Author = GetItemAuthor(item),
                Title = item.GetSafeElementString("title").Trim().DecodeHtml(),
                ImageUrl = GetItemImage(item),
                PublishDate = item.GetSafeElementDate("published"),
                FeedUrl = item.GetLink("alternate"),
            };

            var content = GetItemContent(item);

            //Removes scripts from html
            if (!string.IsNullOrEmpty(content))
            {
                rssItem.Summary = ProcessHtmlSummary(content);
                rssItem.Content = ProcessHtmlContent(content);
            }

            string id = item.GetSafeElementString("guid").Trim();
            if (string.IsNullOrEmpty(id))
            {
                id = item.GetSafeElementString("id").Trim();
                if (string.IsNullOrEmpty(id))
                {
                    id = rssItem.FeedUrl;
                }
            }
            rssItem._id = id;
            return rssItem;
        }

        private static string GetItemAuthor(XElement item)
        {
            var content = string.Empty;

            if (item != null && item.Element(item.GetDefaultNamespace() + "author") != null)
            {
                content = item.Element(item.GetDefaultNamespace() + "author").GetSafeElementString("name");
            }

            if (string.IsNullOrEmpty(content))
            {
                content = item.GetSafeElementString("author");
            }

            return content;
        }

        private static string GetItemImage(XElement item)
        {
            if (!string.IsNullOrEmpty(item.GetSafeElementString("image")))
                return item.GetSafeElementString("image");

            return item.GetImage();
        }

        private static string GetItemContent(XElement item)
        {
            var content = item.GetSafeElementString("description");
            if (string.IsNullOrEmpty(content))
            {
                content = item.GetSafeElementString("content");
            }
            if (string.IsNullOrEmpty(content))
            {
                content = item.GetSafeElementString("summary");
            }

            return content;
        }
    }

    /// <summary>
    /// Class with utilities for Rss related works.
    /// </summary>
    internal static class RssHelper
    {
        private const string ImagePattern = @"<img.*?src=[\""'](.+?)[\""'].*?>";
        private const string HiperlinkPattern = @"<a\s+(?:[^>]*?\s+)?href=""([^ ""]*)""";
        private const string HeightPattern = @"height=(?:(['""])(?<height>(?:(?!\1).)*)\1|(?<height>\S+))";
        private const string WidthPattern = @"width=(?:(['""])(?<width>(?:(?!\1).)*)\1|(?<width>\S+))";

        private static readonly Regex RegexImages = new Regex(ImagePattern, RegexOptions.IgnoreCase);
        private static readonly Regex RegexLinks = new Regex(HiperlinkPattern, RegexOptions.IgnoreCase);
        private static readonly Regex RegexHeight = new Regex(HeightPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex RegexWidth = new Regex(WidthPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        /// Removes \t characters in the string and trim additional space and carriage returns.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SanitizeString(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var textArray = text.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
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
        /// <param name="item"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static DateTime GetSafeElementDate(this XElement item, string elementName)
        {
            return GetSafeElementDate(item, elementName, item.GetDefaultNamespace());
        }

        /// <summary>
        /// Get item date from xelement, element name and namespace.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
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
        /// <param name="item"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static string GetSafeElementString(this XElement item, string elementName)
        {
            if (item == null) return string.Empty;
            return GetSafeElementString(item, elementName, item.GetDefaultNamespace());
        }

        /// <summary>
        /// Get item string value for xelement, element name and namespace.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static string GetSafeElementString(this XElement item, string elementName, XNamespace xNamespace)
        {
            if (item == null) return String.Empty;
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
        /// <param name="item"></param>
        /// <param name="rel"></param>
        /// <returns></returns>
        public static string GetLink(this XElement item, string rel)
        {
            IEnumerable<XElement> links = item.Elements(item.GetDefaultNamespace() + "link");
            IEnumerable<string> link = from l in links
                                       let xAttribute = l.Attribute("rel")
                                       where xAttribute != null && xAttribute.Value == rel
                                       let attribute = l.Attribute("href")
                                       where attribute != null
                                       select attribute.Value;
            if (!link.Any() && links.Any())
            {
                return links.FirstOrDefault().Attributes().First().Value;
            }

            return link.FirstOrDefault();
        }

        /// <summary>
        /// Get feed image.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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
        /// <param name="item"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "The general catch is intended to avoid breaking the Data Provider by a Html decode exception")]
        public static string GetImageFromEnclosure(this XElement item)
        {
            string feedDataImage = null;
            try
            {
                XElement element = item.Element(item.GetDefaultNamespace() + "enclosure");
                if (element == null)
                    return string.Empty;

                var typeAttribute = element.Attribute("type");
                if (typeAttribute != null && !string.IsNullOrEmpty(typeAttribute.Value) &&
                    typeAttribute.Value.StartsWith("image"))
                {
                    var urlAttribute = element.Attribute("url");
                    feedDataImage = (urlAttribute != null && !string.IsNullOrEmpty(urlAttribute.Value)) ?
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
        /// <param name="s"></param>
        /// <param name="result"></param>
        /// <returns></returns>
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
        /// <param name="tz"></param>
        /// <returns></returns>
        public static string TimeZoneToOffset(string tz)
        {
            if (tz == null)
                return null;

            tz = tz.ToUpper().Trim();

            if (TimeZones.ContainsKey(tz))
                return TimeZones[tz].First();
            else
                return null;
        }

        private static IEnumerable<string> GetImagesInHTMLString(string htmlString)
        {
            var images = new List<string>();
            foreach (Match match in RegexImages.Matches(htmlString))
            {
                bool include = true;
                string tag = match.Value;

                //Ignores images with low size
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

        private static Dictionary<string, string[]> TimeZones = new Dictionary<string, string[]>
        {
            {"ACDT", new string[] { "-1030", "Australian Central Daylight" }},
            {"ACST", new string[] { "-0930", "Australian Central Standard"}},
            {"ADT", new string[] { "+0300", "(US) Atlantic Daylight"}},
            {"AEDT", new string[] { "-1100", "Australian East Daylight"}},
            {"AEST", new string[] { "-1000", "Australian East Standard"}},
            {"AHDT", new string[] { "+0900", ""}},
            {"AHST", new string[] { "+1000", ""}},
            {"AST", new string[] { "+0400", "(US) Atlantic Standard"}},
            {"AT", new string[] { "+0200", "Azores"}},
            {"AWDT", new string[] { "-0900", "Australian West Daylight"}},
            {"AWST", new string[] { "-0800", "Australian West Standard"}},
            {"BAT", new string[] { "-0300", "Bhagdad"}},
            {"BDST", new string[] { "-0200", "British Double Summer"}},
            {"BET", new string[] { "+1100", "Bering Standard"}},
            {"BST", new string[] { "+0300", "Brazil Standard"}},
            {"BT", new string[] { "-0300", "Baghdad"}},
            {"BZT2", new string[] { "+0300", "Brazil Zone 2"}},
            {"CADT", new string[] { "-1030", "Central Australian Daylight"}},
            {"CAST", new string[] { "-0930", "Central Australian Standard"}},
            {"CAT", new string[] { "+1000", "Central Alaska"}},
            {"CCT", new string[] { "-0800", "China Coast"}},
            {"CDT", new string[] { "+0500", "(US) Central Daylight"}},
            {"CED", new string[] { "-0200", "Central European Daylight"}},
            {"CET", new string[] { "-0100", "Central European"}},
            {"CST", new string[] { "+0600", "(US) Central Standard"}},
            {"EAST", new string[] { "-1000", "Eastern Australian Standard"}},
            {"EDT", new string[] { "+0400", "(US) Eastern Daylight"}},
            {"EED", new string[] { "-0300", "Eastern European Daylight"}},
            {"EET", new string[] { "-0200", "Eastern Europe"}},
            {"EEST", new string[] { "-0300", "Eastern Europe Summer"}},
            {"EST", new string[] { "+0500", "(US) Eastern Standard"}},
            {"FST", new string[] { "-0200", "French Summer"}},
            {"FWT", new string[] { "-0100", "French Winter"}},
            {"GMT", new string[] { "+0000", "Greenwich Mean"}},
            {"GST", new string[] { "-1000", "Guam Standard"}},
            {"HDT", new string[] { "+0900", "Hawaii Daylight"}},
            {"HST", new string[] { "+1000", "Hawaii Standard"}},
            {"IDLE", new string[] { "-1200", "Internation Date Line East"}},
            {"IDLW", new string[] { "+1200", "Internation Date Line West"}},
            {"IST", new string[] { "-0530", "Indian Standard"}},
            {"IT", new string[] { "-0330", "Iran"}},
            {"JST", new string[] { "-0900", "Japan Standard"}},
            {"JT", new string[] { "-0700", "Java"}},
            {"MDT", new string[] { "+0600", "(US) Mountain Daylight"}},
            {"MED", new string[] { "-0200", "Middle European Daylight"}},
            {"MET", new string[] { "-0100", "Middle European"}},
            {"MEST", new string[] { "-0200", "Middle European Summer"}},
            {"MEWT", new string[] { "-0100", "Middle European Winter"}},
            {"MST", new string[] { "+0700", "(US) Mountain Standard"}},
            {"MT", new string[] { "-0800", "Moluccas"}},
            {"NDT", new string[] { "+0230", "Newfoundland Daylight"}},
            {"NFT", new string[] { "+0330", "Newfoundland"}},
            {"NT", new string[] { "+1100", "Nome"}},
            {"NST", new string[] { "-0630", "North Sumatra"}},
            {"NZ", new string[] { "-1100", "New Zealand "}},
            {"NZST", new string[] { "-1200", "New Zealand Standard"}},
            {"NZDT", new string[] { "-1300", "New Zealand Daylight"}},
            {"NZT", new string[] { "-1200", "New Zealand"}},
            {"PDT", new string[] { "+0700", "(US) Pacific Daylight"}},
            {"PST", new string[] { "+0800", "(US) Pacific Standard"}},
            {"ROK", new string[] { "-0900", "Republic of Korea"}},
            {"SAD", new string[] { "-1000", "South Australia Daylight"}},
            {"SAST", new string[] { "-0900", "South Australia Standard"}},
            {"SAT", new string[] { "-0900", "South Australia Standard"}},
            {"SDT", new string[] { "-1000", "South Australia Daylight"}},
            {"SST", new string[] { "-0200", "Swedish Summer"}},
            {"SWT", new string[] { "-0100", "Swedish Winter"}},
            {"USZ3", new string[] { "-0400", "USSR Zone 3"}},
            {"USZ4", new string[] { "-0500", "USSR Zone 4"}},
            {"USZ5", new string[] { "-0600", "USSR Zone 5"}},
            {"USZ6", new string[] { "-0700", "USSR Zone 6"}},
            {"UT", new string[] { "+0000", "Universal Coordinated"}},
            {"UTC", new string[] { "+0000", "Universal Coordinated"}},
            {"UZ10", new string[] { "-1100", "USSR Zone 10"}},
            {"WAT", new string[] { "+0100", "West Africa"}},
            {"WET", new string[] { "+0000", "West European"}},
            {"WST", new string[] { "-0800", "West Australian Standard"}},
            {"YDT", new string[] { "+0800", "Yukon Daylight"}},
            {"YST", new string[] { "+0900", "Yukon Standard"}},
            {"ZP4", new string[] { "-0400", "USSR Zone 3"}},
            {"ZP5", new string[] { "-0500", "USSR Zone 4"}},
            {"ZP6", new string[] { "-0600", "USSR Zone 5"}}
        };
    }

    /// <summary>
    /// Type of Rss.
    /// </summary>
    internal enum RssType
    {
        Atom,
        Rss,
        Unknown
    }

}
