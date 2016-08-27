var Validation = Validation || {
    validate: function (event) {
        event = event || window.event || event.srcElement;
        var form = $("#" + event.srcElement.id).parents("form");
        $(form).removeData("validator");
        $.validator.unobtrusive.parse(form);
        //$(form).find(".uncollapseAll").click();
        $(':input[required=""],:input[required]').filter(function () { return $(this).val() === ""; }).addClass('input-validation-error');
        $('select[required]').filter(function () { return $(this).val() === "" }).addClass('input-validation-error');
        $('.datepicker').filter(function () { return $(this).val() === "" && $(this).prop("required") }).addClass('input-validation-error');

        $(form).find(".input-validation-error").each(function () {
            var $this = $(this);
            $this.closest('.panel').children('.panel-body').slideDown();
            $this.removeClass('panel-collapsed');
        });
    }
}