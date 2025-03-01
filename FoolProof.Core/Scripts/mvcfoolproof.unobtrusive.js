;//You must load the mvcfoolproof.core.js and mvcfoolproof.jquery.validation.js scripts before this one.

if (!FoolProofCore)
    throw "You must load the mvcfoolproof.core.js script before this.";

if (!FoolProofCore.registerValidators)
    throw "You must load the mvcfoolproof.jquery.validation.js script before this.";

var setValidationValues = function(options, ruleName, value) {
    options.rules[ruleName] = value;
    if (options.message) {
        options.messages[ruleName] = options.message;
    }
};

var $Unob = jQuery.validator.unobtrusive;

$Unob.adapters.add("is", ["dependentproperty", "operator", "passonnull", "datatype"], function(options) {
    setValidationValues(options, "is", {
        dependentproperty: options.params.dependentproperty,
        operator: options.params.operator,
        passonnull: options.params.passonnull,
        datatype: options.params.datatype
    });
});

$Unob.adapters.add("requiredif", ["dependentproperty", "dependentvalue", "operator", "pattern", "datatype"], function(options) {
    setValidationValues(options, "requiredif", {
        dependentproperty: options.params.dependentproperty,
        dependentvalue: options.params.dependentvalue,
        operator: options.params.operator,
        pattern: options.params.pattern,
        datatype: options.params.datatype
    });
});

$Unob.adapters.add("requiredifempty", ["dependentproperty"], function(options) {
    setValidationValues(options, "requiredifempty", {
        dependentproperty: options.params.dependentproperty
    });
});

$Unob.adapters.add("requiredifnotempty", ["dependentproperty"], function(options) {
    setValidationValues(options, "requiredifnotempty", {
        dependentproperty: options.params.dependentproperty
    });
});

$Unob.adapters.add("predicate", ["logicalOperator", "operands"], function(options) {
    setValidationValues(options, "predicate", {
        targetPropertyName: options.params.targetPropertyName,
        logicaloperator: options.params.logicalOperator,
        operands: JSON.parse(options.params.operands)
    });
});

$Unob.adapters.add("isvalid", ["modelPropertyName", "validationParams"], function(options) {
    setValidationValues(options, "isvalid", {
        modelPropertyName: options.params.modelPropertyName,
        validationParams: JSON.parse(options.params.validationParams)
    });
});

//Hack for model-wise validation to work:
//jQuery unobtrusive validation expects a name field for each validable element.
(function ($) {
    function elementsHack() {
        $("[data-model-validation=true]").each(function () {
            this.name = this.name || $(this).attr("name");
            this.form = $(this).closest("form")[0];
        });
    }

    new MutationObserver(function(mutations) {
        elementsHack();
    }).observe(document, { childList: true });

    elementsHack();

    //By default jQuery validation only validate "editable" elements.
    var prevElements = $.validator.prototype.elements;
    $.validator.prototype.elements = function () {
        var $defElems = prevElements.call(this);
        return $defElems.add($("[data-model-validation=true]", this.currentForm));
    };
})(jQuery);