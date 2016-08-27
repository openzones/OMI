var AccountProcessor = AccountProcessor || {
    loadAccountForm: function (accountId) {
        PartialProcessor.getPartial(window.location.origin + '/Account/Edit/' + accountId, 'editUser');
    },
    bindFullnameAutocomplete: function (controlId) {
        $("#" + controlId + " .fullnameAutocomplete").autocomplete({
            dataType: 'json',
            source: function (request, response) {
                var autocompleteUrl = window.location.origin + '/Account/Find?name=' + request.term;
                $.ajax({
                    url: autocompleteUrl,
                    type: 'GET',
                    cache: false,
                    dataType: 'json',
                    success: function (json) {
                        response($.map(json, function (data, id) {
                            return {
                                label: data.Text,
                                value: data.Value
                            };
                        }));
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrown) {
                        console.log('some error occured', textStatus, errorThrown);
                    }
                });
            },
            minLength: 0,
            change: function (event, ui) {
                if (!ui.item) {
                    $("#" + controlId + " .fullnameAutocomplete").val("");
                }
            },
            select: function (event, ui) {
                AccountProcessor.loadAccountForm(ui.item.value);
                return false;
            },
            focus: function (event, ui) {
                event.preventDefault();
                $(this).val(ui.item.label);
            }
        });
    },
    bind: function (controlId) {
        AccountProcessor.bindFullnameAutocomplete(controlId);
    }
}