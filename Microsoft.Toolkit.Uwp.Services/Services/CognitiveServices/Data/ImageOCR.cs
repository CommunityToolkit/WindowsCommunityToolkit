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

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.CognitiveServices
{
    /// <summary>
    /// Represents the ocr details
    /// </summary>
    public class ImageOCR
    {
        /// <summary>
        /// Gets or sets OCR Language
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets The angle, in degrees, of the detected text with respect to the closest horizontal or vertical direction. After rotating the input image clockwise by this angle, the recognized text lines become horizontal or vertical. In combination with the orientation property it can be used to overlay recognition results correctly on the original image, by rotating either the original image or recognition results by a suitable angle around the center of the original image. If the angle cannot be confidently detected, this property is not present. If the image contains text at different angles, only part of the text will be recognized correctly. 
        /// </summary>
        [JsonProperty("textAngle")]
        public float TextAngle { get; set; }

        /// <summary>
        /// Gets or sets image Orientation
        /// </summary>
        [JsonProperty("orientation")]
        public string Orientation { get; set; }

        /// <summary>
        /// Gets or sets OCR Regions
        /// </summary>
        [JsonProperty("regions")]
        public Region[] Regions { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return VisionServiceJsonHelper.JsonSerialize(this);
        }
    }

    /// <summary>
    /// Represents a region of recognized text. A region consists of multiple lines (e.g. a column of text in a multi-column document).
    /// </summary>
    public class Region
    {
        /// <summary>
        /// Gets or sets Bounding box of a recognized region, line, or word, depending on the parent object. The four integers represent the x-coordinate of the left edge, the y-coordinate of the top edge, width, and height of the bounding box, in the coordinate system of the input image, after it has been rotated around its center according to the detected text angle (see textAngle property), with the origin at the top-left corner, and the y-axis pointing down.
        /// </summary>
        [JsonProperty("boundingBox")]
        public string BoundingBox { get; set; }

        /// <summary>
        /// Gets or sets An array of objects, where each object represents a line of recognized text.
        /// </summary>
        [JsonProperty("lines")]
        public Line[] Lines { get; set; }
    }

    /// <summary>
    /// Represents a line of recognized text.
    /// </summary>
    public class Line
    {
        /// <summary>
        /// Gets or sets Bounding box of a recognized region, line, or word, depending on the parent object. The four integers represent the x-coordinate of the left edge, the y-coordinate of the top edge, width, and height of the bounding box, in the coordinate system of the input image, after it has been rotated around its center according to the detected text angle (see textAngle property), with the origin at the top-left corner, and the y-axis pointing down.
        /// </summary>
        [JsonProperty("boundingBox")]
        public string BoundingBox { get; set; }

        /// <summary>
        /// Gets or sets OCR words
        /// </summary>
        [JsonProperty("words")]
        public Word[] Words { get; set; }
    }

    /// <summary>
    /// OCR Words
    /// </summary>
    public class Word
    {
        /// <summary>
        /// Gets or sets Bounding box of a recognized region, line, or word, depending on the parent object. The four integers represent the x-coordinate of the left edge, the y-coordinate of the top edge, width, and height of the bounding box, in the coordinate system of the input image, after it has been rotated around its center according to the detected text angle (see textAngle property), with the origin at the top-left corner, and the y-axis pointing down.
        /// </summary>
        [JsonProperty("boundingBox")]
        public string BoundingBox { get; set; }

        /// <summary>
        /// Gets or sets OCR Text
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
