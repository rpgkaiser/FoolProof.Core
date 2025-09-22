;//You must load jQuery, jQuery.validation, foolproof.core.js and foolproof.validators.js before this.

if (!jQuery || !jQuery.validator)
    throw "You must load jQuery, jQuery.validation before this.";

if (!FoolProofCore)
    throw "You must load foolproof.core.js before this.";

if (!FoolProofCore.registerValidators)
    throw "You must load foolproof.validators.js before this.";

FoolProofCore.registerValidators(
    jQuery.validator.addMethod, 
    function (name) { return jQuery.validator.methods[name]; },
    function (element) {
        var $elem = jQuery(element);
        var result = $elem.val();
        if ($elem.is("[type=checkbox],[type=radio]"))
            result = $elem.filter(":checked").val();

        return result;
    }
);
    