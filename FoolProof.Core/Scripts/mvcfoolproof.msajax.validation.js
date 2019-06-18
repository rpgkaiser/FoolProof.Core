;

if (!FoolProofCore)
    throw "You must load the mvcfoolproof.core.js script before this.";

if (!Sys.Mvc)
    throw "You must load microsoft ajax library before this.";

Sys.Mvc.ValidatorRegistry.validators["is"] = function(rule) {
    return function (value, context) {
        var operator = rule.ValidationParameters["operator"];
        var passOnNull = rule.ValidationParameters["passonnull"];
        var dependentProperty = FoolProofCore.getId(context.fieldContext.elements[0], rule.ValidationParameters["dependentproperty"]);
        var dependentValue = document.getElementById(dependentProperty).value;

        if (FoolProofCore.is(value, operator, dependentValue, passOnNull))
            return true;

        return rule.ErrorMessage;
    };
};

Sys.Mvc.ValidatorRegistry.validators["requiredif"] = function (rule) {
    var pattern = rule.ValidationParameters["pattern"];
    var dependentTestValue = rule.ValidationParameters["dependentvalue"];
    var operator = rule.ValidationParameters["operator"];
    return function (value, context) {
        var dependentProperty = FoolProofCore.getName(context.fieldContext.elements[0], rule.ValidationParameters["dependentproperty"]);
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

        return rule.ErrorMessage;
    };
};

Sys.Mvc.ValidatorRegistry.validators["requiredifempty"] = function (rule) {
    return function (value, context) {
        var dependentProperty = FoolProofCore.getId(context.fieldContext.elements[0], rule.ValidationParameters["dependentproperty"]);
        var dependentValue = document.getElementById(dependentProperty).value;

        if (dependentValue == null || dependentValue.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') == "") {
            if (value != null && value.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "")
                return true;
        }
        else
            return true;

        return rule.ErrorMessage;
    };
};

Sys.Mvc.ValidatorRegistry.validators["requiredifnotempty"] = function (rule) {
    return function (value, context) {
        var dependentProperty = FoolProofCore.getId(context.fieldContext.elements[0], rule.ValidationParameters["dependentproperty"]);
        var dependentValue = document.getElementById(dependentProperty).value;

        if (dependentValue != null && dependentValue.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "") {
            if (value != null && value.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "")
                return true;
        }
        else
            return true;

        return rule.ErrorMessage;
    };
};