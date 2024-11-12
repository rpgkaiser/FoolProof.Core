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

$Unob.adapters.add("is", ["dependentproperty", "operator", "passonnull", "datatype"], function(options) {
    setValidationValues(options, "is", {
        dependentproperty: options.params.dependentproperty,
        operator: options.params.operator,
        passonnull: options.params.passonnull,
        datatype: options.params.datatype
    });
});

$Unob.adapters.add("requiredif", ["dependentproperty", "dependentvalue", "operator", "pattern", "datatype"], function(options) {
    var value = {
        dependentproperty: options.params.dependentproperty,
        dependentvalue: options.params.dependentvalue,
        operator: options.params.operator,
        pattern: options.params.pattern,
        datatype: options.params.datatype
    };
    setValidationValues(options, "requiredif", value);
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

$Unob.adapters.add("predicate", ["logicalOperator", "leftPart", "rightPart"], function(options) {
    setValidationValues(options, "predicate", {
        logicaloperator: options.params.logicalOperator,
        leftpart: JSON.parse(options.params.leftPart),
        rightpart: options.params.rightPart ? JSON.parse(options.params.rightPart) : null
    });
});