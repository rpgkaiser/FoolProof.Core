function fadeOut(elem) {
    if (!elem.checkVisibility())
        return;

    elem.classList.remove("fade-in", "fade-out");
    setTimeout(function () {
        elem.classList.add("fade-out");
        setTimeout(function () {
            elem.classList.add("d-none");
        }, 400);
    }, 0);
}

function fadeIn(elem) {
    if (elem.checkVisibility())
        elem.classList.remove("fade-in", "fade-out");

    setTimeout(function () {
        elem.classList.add("fade-in");
        setTimeout(function () {
            elem.classList.remove("d-none");
        }, 400);
    }, 0);
}

function showValidationResult(form, result) {
    var validAlert = form.querySelector(".valid-alert");
    fadeOut(validAlert);

    if (result && result.succeed) {
        document.dispatchEvent(new Event("foolproof.validation.succeed"));
        validAlert.querySelector(".alert-message").innerHTML = "Model validation succeed.";
        validAlert.classList.remove("alert-danger");
        validAlert.classList.add("alert-success");
        fadeIn(validAlert);

        return;
    }

    document.dispatchEvent(new Event("foolproof.validation.failed", result));
        
    if (!Array.isArray(result.errors) || !result.errors.length)
        result.errors = ["Model validation failed."];

    var messages = "";
    result.errors.forEach(function (err, indx) {
        messages += `${(indx ? "<br />" : "")}<div>${err}</div>`;
    });
    validAlert.querySelector(".alert-message").innerHTML = messages;
    validAlert.classList.remove("alert-success");
    validAlert.classList.add("alert-danger");
    fadeIn(validAlert);
}

function clientValidate(form) {
    var valid = isFormValid(form);
    showValidationResult(form, {
        succeed: valid,
        errors: []
    });
}

//This function will be overriden for every validation library to use.
function isFormValid(form) {
    return form.valid();
}

function serverValidate(form) {
    isFormValid(form);

    var url = form.getAttribute("action");
    fetch(url, {
        method: "POST",
        body: new FormData(form)
    })
    .then(response => response.json())
    .then(function (result) { 
        showValidationResult(form, result);
    })
    .catch(function (error) { 
        showValidationResult(form, {
            succeed: false,
            errors: ["Unexpected error validating the model."]
        });
    });
}

function setupForms() {
    document.querySelectorAll("form .btn-validate").forEach(function (btnElem) {
        btnElem.addEventListener("click", function (evt) {
            evt.preventDefault();

            var form = this.closest("form");
            if (btnElem.dataset.serverValidate)
                serverValidate(form);
            else
                clientValidate(form);
        });
    })

    document.querySelector("form .btn-reset").addEventListener("click", function (evt) {
        evt.preventDefault();

        var form = this.closest("form");
        form.reset();

        fadeOut(form.querySelector(".valid-alert"));
        form.querySelector("[data-valmsg-for]").innerHTML = "";
    });
}

function useJQuery(useIt) {
    Cookies.set('UseJQuery', useIt, { expires: 7 });
    location.reload();
}