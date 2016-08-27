var PartialProcessor = PartialProcessor || {

    getPartial: function (url, containerId, successCallback) {
        $('.loadingOverlay').show();
        $.ajax({
            type: 'GET',
            url: url,
            success: function (data) {
                $('.loadingOverlay').hide();
                $('#' + containerId).html(data);
                if (successCallback) {
                    successCallback(containerId);
                }
            },
            error: function (data) {
                $('.loadingOverlay').hide();
            }
        });
    },

    saveAndGetPartial: function (url, formId, containerId) {
        $.validator.unobtrusive.parse($('#' + formId));
        if ($('#' + formId).valid()) {
            $('#' + formId).ajaxSubmit(
                {
                    url: url,
                    type: 'post',
                    target: $('#' + formId).parent(),
                    success: function (data) {
                        if (containerId) {
                            $('#' + containerId).html(data);
                        } else {
                            $('#' + formId).parent().html(data);
                        }
                    }
                })
        } else {
            validator.focusInvalid();
        }
    },

    getFileWithParameters: function (url, formId) {
        var prevUrl = $('#' + formId).attr('action');
        $('#' + formId).attr('action', url);
        $('#' + formId).submit();
        $('#' + formId).attr('action', prevUrl);
    },

    submitForm: function (url, formId) {
        if (url && url !== '') {
            $('#' + formId).attr('action', url);
        }
        $('#' + formId).submit();
    }
};