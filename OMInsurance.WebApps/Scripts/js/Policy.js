var PolicyProcessor = PolicyProcessor || {
    bindOKATOandOGRN: function (policyContainerId) {
        $("#" + policyContainerId + " .smoLabel").autocomplete({
            dataType: 'json',
            source: function (request, response) {
                var autocompleteUrl = window.location.origin + '/FIAS/GetSMO?name=' + request.term;
                $.ajax({
                    url: autocompleteUrl,
                    type: 'GET',
                    cache: false,
                    dataType: 'json',
                    success: function (json) {
                        response($.map(json, function (data, id) {
                            return {
                                label: data.Shortname + " " + data.TerritoryName,
                                value: data.Id,
                                data_okato: data.OKATO,
                                data_ogrn: data.OGRN,
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
                    $("#" + policyContainerId + " .smoLabel").val("");
                    $("#" + policyContainerId + " .smoValue").val("");
                }
            },
            select: function (event, ui) {
                $("#" + policyContainerId + " .smoLabel").val(ui.item.label);
                $("#" + policyContainerId + " .smoValue").val(ui.item.value);
                $("#" + policyContainerId + " .okatoValue").attr("value", ui.item.data_okato);
                $("#" + policyContainerId + " .ogrnValue").attr("value", ui.item.data_ogrn);
                return false;
            },
            focus: function (event, ui) {
                event.preventDefault();
                $(this).val(ui.item.label);
            }
        });
    },
}

PolicyProcessor.bind = function (policyContainerId) {
    PolicyProcessor.bindOKATOandOGRN(policyContainerId);
}