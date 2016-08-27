var DisplaySettings = DisplaySettings || {
    disableAll: function (divId, formId) {
        var container = document.getElementById(divId);
        $(container).find(':text').prop('disabled', true);
        $(container).find('select').prop('disabled', true);
        $(container).find(':checkbox').prop('disabled', true);
        $(container).find('textarea').prop('disabled', true);
        $('#' + formId).find('input:submit').prop('disabled', true);
    }
}