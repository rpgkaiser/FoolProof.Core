function showValidationResult($form, result) {
    if (result && result.succeed) {
        $(document).trigger("foolproof.validation.succeed");
        $(".valid-alert", $form).fadeOut(function () {
            $(this).find(".alert-message").html("Model validation succeed.");
            $(this).removeClass("alert-danger")
            .addClass("alert-success")
            .fadeIn();
        });  

        return;
    }

    if (!result || !result.succeed) {
        $(document).trigger("foolproof.validation.failed", result);
        $(".valid-alert", $form).fadeOut(function () {
            if (!Array.isArray(result.errors) || !result.errors.length)
                result.errors = ["Model validation failed."];
            var messages = $.map(result.errors, function (err, indx) {
                return `${(indx ? "<br/>" : "" )}<p>${err}</p>`;
            });
            $(this).find(".alert-message").html(messages);
            $(this).removeClass("alert-success")
            .addClass("alert-danger")
            .fadeIn();
        });  

        return;
    }
}

function clientValidate($form) {
    var valid = $form.valid();
    showValidationResult($form, {
        succeed: valid,
        errors: []
    });
}

function serverValidate($form) {
    $form.valid();

    $.ajax({
        url: $form.attr("action"),
        type: "POST",
        data: $form.serialize()
    })
    .then(function (result) { 
        showValidationResult($form, result);
    })
    .catch(function (error) { 
        showValidationResult($form, {
            succeed: false,
            errors: ["Unexpected error validating the model."]
        });
    });
}

function setupForms() {
    $("form .btn-validate").on("click", function (evt) {
        evt.preventDefault();

        var $form = $(this).closest("form");
        if ($(this).data("serverValidate"))
            serverValidate($form);
        else
            clientValidate($form);
    });

    $("form .btn-reset").on("click", function (evt) {
        evt.preventDefault();

        var $form = $(this).closest("form");
        $form[0].reset();

        $(".valid-alert", $form).fadeOut();
        $("[data-valmsg-for]", $form).html("");
    });
}