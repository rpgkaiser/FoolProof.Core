## Migration to asp.net core of the [MVC Foolproof Validation](https://codeplexarchive.org/project/foolproof) library. 

This library add many validation attributes to your toolbox.

**Operator validators:**
+ Is
+ EqualTo
+ NotEqualTo
+ GreaterThan
+ LessThan
+ GreaterThanOrEqualTo
+ LessThanOrEqualTo

**Improved required validators:**
+ RequiredIf              (*field* required if *dependant* satisfy condition)
+ RequiredIfNot           (*field* required if *dependant* doesn't satisfy condition)
+ RequiredIfTrue          (*field* required if *dependant* value is true)
+ RequiredIfFalse         (*field* required if *dependant* value is false)
+ RequiredIfEmpty         (*field* required if *dependant* has no value)
+ RequiredIfNotEmpty      (*field* required if *dependant* has value)
+ RequiredIfRegExMatch    (*field* required if *dependant* match regex)
+ RequiredIfNotRegExMatch (*field* required if *dependant* doesn't match regex)
+ RegularExpressionIf     (*field* must match regex if *dependant* satisfy condition)

### New features added:

**New operator validators:**
+ In (*field* must be in *dependant*)
+ NotIn (*field* must not be in *dependant*)

**New predicate validators:** Predicate validators will allow you to create complex validation rules (logical predicates), 
combining other validators (inlcuding the predicate ones).
+ Not(_predicate_)
+ And(_predicate1_, _predicate2_) 
+ Or(_predicate1_, _predicate2_)
_preciate_ could be any of the existing validator, inlcuding the predicate ones, so you can recursively combine validators 
to build any logical predicate.

**_All the validators are available for client side validation as well._**

## Installation

**NuGet:** _install-package_ [FoolProof.Core](https://www.nuget.org/packages/FoolProof.Core "FoolProof.Core nuget package URL")

## Setting Up

+ Include namespace  _FoolProof.Core_
+ Just add this line `services.AddFoolProof();` to your _ConfigureServices_ method on the _Startup_ class; this will register a new `IValidationAttributeAdapterProvider`.

## Example WebApp

You can review a kind of DEMO app (the WebApp used to execute the E2E tests) here: [E2E/Demo WebApp](http://rpgkaiser.github.io/FoolProof.Core "E2E/Demo WebApp URL")

## Model-wise validation rules

One important feature is the ability to specify a _TargetPropertyName_ in the validators, so, model-wise validation rules can
be created using the _Predicate_ validators. Take a look at the [predicate page](http://foolproofcore.tryasp.net/predicate) in the example app.