var FundResponseProcessor = FundResponseProcessor || {
    openResponseApplySubmitModal: function (t) {
        $(t).parents('.fundResponsePackage').find('tr').css('background-color', 'white');
        $(t).closest('tr').css('background-color', '#c6d2f5');
        var clientVisitGroupId = $(t).attr('data-client-visit-group-id');
        var responseId = $(t).attr('data-response-id');
        $('#ClientVisitGroupId').val(clientVisitGroupId);
        $('#ResponseId').val(responseId);
        $('#modal').modal('show');
    },
    successSubmitResponse: function (t) {
        $('#modal').find("input:checkbox").prop('checked', false);
        $('#modal').modal('hide');
    },
    errorSubmitResponse: function (t) {
        $('#modal').find("input:checkbox").prop('checked', false);
        $('#modal').modal('hide');
    },
    showHidedResponses: function (t) {
        var fundResponsePackage = $(t).closest('.fundResponsePackage');
        var rows = $(fundResponsePackage).find('tr');
        rows.show();
    },
    markAllReadyToSubmit: function (t) {
        $('.loadingOverlay').show();
        var value;
        var $checkbox = $(t);
        if ($checkbox.prop('checked')) {
            value = true;
        } else {
            value = false;
        }
        var items = $(".readyForSubmitCheckBox");
        var ids = $.map(items, function (elem) { return $(elem).closest('tr').attr('data-clientvisitid'); });

        $.ajax({
            type: 'POST',
            data: {ids: ids},
            url: window.location.origin + '/FundRequestProcessing/ClientVisits_SetReadyToFundSubmitRequest?isReady=' + value,
            success: function (data) {
                if (data.result === "OK") {
                    $checkbox.prop('checked', value);
                    var elements;
                    for (var i = 0; i < data.successIds.length; i++) {
                        $("[data-clientvisitid=" + data.successIds[i] + "]").find(".readyForSubmitCheckBox").prop('checked', value);
                    }
                    $('.loadingOverlay').hide();
                } else {
                    $('.loadingOverlay').hide();
                }
            },
            error: function (data) {
                $('.loadingOverlay').hide();
            }
        });

    },
    setReadyToFundSubmitRequest: function (clientVisitId, cb, value) {
        var value;
        var $checkbox = $(cb);
        if ($checkbox.prop('checked')) {
            value = true;
        } else {
            value = false;
        }
        $.ajax({
            type: 'POST',
            url: window.location.origin + '/FundRequestProcessing/ClientVisit_SetReadyToFundSubmitRequest?id=' + clientVisitId + '&isReady=' + value,
            success: function (data) {
                if (data.result === "OK") {
                    $checkbox.prop('checked', value);
                } else {
                    alert(data.message);
                }
            },
            error: function (data) {
                alert(data.message);
            }
        });
    },
    setIsHide: function (ClientID, cb, value) {
        var value;
        var $checkbox = $(cb);
        if ($checkbox.prop('checked')) {
            value = true;
        } else {
            value = false;
        }
        $.ajax({
            type: 'POST',
            url: window.location.origin + '/Check/ClientId_SetIsHide?id=' + ClientID + '&isReady=' + value,
            success: function (data) {
                if (data.result === "OK") {
                    $checkbox.prop('checked', value);
                } else {
                    alert(data.message);
                }
            },
            error: function (data) {
                alert(data.message);
            }
        });
    },

    changeDeliveryPoint: function (VisitId) {
        var dp = "#DeliveryPoint" + VisitId
        var dc = "#DeliveryCenter" + VisitId
        var dcHidden = "#DeliveryCenterHidden" + VisitId

        $.ajax({
            type: 'POST',
            url: window.location.origin + '/ClientVisit/ChangeDeliveryPoint?id=' + $(dp).val(),
            success: function (data) {
                if (data.result === "OK") {
                    $(dc).prop("value", data.message);
                    $(dcHidden).prop("value", data.message);
                } else {
                    //если какие-то ошибки - ничего не делаем
                    //nothing
                    alert(data.message);
                }
            },
            error: function (data) {
                alert(data.message);
            }
        });
    },

    checkBSO: function (id, visitGroupId, VisitId) {
        var str = "#TemporaryPolicyNumber" + VisitId
        
        $.ajax({
            type: 'POST',
            url: window.location.origin + '/ClientVisit/CheckBSOinVisitGroup?id=' + id + '&visitGroupId=' + visitGroupId + '&bso=' + $(str).val(),
            success: function (data) {
                if (data.result === "OK") {
                    //nothing
                    //alert(data.message);
                } else {
                    alert(data.message);
                }
            },
            error: function (data) {
                alert(data.message);
            }
        });
    },
    setDifficultCase: function (clientVisitId, cb, value) {
        var value;
        var $checkbox = $(cb);
        if ($checkbox.prop('checked')) {
            value = true;
        } else {
            value = false;
        }
        $.ajax({
            type: 'POST',
            url: window.location.origin + '/FundRequestProcessing/ClientVisit_SetDifficultCase?id=' + clientVisitId + '&isDifficult=' + value,
            success: function (data) {
                if (data.result === "OK") {
                    $checkbox.prop('checked', value);
                } else {
                    alert(data.message);
                }
            },
            error: function (data) {
                alert(data.message);
            }
        });
    }
}