;//You must load the mvcfoolproof.core.js script before this.

if (!FoolProofCore)
    throw "You must load the mvcfoolproof.core.js script before this.";

FoolProofCore.registerValidators = function (jQuery) {
    if (!jQuery)
        throw "You must load jquery library before this.";

    jQuery.validator.addMethod("is", function (value, element, params) {
        var dependentProperty = FoolProofCore.getId(element, params["dependentproperty"]);
        var operator = params["operator"];
        var passOnNull = params["passonnull"];
        var dataType = params["datatype"];
        var dependentValue = jQuery(document.getElementById(dependentProperty)).val();

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
                    dependentValue = jQuery(dependentPropertyElement[index]).val();
                    break;
                }

            if (dependentValue == null)
                dependentValue = false
        }
        else
            dependentValue = jQuery(dependentPropertyElement[0]).val();

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
        var dependentValue = jQuery(document.getElementById(dependentProperty)).val();

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
        var dependentValue = jQuery("#" + dependentProperty).val();

        if (dependentValue != null && dependentValue.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "") {
            if (value != null && value.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "")
                return true;
        }
        else
            return true;

        return false;
    });

    jQuery.validator.addMethod("predicate", function (value, element, params) {
        var logicalOper = params["logicaloperator"];
        if (!logicalOper) {
            var methodName = params["method"].toLowerCase();
            var methodParams = params["params"];
            return jQuery.validator.methods[methodName].apply(this, [value, element, methodParams]);
        }

        var operands = params["operands"];
        if (!Array.isArray(operands))
            operands = [operand];

        var validMethod = jQuery.validator.methods.predicate;
        var self = this;
        switch (logicalOper.toLowerCase()) {
            case "not":
                var operandValid = validMethod.apply(self, [value, element, operands[0]]);
                return !operandValid;
            case "and":
                var falseOperand = operands.find(function (operand) { return !validMethod.apply(self, [value, element, operand]) });
                return falseOperand == undefined;
            case "or":
                var trueOperand = operands.find(function (operand) { return validMethod.apply(self, [value, element, operand]) });
                return trueOperand != undefined;
        }
    });

    jQuery.validator.addMethod("isvalid", function (value, element, params) {
        function prepareParams(methodName, methodParams) {
            if (methodName == 'range' || methodName == 'rangelength') {
                if (methodParams['min'])
                    methodParams['min'] = methodParams[0] = Number(methodParams['min']);
                if (methodParams['max'])
                    methodParams['max'] = methodParams[1] = Number(methodParams['max']);
            }
            else {
                var numMethods = ["length", "minlength", "maxlength", "min", "max", "step"];
                if (numMethods.indexOf(methodName) != -1)
                    for (var i = 0; i < numMethods.length; i++) {
                        if (methodParams[numMethods[i]])
                            methodParams[numMethods[i]] = Number(methodParams[numMethods[i]]);
                    }
            }
        }

        var modelPropName = params["modelpropertyname"];
        if (modelPropName) {
            var elemId = FoolProofCore.getId(element, modelPropName);
            element = document.getElementById(elemId);
            value = jQuery(element).val();
        }

        var validationParams = params["validationparams"];
        var methodName = validationParams["method"].toLowerCase();
        var validMethod = jQuery.validator.methods[methodName];

        var methodParams = validationParams["params"];
        prepareParams(methodName, methodParams);

        return validMethod.apply(this, [value, element, methodParams]);
    });
};

(FoolProofCore.registerValidators)(jQuery);