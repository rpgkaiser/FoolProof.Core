;//You must load aspnet-client-validation, foolproof.core.js, foolproof.validators.js and foolproof.aspnet-client-validation.js before this.

if (typeof (aspnetValidation) !== "object" || typeof (aspnetValidation.ValidationService) !== "function")
    throw "You must load aspnet-client-validation before this.";

if (!FoolProofCore)
    throw "You must load foolproof.core.js before this.";

if (!FoolProofCore.registerValidators)
    throw "You must load foolproof.validators.js before this.";

if(!FoolProofCore.aspnetValidationService)
    throw "You must load foolproof.aspnet-client-validation.js before this.";

var oldParseFunc = aspnetValidation.ValidationService.prototype.parseDirectives;
aspnetValidation.ValidationService.prototype.parseDirectives = function (attributes) {
    var that = this;
    var directives = oldParseFunc.call(that, attributes);
    for (var key in directives) {
        var currDtv = directives[key];
        switch (key) {
            //case "is":
            //case "requiredif":
            //    currDtv.params.dependentvalue = !!currDtv.params.dependentvalue ? JSON.parse(currDtv.params.dependentvalue) : null;
            //    break;
            case "predicate":
                currDtv.params.operands = JSON.parse(currDtv.params.operands);
                break;
            case "isvalid":
                currDtv.params.validationParams = JSON.parse(currDtv.params.validationParams);
                break;
        }
    }

    return directives;
};

//Hack for model-wise validation to work with aspnet-validation.
(function () {
    function elementsHack() {
        var elems = document.querySelectorAll("[data-model-validation=true]");
        elems.forEach(function (elem) {
            elem.name = elem.name || elem.getAttribute("name");
            elem.form = elem.closest("form");
        });
    }

    new MutationObserver(function(mutations) {
        elementsHack();
    }).observe(document, { childList: true });

    elementsHack();

    //By default aspnet-validation validate input, select and texarea elements only.
    var oldIsValidatable = aspnetValidation.isValidatable;
    aspnetValidation.isValidatable = function (element) {
        return oldIsValidatable.call(aspnetValidation, element) || element.getAttribute("data-model-validation") == "true";
    }

    var oldScanInputs = aspnetValidation.ValidationService.prototype.scanInputs;
    aspnetValidation.ValidationService.prototype.scanInputs = function (root, cb) {
        var that = this;
        oldScanInputs.call(that, root, cb);
        var modelElems = root.querySelectorAll("[data-model-validation=true]");
        modelElems.forEach(function (elem) { cb.call(that, elem); });
    };
})();