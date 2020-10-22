## Migration to asp.net core of the [MVC Foolproof Validation](https://archive.codeplex.com/?p=foolproof) library. 

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

See full library documentation here: [https://archive.codeplex.com/?p=foolproof](https://archive.codeplex.com/?p=foolproof "FoolProof library doc.").

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
