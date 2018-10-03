// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Toolkit.Uwp.Notifications
{
#if !WINRT
    public partial class TileContentBuilder
    {
        public static TileBindingContentContact CreateContactTileContent(Uri contactImageUri, string contactName, string contactImageAltText = default(string), bool? contactImageAddImageQuery = default(bool?), string textLanguage = default(string))
        {
            var contactTileContent = new TileBindingContentContact();
            contactTileContent.Image = CreateTileBasicImage(contactImageUri, contactImageAltText, contactImageAddImageQuery);

            contactTileContent.Text = new TileBasicText();
            contactTileContent.Text.Text = contactName;

            if (textLanguage != default(string))
            {
                contactTileContent.Text.Lang = textLanguage;
            }

            return contactTileContent;
        }

        public static TileBindingContentIconic CreateIconicTileContent(Uri iconImageUri, string iconImageAltText = default(string), bool? iconImageAddImageQuery = default(bool?))
        {
            var iconicTileContent = new TileBindingContentIconic();
            iconicTileContent.Icon = CreateTileBasicImage(iconImageUri, iconImageAltText, iconImageAddImageQuery);

            return iconicTileContent;
        }

        public static TileBindingContentPeople CreatePeopleTileContent(params Uri[] peoplePictureSources)
        {
            IEnumerable<TileBasicImage> images = peoplePictureSources.Select(u => CreateTileBasicImage(u, default(string), default(bool?)));

            return CreatePeopleTileContent(images);
        }

        public static TileBindingContentPeople CreatePeopleTileContent(params (Uri source, string imageAltText, bool? addImageQuery)[] peoplePictures)
        {
            IEnumerable<TileBasicImage> images = peoplePictures.Select(t => CreateTileBasicImage(t.source, t.imageAltText, t.addImageQuery));

            return CreatePeopleTileContent(images);
        }

        public static TileBindingContentPeople CreatePeopleTileContent(IEnumerable<TileBasicImage> peoplePictures)
        {
            var peopleTileContent = new TileBindingContentPeople();

            foreach (var image in peoplePictures)
            {
                peopleTileContent.Images.Add(image);
            }

            return peopleTileContent;
        }

        public static TileBindingContentPhotos CreatePhotosTileContent(params Uri[] photoSources)
        {
            IEnumerable<TileBasicImage> images = photoSources.Select(u => CreateTileBasicImage(u, default(string), default(bool?)));

            return CreatePhotosTileContent(images);
        }

        public static TileBindingContentPhotos CreatePhotosTileContent(params (Uri source, string imageAltText, bool? addImageQuery)[] photos)
        {
            IEnumerable<TileBasicImage> images = photos.Select(t => CreateTileBasicImage(t.source, t.imageAltText, t.addImageQuery));

            return CreatePhotosTileContent(images);
        }

        public static TileBindingContentPhotos CreatePhotosTileContent(IEnumerable<TileBasicImage> photos)
        {
            var photoTileContent = new TileBindingContentPhotos();

            foreach (var image in photos)
            {
                photoTileContent.Images.Add(image);
            }

            return photoTileContent;
        }

        private static TileBasicImage CreateTileBasicImage(Uri imageUri, string imageAltText, bool? addImageQuery)
        {
            var tileImage = new TileBasicImage();
            tileImage.Source = imageUri.OriginalString;

            if (imageAltText != default(string))
            {
                tileImage.AlternateText = imageAltText;
            }

            if (addImageQuery != default(bool?))
            {
                tileImage.AddImageQuery = addImageQuery;
            }

            return tileImage;
        }
    }
#endif
}
