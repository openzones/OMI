var FileProcessor = FileProcessor || {
    uploadImage: function (fileInput, imageId, hiddenInputId) {
        var file_data = $('#' + fileInput).prop("files")[0];
        var form_data = new FormData();
        form_data.append("file", file_data)
        $.ajax({
            url: window.location.protocol + '//' +
                    window.location.host + '/File/UploadImage',
            data: form_data,
            type: 'post',
            dataType: 'json',
            cache: false,
            contentType: false,
            processData: false,
            success: function (data, textStatus) {
                $('#' + imageId).attr("src", window.location.protocol + '//' +
                    window.location.host + '/File/Image/' + data.filename);
                $('#' + hiddenInputId).val(data.filename);
            },
            error: function (data, textStatus) {
                $('#' + imageId).attr("src", window.location.protocol + '//' +
                    window.location.host + '/File/Image/' + data.filename);
                $('#' + hiddenInputId).val(data.filename);
            }
        })
    },

    uploadDbf: function (fileInput, url, container, formId) {
        var file_data = $('#' + fileInput).prop("files")[0];
        var form_data = new FormData();

        if (formId) {
            $('#' + formId).find('input').each(
                function () {
                    var input = $(this);
                    form_data.append(input[0].id, input[0].value);
                });
            $('#' + formId).find('select').each(
                function () {
                    var select = $(this);
                    form_data.append(select[0].id, $(select[0]).val());
                });
        }

        form_data.append("file", file_data)
        $('.loadingOverlay').show();
        $.ajax({
            url: window.location.protocol + '//' +
                    window.location.host + url,
            data: form_data,
            type: 'post',
            dataType: 'json',
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                $('#' + container).html(data.responseText);
                $('.loadingOverlay').hide();
            },
            error: function (data) {
                $('#' + container).html(data.responseText);
                $('.loadingOverlay').hide();
            }
        })
    },

    getQueryVariable: function getQueryVariable(variable) {
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            if (pair[0] == variable) { return pair[1]; }
        }
        return (false);
    },

    callSavePhoto: function callSavePhoto() {
        try {
            var result = StudioUEC_COM.GetPhotoBase64();
            var filename = FileProcessor.getQueryVariable("photoName");
            $.ajax({
                type: 'POST',
                url: window.location.protocol + '//' +
                    window.location.host + '/File/UploadImage?filename=' + filename,
                async: false,
                data: { source: result },
                success: function (data) {
                    if (data.answer !== 'OK') {
                        alert('Ошибка загрузки фотографии');
                    } else {
                        alert('Фотография загружена');
                    }
                }
            });
        }
        catch (e) {
            alert(e.message);
        }
    },
    callSaveSign: function callSaveSign() {
        try {
            var result = StudioUEC_COM.GetSignatureBase64();
            var filename = FileProcessor.getQueryVariable("signatureName");
            $.ajax({
                type: 'POST',
                url: window.location.protocol + '//' +
                    window.location.host + '/File/UploadImage?filename=' + filename,
                async: false,
                data: { source: result },
                success: function (data) {
                    if (data.answer !== 'OK') {
                        alert('Ошибка загрузки подписи');
                    } else {
                        alert('Подпись загружена');
                    }
                }
            });
        }
        catch (e) {
            alert(e.message);
        }
    },

    reloadImage: function (id) {
        return function (id) {
            var $img = $("#" + id);
            $img.attr("src", $img.attr("src").split("?")[0] + "?" + Math.random());
        }(id)
    },

    clearImage: function (id) {
        return function (id) {
            var $img = $("#" + id);
            var filename = $img.attr("src").split("Image/")[1].split("?")[0];
            $.ajax({
                type: 'POST',
                url: window.location.protocol + '//' +
                    window.location.host + '/File/DeleteFile?filename=' + filename,
                async: false,
                data: { filename: filename },
                success: function (data) {
                    if (data.answer === 'OK') {
                        $img.attr("src", '/File/Image/' + data.filename + "?" + Math.random());
                    }
                }
            });
        }(id)
    },

    callSavePhotoAndSignature: function callSavePhotoAndSignature() {
        try {
            FileProcessor.callSavePhoto();
            FileProcessor.callSaveSign();
            window.close();
        }
        catch (e) {
            alert(e.message);
        }
    },

    loadImagesFromStudioUEC: function loadImagesFromStudioUEC(signature, photo) {
        try {
            var uecUrl = window.location.protocol + '//' +
                    window.location.host + '/File/Upload?signatureName=' + signature + '&photoName=' + photo;
            var uecWindow = window.open(uecUrl, '_blank');
        }
        catch (e) {
            alert(e.message);
        }
    },
}