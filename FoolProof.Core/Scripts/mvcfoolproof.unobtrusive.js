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

var $Unob = $.validator.unobtrusive;

$Unob.adapters.add("requiredif", ["dependentproperty", "dependentvalue", "operator", "pattern"], function(options) {
    var value = {
        dependentproperty: options.params.dependentproperty,
        dependentvalue: options.params.dependentvalue,
        operator: options.params.operator,
        pattern: options.params.pattern
    };
    setValidationValues(options, "requiredif", value);
});

$Unob.adapters.add("is", ["dependentproperty", "operator", "passonnull"], function(options) {
    setValidationValues(options, "is", {
        dependentproperty: options.params.dependentproperty,
        operator: options.params.operator,
        passonnull: options.params.passonnull
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