## Migration to asp.net core of the [MVC Foolproof Validation](https://codeplexarchive.org/project/foolproof) library. 

This library add many new validation attributes to your toolbox.

_Operator validators:_
+ Is
+ EqualTo
+ NotEqualTo
+ GreaterThan
+ LessThan
+ GreaterThanOrEqualTo
+ LessThanOrEqualTo


*Improved required validators:*
+ RequiredIf
+ RequiredIfNot
+ RequiredIfTrue
+ RequiredIfFalse
+ RequiredIfEmpty
+ RequiredIfNotEmpty
+ RequiredIfRegExMatch
+ RequiredIfNotRegExMatch

See full library documentation here: [https://codeplexarchive.org/project/foolproof](https://codeplexarchive.org/project/foolproof)

### New features added:

_New operator validators:_
+ In
+ NotIn

**_All the validators are available for client side validation as well._**

## Installation

**NuGet:** _install-package_ [FoolProof.Core](https://www.nuget.org/packages/FoolProof.Core "FoolProof.Core nuget package URL")

## Setting Up

+ Include namespace  _FoolProof.Core_
+ Just add this line `services.AddFoolProof();` to your _ConfigureServices_ method on the _Startup_ class; this will register a new `IValidationAttributeAdapterProvider`.

## Example WebApp

You can review a kind of DEMO app (the WebApp used to execute E2E tests) here: [E2E/Demo WebApp](http://rpgkaiser.github.io/foolproof.core "E2E/Demo WebApp URL")