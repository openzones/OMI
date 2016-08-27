$(function () {
    $.validator.addMethod('date',
            function (value, element) {
                if (this.optional(element)) {
                    return true;
                }
                var isValid = true;
                try {
                    if (value !== "__.__.____") {
                        $.datepicker.parseDate('dd.mm.yy', value);
                    }
                }
                catch (err) {
                    isValid = false;
                }
                return isValid;
            });
});