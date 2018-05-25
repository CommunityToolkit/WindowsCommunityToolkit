---
title: DeepLinkParser
author: nmetulev
description: Provides a way to create, Dictionary<string,string> - inheriting object that provides an additional .Root property to pull the base path of the URI 
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, DeepLinkParser
dev_langs:
  - csharp
  - vb
---

# DeepLinkParser

The [DeepLinkParser Class](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.helpers.deeplinkparser) provides a way to create, from `IActivatedEventArgs` a `Dictionary<string,string>`-inheriting object that provides an additional `.Root` property to pull the base path of the URI (eg: in `MainPage/Options?option1=value1`, `.Root` would be `MainPage/Options`.
Once you have an instance, simply saying `instance["optionName"]` will pull the value from the querystring for that option.

### Properties

| Property | Type | Description |
| -- | -- | -- |
| Root | string | Gets or sets the root path of the Deep link URI |

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| Create(String) | DeepLinkParser | Creates an instance of `DeepLinkParser` for the given `string` |
| Create(Uri) | DeepLinkParser | Creates an instance of DeepLinkParser for the given Uri |
| Create(IActivatedEventArgs) | DeepLinkParser | Creates an instance of DeepLinkParser for the given [IActivatedEventArgs](https://docs.microsoft.com/uwp/api/Windows.ApplicationModel.Activation.IActivatedEventArgs) |
| ValidateSourceUri(String) | Uri | Validates the source URI |

### Example

In OnLaunched of App.xaml.cs:

```csharp
if (e.PrelaunchActivated == false)
{
    if (rootFrame.Content == null)
    {
        var parser = DeepLinkParser.Create(args);
        if (parser["username"] == "John Doe")
        {
            // do work here
        }
        if (parser.Root == "Signup")
        {
            rootFrame.Navigate(typeof(Signup));
        }
...
```
```vb
If e.PrelaunchActivated = False Then
    If rootFrame.Content Is Nothing Then
        Dim parser = DeepLinkParser.Create(args)
        If parser("username") = "John Doe" Then
            ' do work here
        End If

        If parser.Root = "Signup" Then
            rootFrame.Navigate(GetType(Signup))
        End If
...
```

## CollectionFormingDeepLinkParser

The [CollectionFormingDeepLinkParser Class](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.helpers.collectionformingdeeplinkparser) will to be able to do something like `?pref=this&pref=that&pref=theOther` and have a pull of `pref` come back with `this,that,theOther` as its value. This derivative of `DeepLinkParser` provides this functionality.

### Example

in OnLaunched of App.xaml.cs:

```csharp
if (e.PrelaunchActivated == false)
{
    if (rootFrame.Content == null)
    {
        var parser = CollectionFormingDeepLinkParser.Create(args);
        if (parser["username"] == "John Doe")
        {
            // do work here
        }
        if (parser.Root == "Signup")
        {
            var preferences = parser["pref"].Split(',');    // now a string[] of all 'pref' querystring values passed in URI
            rootFrame.Navigate(typeof(Signup));
        }
...
```
```vb
If e.PrelaunchActivated = False Then
    If rootFrame.Content Is Nothing Then
        Dim parser = CollectionFormingDeepLinkParser.Create(args)
        If parser("username") = "John Doe" Then
            ' do work here
        End If

        If parser.Root = "Signup" Then
            Dim preferences = parser("pref").Split(","c) ' now a string[] of all 'pref' querystring values passed in URI
            rootFrame.Navigate(GetType(Signup))
        End If
...
```

Both of these are createable using a `.Create(IActivatedEventArgs)` method. Should you wish to create one in a different manner, the default constructor is `protected` so inheriting from either of these can provide extensibility.
The method that does the heavy lifting of parsing in to the `Dictionary<string,string>` (`ParseUriString`) is also `protected` and `virtual` so can be used/overridden by any inheriting class.

## QueryParameterCollection

The [QueryParameterCollection](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.helpers.queryparametercollection) helper class aids in the creation of a `Collection<KeyValuePair<string,string>>` populated with they key-value pairs of all parameters in a query string.

### Example

```csharp
var myUrl = "http://microsoft.com/?user=fooUser&email=fooUser@outlook.com&firstName=John&lastName=Doe"
var paramCollection = new QueryParameterCollection(myUrl);
foreach (var pair in paramCollection)
{
    Console.WriteLine($"{pair.Key} - {pair.Value}");
}
```
```vb
Dim myUrl = "http://microsoft.com/?user=fooUser&email=fooUser@outlook.com&firstName=John&lastName=Doe"
Dim paramCollection = New QueryParameterCollection(myUrl)
For Each pair In paramCollection
    Console.WriteLine($"{pair.Key} - {pair.Value}")
Next
```

### Output

```
user - fooUser
email - fooUser@outlook.com
firstname - John
lastName - Doe
```

### Requirements

| Device family | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |
| NuGet package | [Microsoft.Toolkit.Uwp](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp/) |

### API

* [DeepLinkParser source code](https://github.com/Microsoft/WindowsCommunityToolkit//tree/master/Microsoft.Toolkit.Uwp/Helpers/DeepLinkParser)