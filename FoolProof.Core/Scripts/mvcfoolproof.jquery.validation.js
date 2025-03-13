;//You must load the mvcfoolproof.core.js script before this.

if (!FoolProofCore)
    throw "You must load the mvcfoolproof.core.js script before this.";

function isObject(value, pureOnly) {
    return !!value && typeof value === 'object'
           && (!pureOnly || Object.getPrototypeOf(value).isPrototypeOf(Object));
}

function getElementValue(element) {
    var $elem = jQuery(element);
    var result = $elem.val();
    if ($elem.is("[type=checkbox],[type=radio]"))
        result = $elem.filter(":checked").val();

    return result;
}

FoolProofCore.registerValidators = function (jQuery) {
    if (!jQuery)
        throw "You must load jquery library before this.";

    jQuery.validator.addMethod("is", function (value, element, params) {
        var operator = params["operator"];
        var passOnNull = params["passonnull"];
        var dataType = params["datatype"];

        var dependentValue = params["dependentvalue"];
        if (params["dependentproperty"]) {
            var elemId = FoolProofCore.getId(element, params["dependentproperty"]);
            dependentValue = getElementValue(document.getElementById(elemId));
        }

        return FoolProofCore.is(value, operator, dependentValue, passOnNull, dataType);
    });

    jQuery.validator.addMethod("requiredif", function (value, element, params) {
        var dependentProperty = FoolProofCore.getName(element, params["dependentproperty"]);
        var dependentTestValue = params["dependentvalue"];
        var operator = params["operator"];
        var pattern = params["pattern"];
        var dataType = params["datatype"];
        var dependentPropertyElement = document.getElementsByName(dependentProperty);
        var dependentValue = null;

        if (dependentPropertyElement.length > 1) {
            for (var index = 0; index != dependentPropertyElement.length; index++)
                if (dependentPropertyElement[index]["checked"]) {
                    dependentValue = getElementValue(dependentPropertyElement[index]);
                    break;
                }

            if (dependentValue == null)
                dependentValue = false
        }
        else
            dependentValue = getElementValue(dependentPropertyElement[0]);

        if (FoolProofCore.is(dependentValue, operator, dependentTestValue, undefined, dataType)) {
            if (pattern == null) {
                if (value != null && value.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "")
                    return true;
            }
            else
                return (new RegExp(pattern)).test(value);
        }
        else
            return true;

        return false;
    });

    jQuery.validator.addMethod("requiredifempty", function (value, element, params) {
        var dependentProperty = FoolProofCore.getId(element, params["dependentproperty"]);
        var dependentValue = getElementValue(document.getElementById(dependentProperty));

        if (dependentValue == null || dependentValue.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') == "") {
            if (value != null && value.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "")
                return true;
        }
        else
            return true;

        return false;
    });

    jQuery.validator.addMethod("requiredifnotempty", function (value, element, params) {
        var dependentProperty = FoolProofCore.getId(element, params["dependentproperty"]);
        var dependentValue = getElementValue("#" + dependentProperty);

        if (dependentValue != null && dependentValue.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "") {
            if (value != null && value.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "")
                return true;
        }
        else
            return true;

        return false;
    });

    var numMethods = ["range", "rangelength", "length", "minlength", "maxlength", "min", "max", "step"];
    function prepareParams(adapterName, adapterParams) {
        var result = adapterParams;

        if (numMethods.indexOf(adapterName) != -1) {
            var min = adapterParams['min'];
            var max = adapterParams['max'];
            if (min && max) {
                result = [Number(min), Number(max)];
            }
            else if (min) {
                result = Number(min);
            }
            else if (max) {
                result = Number(max);
            }
            else {
                result = [];
                for (var key in adapterParams) {
                    if (adapterParams[key])
                        result.push(Number(adapterParams[key]));
                }

                result = result.length == 1 ? result[0] : result;
            }
        }
        else if (adapterName == "regex") {
            result = adapterParams["pattern"];
        }

        return result;
    }

    function callValidation(value, element, params, caller) {
        var adapterName = params["method"].toLowerCase();
        var methodParams = prepareParams(adapterName, params["params"]);
        var validationMethod = null;

        switch (adapterName) {
            case "length":
                validationMethod = jQuery.validator.methods["rangelength"];
                break;
            default:
                validationMethod = jQuery.validator.methods[adapterName];
        }

        return validationMethod.apply(caller || this, [value, element, methodParams]);
    }

    jQuery.validator.addMethod("predicate", function (value, element, params) {
        var logicalOper = params["logicaloperator"];
        if (!logicalOper)
        {
            var result = callValidation(value, element, params, this);
            return result;
        }

        var operands = params["operands"] || [];
        if (!Array.isArray(operands))
            operands = [operands];

        operands = operands.filter(function (value, indx) { return isObject(value); });
        if (!operands.length)
            return true;

        var caller = this;
        switch (logicalOper.toLowerCase()) {
            case "not":
                var operandValid = jQuery.validator.methods.predicate.apply(caller, [value, element, operands[0]]);
                return !operandValid;
            case "and":
                var falseOperand = operands.find(function (operand) {
                    return !jQuery.validator.methods.predicate.apply(caller, [value, element, operand]);
                });
                return falseOperand == undefined;
            case "or":
                var trueOperand = operands.find(function (operand) {
                    return jQuery.validator.methods.predicate.apply(caller, [value, element, operand]);
                });
                return trueOperand != undefined;
        }
    });

    jQuery.validator.addMethod("isvalid", function (value, element, params) {
        var modelPropName = params["modelpropertyname"];
        if (modelPropName) {
            var elemId = FoolProofCore.getId(element, modelPropName);
            element = document.getElementById(elemId);
            value = getElementValue(element);
        }

        var validParams = params["validationparams"];
        if (!isObject(validParams))
            return true;

        var result = callValidation(value, element, validParams, this);
        return result;
    });
};

(FoolProofCore.registerValidators)(jQuery);