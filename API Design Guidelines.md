# UWP Toolkit - Design guidelines

The foundation of UWP toolkit is simplicity. 

A developer should be able to quickly and easily learn to use the API. 

Simplicity and a low barrier to entry are must-have features of every API. If you have any second thoughts about the complexity of a design, it is almost always much better to cut the feature from the current release and spend more time to get the design right for the next release. 

You can always add to an API; you cannot ever remove anything from one. If the design does not feel right, and you ship it anyway, you are likely to regret having done so.
Many of the guidelines of this document are obvious and serve only one purpose: Simplicity.

## General rules

* DO NOT require that users perform any extensive initialization before they can start programming basic scenarios.
* DO provide good defaults for all values associated with parameters, options, etc.
* DO ensure that APIs are intuitive and can be successfully used in basic scenarios without referring to the reference documentation.
* DO communicate incorrect usage of APIs as soon as possible. 
* DO design an API by writing code samples for the main scenarios. Only then, you define the object model that supports those code samples.

## Naming conventions
* DO use PascalCasing for namespace, type, and member names consisting of multiple words. 
* DO use camelCasing for parameter names and field values.
* DO use a capital I prefix followed by PascalCasing for interface parameter names.
* DO capitalize both characters of two-character acronyms, except the first word of a camel-cased identifier.
* DO capitalize only the first character of acronyms with three or more characters, except the first word of a camel-cased identifier.
* DO not capitalize any of the characters of any acronyms, whatever their length, at the beginning of a camel-cased identifier.
* DO NOT introduce generic type names such as Element, Node, Log, and Message.
* DO NOT have properties that match the name of “Get” methods
* DO name Boolean properties with an affirmative phrase (CanSeek instead of CantSeek).
* DO name the delegate used to describe an event callback with the "EventHander" suffix. For example, the delegate for a "Clicked" event should be named "ClickedEventHandler".

## Documentation
* DO NOT expect that your API is so well designed that it needs no documentation. No API is that intuitive.
* DO provide great documentation with all APIs. 
* DO use readable and self-documenting identifier names. 
* DO use consistent naming and terminology.
* DO provide strongly typed APIs.
* DO use verbose identifier names.
