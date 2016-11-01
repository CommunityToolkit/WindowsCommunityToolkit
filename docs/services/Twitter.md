# Twitter Service

The **Twitter Service** allows users to retrieve or publish data to Twitter. 

[Twitter Developer Site](https://dev.twitter.com) is the main content site for all twitter developers.  Visit the [Twitter Apps List](https://apps.twitter.com/) to manage existing apps.

[Create new Twitter App](https://apps.twitter.com/app/new) can be used to create a new app within the Twitter portal.

## App Setup

**Consumer Key**
Copy this from the *Keys and Access Tokens* tab on your application page. 

**Consumer Secret**
Copy this from the *Keys and Access Tokens* tab on your application page. 

**Callback URI** Enter a unique URI for your application.  This must match the *Callback URL* field on the *Application Details* tab in Twitter.
*Example*: http://myapp.company.com - (this does not have to be a working URL)

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

// Get user timeline
ListView.ItemsSource = await TwitterService.Instance.GetUserTimeLineAsync(user.ScreenName, 50);

// Post a tweet
await TwitterService.Instance.TweetStatusAsync(TweetText.Text);

// Post a tweet with a picture
await TwitterService.Instance.TweetStatusAsync(TweetText.Text, stream);

// Search for a specific tag
ListView.ItemsSource = await TwitterService.Instance.SearchAsync(TagText.Text, 50);

```

## Example

[Twitter Service Sample Page](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.SampleApp/SamplePages/Twitter%20Service)

## Posting to timeline fails to appear

Twitter app models allows for read only applications.  If the app is tagged as Readonly, but attempts to post there is *no error returned*.  The post is just eaten by the service.

If you are posting from your app and never seeing them show up in the timeline check the *Permissions* tab on the app page.  You want to ensure that you have *Read and Write* checked on that tab.

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.10586.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.Services |

## API

* [Twitter Service source code](https://github.com/Microsoft/UWPCommunityToolkit/tree/master/Microsoft.Toolkit.Uwp.Services/Services/Twitter)

## NuGet Packages Required

Microsoft.Toolkit.Uwp.Services

See the [NuGet Packages page](../Nuget-Packages.md) for complete list.
