;//You must load aspnet-client-validation, mvcfoolproof.core.js and mvcfoolproof.validators.js scripts before this.

if (typeof (ValidationService) !== "function")
    throw "You must load aspnet-client-validation script before this.";

var validationService = new ValidationService();
FoolProofCore.registerValidators(
    validationService.addProvider,
    function (name) { return validationService.providers[name]; }
);
validationService.bootstrap();