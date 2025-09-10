;//You must load the mvcfoolproof.core.js and mvcfoolproof.validators.js scripts before this.

if (!jQuery || !jQuery.validator)
    throw "You must load jQuery and jQuery.validation before this.";

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
    