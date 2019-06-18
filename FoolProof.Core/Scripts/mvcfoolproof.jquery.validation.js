;//You must load the mvcfoolproof.core.js script before this.

if (!FoolProofCore)
    throw "You must load the mvcfoolproof.core.js script before this.";

FoolProofCore.registerValidators = function(jQuery) {
    if (!jQuery)
        throw "You must load jquery library before this.";

    jQuery.validator.addMethod("is", function(value, element, params) {
        var dependentProperty = FoolProofCore.getId(element, params["dependentproperty"]);
        var operator = params["operator"];
        var passOnNull = params["passonnull"];
        var dependentValue = document.getElementById(dependentProperty).value;

        if (FoolProofCore.is(value, operator, dependentValue, passOnNull))
            return true;

        return false;
    });

    jQuery.validator.addMethod("requiredif", function(value, element, params) {
        var dependentProperty = FoolProofCore.getName(element, params["dependentproperty"]);
        var dependentTestValue = params["dependentvalue"];
        var operator = params["operator"];
        var pattern = params["pattern"];
        var dependentPropertyElement = document.getElementsByName(dependentProperty);
        var dependentValue = null;

        if (dependentPropertyElement.length > 1) {
            for (var index = 0; index != dependentPropertyElement.length; index++)
                if (dependentPropertyElement[index]["checked"]) {
                    dependentValue = dependentPropertyElement[index].value;
                    break;
                }

            if (dependentValue == null)
                dependentValue = false
        }
        else
            dependentValue = dependentPropertyElement[0].value;

        if (FoolProofCore.is(dependentValue, operator, dependentTestValue)) {
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
        var dependentProperty = FoolProofCore.getId(element, params["dependentproperty"]);
        var dependentValue = document.getElementById(dependentProperty).value;

        if (dependentValue == null || dependentValue.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') == "") {
            if (value != null && value.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "")
                return true;
        }
        else
            return true;

        return false;
    });

    jQuery.validator.addMethod("requiredifnotempty", function(value, element, params) {
        var dependentProperty = FoolProofCore.getId(element, params["dependentproperty"]);
        var dependentValue = document.getElementById(dependentProperty).value;

        if (dependentValue != null && dependentValue.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "") {
            if (value != null && value.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "")
                return true;
        }
        else
            return true;

        return false;
    });
};

(FoolProofCore.registerValidators)(jQuery);