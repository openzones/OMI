$.validator.unobtrusive.addValidation = function (selector) {
    var form = $(selector);
    $(selector).removeData("validator");
    $.validator.unobtrusive.parse(form);
}