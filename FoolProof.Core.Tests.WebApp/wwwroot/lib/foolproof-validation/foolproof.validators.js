;//You must load foolproof.core.js before this.

if (!FoolProofCore)
    throw "You must load foolproof.core.js before this.";

function isObject(value, pureOnly) {
    return !!value && typeof value === 'object'
           && (!pureOnly || Object.getPrototypeOf(value).isPrototypeOf(Object));
}

FoolProofCore.registerValidators = function (addValidatorFunc, getValidatorFunc, elemValueFunc) {
    if (!addValidatorFunc)
        throw "No register method supplied.";

    var getElementValue = elemValueFunc || function(element) {
        var result = element.value;
        if (element.type && (element.type == 'checkbox' || element.type == 'radio'))
            result = element.checked;
        else if (element.type == 'select-multiple'){
            var values = [];
            for (var i = 0; i < element.selectedOptions.length; i++)
                values.push(element.selectedOptions[i].value);

            result = values;
        }

        return result;
    }    

    function isOptional(element) {
        var val = getElementValue(element);
        var reqMethod = getValidatorFunc("required");
		return !reqMethod.call(this, val, element) && "dependency-mismatch";
    };

    //Unify the regex validation method
    addValidatorFunc("regex", function (value, element, params) {
        if (isOptional.call(this, element))
            return true;

        var pattern = typeof (params) === "string" ? params : params.pattern;
        var match = new RegExp(pattern).exec(value);
        return (match && (match.index === 0) && (match[0].length === value.length));
    });

    addValidatorFunc("is", function (value, element, params) {
        var elemValue = getElementValue(element);
        if (typeof (elemValue) !== typeof (value))
            value = elemValue;

        var operator = params["operator"];
        var passOnNull = (/true/i).test(params["passonnull"] + "");
        var dataType = params["datatype"];

        var dependentValue = params["dependentvalue"];
        if (typeof (dependentValue) === "string")
            dependentValue = JSON.parse(dependentValue);

        if (params["dependentproperty"]) {
            var elemId = FoolProofCore.getId(element, params["dependentproperty"]);
            dependentValue = getElementValue(document.getElementById(elemId));
        }
        else if (passOnNull && FoolProofCore.isNullish(value))
            return true;

        return FoolProofCore.is(value, operator, dependentValue, passOnNull, dataType);
    });

    addValidatorFunc("requiredif", function (value, element, params) {
        var dependentTestValue = params["dependentvalue"];
        if (typeof (dependentTestValue) === "string")
            dependentTestValue = JSON.parse(dependentTestValue);

        var operator = params["operator"];
        var pattern = params["pattern"];
        var dataType = params["datatype"];
        var dependentProperty = FoolProofCore.getName(element, params["dependentproperty"]);
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

    addValidatorFunc("requiredifempty", function (value, element, params) {
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

    addValidatorFunc("requiredifnotempty", function (value, element, params) {
        var dependentProperty = FoolProofCore.getId(element, params["dependentproperty"]);
        var dependentValue = getElementValue(document.getElementById(dependentProperty));

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

        return result;
    }

    function callValidation(value, elem2Valid, params, caller) {
        var adapterName = params["method"].toLowerCase();
        var methodParams = prepareParams(adapterName, params["params"]);
        var validationMethod = null;

        switch (adapterName) {
            case "length":
                validationMethod = getValidatorFunc("rangelength");
                break;
            default:
                validationMethod = getValidatorFunc(adapterName);
        }

        return validationMethod.apply(caller || this, [value, elem2Valid, methodParams]);
    }

    addValidatorFunc("predicate", function (value, element, params) {
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
                var operandValid = getValidatorFunc("predicate").apply(caller, [value, element, operands[0]]);
                return !operandValid;
            case "and":
                var falseOperand = operands.find(function (operand) {
                    return !getValidatorFunc("predicate").apply(caller, [value, element, operand]);
                });
                return falseOperand == undefined;
            case "or":
                var trueOperand = operands.find(function (operand) {
                    return getValidatorFunc("predicate").apply(caller, [value, element, operand]);
                });
                return trueOperand != undefined;
        }
    });

    addValidatorFunc("isvalid", function (value, element, params) {
        var modelPropName = params["modelpropertyname"];
        var elem2Valid = element;
        if (modelPropName) {
            var elemId = FoolProofCore.getId(element, modelPropName);
            elem2Valid = document.getElementById(elemId);
            value = getElementValue(elem2Valid);
        }

        var validParams = params["validationparams"];
        if (!isObject(validParams))
            return true;

        var result = callValidation(value, elem2Valid, validParams, this);
        return result;
    });
};