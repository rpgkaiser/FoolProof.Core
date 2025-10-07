;//You must load aspnet-client-validation, foolproof.core.js and foolproof.validators.js before this.

if (typeof (aspnetValidation) !== "object" && typeof (aspnetValidation.ValidationService) !== "function")
    throw "You must load aspnet-client-validation before this.";

if (!FoolProofCore)
    throw "You must load foolproof.core.js before this.";

if (!FoolProofCore.registerValidators)
    throw "You must load foolproof.validators.js before this.";

FoolProofCore.aspnetValidationService = new aspnetValidation.ValidationService();
FoolProofCore.registerValidators(
    function (name, callback) {
        FoolProofCore.aspnetValidationService.addProvider.call(FoolProofCore.aspnetValidationService, name, callback);
    },
    function (name) { return FoolProofCore.aspnetValidationService.providers[name]; }
);
FoolProofCore.aspnetValidationService.bootstrap();