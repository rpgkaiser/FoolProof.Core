;//You must load the mvcfoolproof.core.js script before this.

if (!FoolProofCore)
    throw "You must load the mvcfoolproof.core.js script before this.";

FoolProofCore.registerValidators = function(jQuery) {
    if (!jQuery)
        throw "You must load jquery library before this.";

    jQuery.validator.addMethod("is", function(value, element, params) {
        var targPropName = params["targetpropertyname"];
        if (targPropName) {
            var propId = FoolProofCore.getId(element, targPropName);
            element = document.getElementById(propId);
            value = jQuery(element).val();
        }

        var dependentProperty = FoolProofCore.getId(element, params["dependentproperty"]);
        var operator = params["operator"];
        var passOnNull = params["passonnull"];
        var dataType = params["datatype"];
        var dependentValue = jQuery(document.getElementById(dependentProperty)).val();

        return FoolProofCore.is(value, operator, dependentValue, passOnNull, dataType);
    });

    jQuery.validator.addMethod("requiredif", function(value, element, params) {
        var targPropName = params["targetpropertyname"];
        if (targPropName) {
            var propId = FoolProofCore.getId(element, targPropName);
            element = document.getElementById(propId);
            value = jQuery(element).val();
        }

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

    jQuery.validator.addMethod("requiredifempty", function(value, element, params) {
        var targPropName = params["targetpropertyname"];
        if (targPropName) {
            var propId = FoolProofCore.getId(element, targPropName);
            element = document.getElementById(propId);
            value = jQuery(element).val();
        }

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

    jQuery.validator.addMethod("requiredifnotempty", function(value, element, params) {
        var targPropName = params["targetpropertyname"];
        if (targPropName) {
            var propId = FoolProofCore.getId(element, targPropName);
            element = document.getElementById(propId);
            value = jQuery(element).val();
        }

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
            return jQuery.validator.methods[methodName](value, element, methodParams);
        }

        var leftPart = params["leftpart"];
        var rightPart = params["rightpart"];

        switch (logicalOper.toLowerCase()) {
            case "not":
                var validMethod = jQuery.validator.methods.predicate;
                return !validMethod(value, element, leftPart);
            case "and":
                var validMethod = jQuery.validator.methods.predicate;
                return validMethod(value, element, leftPart) && validMethod(value, element, rightPart);
            case "or":
                var validMethod = jQuery.validator.methods.predicate;
                return validMethod(value, element, leftPart) || validMethod(value, element, rightPart);
        }
    });
};

(FoolProofCore.registerValidators)(jQuery);