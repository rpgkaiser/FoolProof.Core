## Starting from the migration to asp.net core of the [MVC Foolproof Validation](https://codeplexarchive.org/project/foolproof) library. 

## Validation attributes at your disposal.

**Validators to compare with other property:**
+ `Is` (base class)
+ `EqualTo`
+ `NotEqualTo`
+ `GreaterThan`
+ `LessThan`
+ `GreaterThanOrEqualTo`
+ `LessThanOrEqualTo`
+ `In` (*field* must be in *dependant*)
+ `NotIn` (*field* must not be in *dependant*)

**Validators to compare with specific values:**
+ `Is<T>` (base class)
+ `EqualTo<T>`
+ `NotEqualTo<T>`
+ `GreaterThan<T>`
+ `LessThan<T>`
+ `GreaterThanOrEqualTo<T>`
+ `LessThanOrEqualTo<T>`
+ `In<T>`
+ `NotIn<T>`
+ `IsEmpty`
+ `IsTrue`
+ `IsFalse`

**Improved required validators:**
+ `RequiredIf`              (*field* required if *dependant* satisfy condition)
+ `RequiredIfNot`           (*field* required if *dependant* doesn't satisfy condition)
+ `RequiredIfTrue`          (*field* required if *dependant* value is true)
+ `RequiredIfFalse`         (*field* required if *dependant* value is false)
+ `RequiredIfEmpty`         (*field* required if *dependant* has no value)
+ `RequiredIfNotEmpty`      (*field* required if *dependant* has value)
+ `RequiredIfRegExMatch`    (*field* required if *dependant* match regex)
+ `RequiredIfNotRegExMatch` (*field* required if *dependant* doesn't match regex)
+ `RegularExpressionIf`     (*field* must match regex if *dependant* satisfy condition)

**Predicate validators:** Predicate validators will allow you to create complex validation rules (logical predicates), 
combining other validators (inlcuding the predicate ones).
+ `Predicate` (base abstract class)
+ `Not(operand)`
+ `And(operand1, operand2,..., operandN)` 
+ `Or(operand1, operand2,..., operandN)`
+ `IsValid(property, operand)`

*operand* could be any `ValidationAttribute`, inlcuding the *predicate* ones, so you can recursively combine validators 
to build any logical predicate.

**IsValid** will let you validate any property in the model, using any existing `ValidationAttribute`; so combining the 
*predicate* validators and the *IsValid* validator, you can create validation rules to be evaluated at the model 
level (aka model-wise validation).

## Client-side validation

All the validators are available for client side validation to use with `jquery.validation` or `aspnet-client-validation`.
Although you can combine any *ValidationAttribute* using the *predicate* validators, only *operands* (`ValidationAttribute`) with 
a registered `IAttributeAdapter` or implementing the `IClientModelValidator` interface will be available for client-side validation; of course, 
this include all the validators provided by this library (and many others from `System.ComponentModel.DataAnnotations`).

This library provides you with an HTML helper method `ModelValidation` to render the model-wise validation as you would do with any
property validation. To bring this helper in context, you need to include the `FoolProof.Core` namespace with the `@using` directive.
Take a look at the [predicate page](http://foolproofcore.tryasp.net/predicate) in the example app.

## Installation

**NuGet:** _install-package_ [FoolProof.Core](https://www.nuget.org/packages/FoolProof.Core "FoolProof.Core nuget package URL")

## Setting Up

### Back-end
+ Include namespace  _FoolProof.Core_
+ Just add this line `services.AddFoolProof();` to your _ConfigureServices_ method on the _Startup_ class; this will register a new `IValidationAttributeAdapterProvider`.

### Front-end
After installing the nuget package, a new folder `foolproof-validation` should be created under your `wwwroot\lib` folder 
with all the required JavaScript files for the client-side validation. The content of this new folder correspond 
with the content of the `Scripts` folder in the nuget package.

#### To integrate with [jquery.validation](https://jqueryvalidation.org), include the following JavaScript files in the given order:

1. [`jquery.js` (NPM)](https://www.npmjs.com/package/jquery)
2. [`jquery.validate.js` (NPM)](https://www.npmjs.com/package/jquery-validation)
3. [`jquery.validate.unobtrusive.js` (NPM)](https://www.npmjs.com/package/@types/jquery-validation-unobtrusive)
4. `foolproof.core.js`
5. `foolproof.validators.js`
6. `foolproof.jquery.validation.js`
7. `foolproof.jquery.validation.unobtrusive.js`

#### To integrate with [aspnet-client-validation](https://github.com/haacked/aspnet-client-validation), include the following JavaScript files in the given order:

1. [`aspnet-validation.js` (NPM)](https://www.npmjs.com/package/aspnet-client-validation)
4. `foolproof.core.js`
5. `foolproof.validators.js`
6. `foolproof.aspnet-validation.js`
7. `foolproof.aspnet-validation.unobtrusive.  js`

Once the page get loaded, `FoolProofCore.aspnetValidationService` will contain a bootstrapped instance of a _[ValidationService](https://github.com/haacked/aspnet-client-validation?tab=readme-ov-file#quick-start-guide)_, with all
the available validation methods already registered and associated with the corresponding form fields.

## Example WebApp

You can review a kind of DEMO app (the WebApp used to execute the E2E tests) here: [E2E/Demo WebApp](http://rpgkaiser.github.io/FoolProof.Core "E2E/Demo WebApp URL")