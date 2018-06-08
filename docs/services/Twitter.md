---
title: Twitter Service
author: nmetulev
description: The Twitter Service allows users to retrieve or publish data to Twitter. 
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Twitter 
dev_langs:
  - csharp
  - vb
---

# Twitter Service

The **Twitter Service** allows users to retrieve or publish data to Twitter. 

[Twitter Developer Site](https://dev.twitter.com) is the main content site for all twitter developers.  Visit the [Twitter Apps List](https://apps.twitter.com/) to manage existing apps.

[Create new Twitter App](https://apps.twitter.com/app) can be used to create a new app within the Twitter portal.

## App Setup

**Consumer Key**
Copy this from the *Keys and Access Tokens* tab on your application page. 

**Consumer Secret**
Copy this from the *Keys and Access Tokens* tab on your application page. 

**Callback URI** Enter a unique URI for your application.  This must match the *Callback URL* field on the *Application Details* tab in Twitter.
*Example*: http://myapp.company.com - (this does not have to be a working URL)

## Overview

In the code section below the GetUserTimeLineAsync method returns some Tweet objects.  The Tweet class returns some basic information along with the tweet text itself.

| Property | Type | Description |
| -- | -- | -- |
| **CreatedAt** | string | The date and time of the Tweet formatted by Twitter |
| **Text** | string | The text of the Tweet (if retweet, the text might not be complete - use the RetweetedStatus object for the original tweet)|
| **Id** | string | The Twitter status identifier |
| **GeoData** | TwitterGeoData | A class containing the latitude and longitude of the Tweet |
| **User** | TwitterUser | A class containing the user ID, Name, ScreenName, and ProfileImageUrl |
| **RetweetedStatus** | Tweet | if this tweet is a retweet, this object will contain the original tweet |

## Syntax

```csharp
// Initialize service
TwitterService.Instance.Initialize(ConsumerKey.Text, ConsumerSecret.Text, CallbackUri.Text);

// Login to Twitter
if (!await TwitterService.Instance.LoginAsync())
{
    return;
}

// Get current user info
var user = await TwitterService.Instance.GetUserAsync();
ProfileImage.DataContext = user;

// Get user time line
ListView.ItemsSource = await TwitterService.Instance.GetUserTimeLineAsync(user.ScreenName, 50);

// Post a tweet
await TwitterService.Instance.TweetStatusAsync(TweetText.Text);

var status = new TwitterStatus
			{
				Message = TweetText.Text,

				// Optional parameters defined by the Twitter "update" API (they may all be null or false)

				DisplayCoordinates = true,
				InReplyToStatusId = "@ValidAccount",
				Latitude = validLatitude,
				Longitude = validLongitude,
				PlaceId = "df51dec6f4ee2b2c",	// As defined by Twitter
				PossiblySensitive = true,		// As defined by Twitter (nudity, violence, or medical procedures)
				TrimUser = true
			}

await TwitterService.Instance.TweetStatusAsync(status);

// Post a tweet with a picture
await TwitterService.Instance.TweetStatusAsync(TweetText.Text, stream);

await TwitterService.Instance.TweetStatusAsync(status, stream);

// Search for a specific tag
ListView.ItemsSource = await TwitterService.Instance.SearchAsync(TagText.Text, 50);

// Open a connection with the stream service in order to receive live tweets and events
ListView.ItemsSource = _tweets;
await TwitterService.Instance.StartUserStreamAsync(async tweet =>
{
    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
    {
        if (tweet != null)
        {
		_tweets.Insert(0, tweet);
        }
    });
});

// Stop receiving live tweets and events
TwitterService.Instance.StopUserStream();
```
```vb
' Initialize service
TwitterService.Instance.Initialize(ConsumerKey.Text, ConsumerSecret.Text, CallbackUri.Text)

' Login to Twitter
If Not Await TwitterService.Instance.LoginAsync() Then
    Return
End If

' Get current user info
Dim user = Await TwitterService.Instance.GetUserAsync()
ProfileImage.DataContext = user

' Get user time line
ListView.ItemsSource = Await TwitterService.Instance.GetUserTimeLineAsync(user.ScreenName, 50)

' Post a tweet
Await TwitterService.Instance.TweetStatusAsync(TweetText.Text)
Dim status = New TwitterStatus With {
    .Message = TweetText.Text,

    ' Optional parameters defined by the Twitter "update" API (they may all be null or false)

    .DisplayCoordinates = True,
    .InReplyToStatusId = "@ValidAccount",
    .Latitude = validLatitude,
    .Longitude = validLongitude,
    .PlaceId = "df51dec6f4ee2b2c",  ' As defined by Twitter
    .PossiblySensitive = True,      ' As defined by Twitter (nudity, violence, or medical procedures)
    .TrimUser = True
}
Await TwitterService.Instance.TweetStatusAsync(status)

' Post a tweet with a picture
Await TwitterService.Instance.TweetStatusAsync(TweetText.Text, stream)
Await TwitterService.Instance.TweetStatusAsync(status, stream)

' Search for a specific tag
ListView.ItemsSource = Await TwitterService.Instance.SearchAsync(TagText.Text, 50)

' Open a connection with the stream service in order to receive live tweets and events
ListView.ItemsSource = _tweets
Await TwitterService.Instance.StartUserStreamAsync(
    Async Sub(tweet)
        Await Dispatcher.RunAsync(
        CoreDispatcherPriority.Normal,
        Sub()
            If tweet IsNot Nothing Then
                _tweets.Insert(0, tweet)
            End If
        End Sub)
    End Sub)

' Stop receiving live tweets and events
TwitterService.Instance.StopUserStream()
```

## Posting to timeline fails to appear

Twitter app models allows for read only applications.  If the app is tagged as Readonly, but attempts to post there is *no error returned*.  The post is just eaten by the service.

If you are posting from your app and never seeing them show up in the timeline check the *Permissions* tab on the app page.  You want to ensure that you have *Read and Write* checked on that tab.

## Sample Code

[Twitter Service Sample Page Source](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Twitter%20Service). You can see this in action in [Windows Community Toolkit Sample App](https://www.microsoft.com/store/apps/9NBLGGH4TLCQ).

## Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.Services |
| NuGet package | [Microsoft.Toolkit.Uwp.Services](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.Services/) |

## API

* [Twitter Service source code](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp.Services/Services/Twitter)
