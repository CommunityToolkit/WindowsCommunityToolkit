namespace Microsoft.Windows.Toolkit.Notifications
{
    using Microsoft.Windows.Toolkit.Notifications.Adaptive.Elements;

    /// <summary>
    /// A full-bleed background image that appears beneath the Tile content.
    /// </summary>
    public sealed class TileBackgroundImage : IBaseImage
    {
        private string _source;

        /// <summary>
        /// The URI of the image. Can be from your application package, application data, or the internet. Internet images must be less than 200 KB in size.
        /// </summary>
        public string Source
        {
            get { return this._source; }
            set { BaseImageHelper.SetSource(ref this._source, value); }
        }

        /// <summary>
        /// A description of the image, for users of assistive technologies.
        /// </summary>
        public string AlternateText { get; set; }

        /// <summary>
        /// Set to true to allow Windows to append a query string to the image URI supplied in the Tile notification. Use this attribute if your server hosts images and can handle query strings, either by retrieving an image variant based on the query strings or by ignoring the query string and returning the image as specified without the query string. This query string specifies scale, contrast setting, and language.
        /// </summary>
        public bool? AddImageQuery { get; set; }

        private int? _hintOverlay;

        /// <summary>
        /// A black overlay on the background image. This value controls the opacity of the black overlay, with 0 being no overlay and 100 being completely black. Defaults to 20.
        /// </summary>
        public int? HintOverlay
        {
            get
            {
                return this._hintOverlay;
            }

            set
            {
                if (value != null)
                {
                    Element_TileBinding.CheckOverlayValue(value.Value);
                }

                this._hintOverlay = value;
            }
        }

        /// <summary>
        /// New in 1511: Control the desired cropping of the image.
        /// Previously for RTM: Did not exist, value will be ignored and background image will be displayed without any cropping.
        /// </summary>
        public TileBackgroundImageCrop HintCrop { get; set; }

        internal Element_AdaptiveImage ConvertToElement()
        {
            Element_AdaptiveImage image = BaseImageHelper.CreateBaseElement(this);

            image.Placement = AdaptiveImagePlacement.Background;
            image.Crop = this.GetAdaptiveImageCrop();
            image.Overlay = this.HintOverlay;

            return image;
        }

        private AdaptiveImageCrop GetAdaptiveImageCrop()
        {
            switch (this.HintCrop)
            {
                case TileBackgroundImageCrop.Circle:
                    return AdaptiveImageCrop.Circle;

                case TileBackgroundImageCrop.None:
                    return AdaptiveImageCrop.None;

                default:
                    return AdaptiveImageCrop.Default;
            }
        }
    }
}