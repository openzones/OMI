$(document).on('change', '.documentType.form-control', function (e) {
    DocumentProcessor.setPasportMasks($(this));
})

$(document).on('change', '.documentType.form-control', function (e) {
    DocumentProcessor.setPasportMasks($(this));
})

$(document).on('blur', '.capitalized', function (e) {
    if ($(this).val() && $(this).val().length > 1) {
        var str = $(this).val();
        str = str.charAt(0).toUpperCase() + str.substring(1, str.length).toLowerCase()
        if (str.indexOf('-') !== -1) {
            var index = str.indexOf('-');

            str = str.substring(0, index)
                + '-'
                + str.charAt(index + 1).toUpperCase()
                + str.substring(index + 2, str.length);
        }
        if (str.indexOf(' ') !== -1) {
            var index = str.indexOf(' ');

            str = str.substring(0, index)
                + ' '
                + str.charAt(index + 1).toUpperCase()
                + str.substring(index + 2, str.length);
        }
        $(this).val(str);
    }
})

$(document).on('blur', '.UpperCase', function (e) {
    if ($(this).val() && $(this).val().length > 1) {
        var str = $(this).val();
        str = str.toUpperCase();
        $(this).val(str);
    }
})

$(document).on('click', '.panel-collapsed', function (e) {
    $(".snils").mask("999-999-999 99");
    $(".phone").mask("(999)999-99-99");
    $(".documentType.form-control").each(function () {
        DocumentProcessor.setPasportMasks($(this));
    });
    $(".representativeType.form-control").each(function () {
        RepresentativeProcessor.setType($(this));
    });
    $(".datepicker").datepicker().mask("99.99.9999");
})


$(document).ajaxComplete(function () {
    $('.multiselect').multiselect({
        placeholder: 'Выберите значения',
        minWidth: 300
    });
    $(".snils").mask("999-999-999 99");
    $(".phone").mask("(999)999-99-99");
    $(".datepicker").datepicker().mask("99.99.9999");
    $(".representativeType.form-control").each(function () {
        RepresentativeProcessor.setType($(this));
    });
    $(".representativeDocumentType.form-control").each(function () {
        RepresentativeProcessor.setPasportMasks($(this));
    });
    $(".responseRow").on("click", function () {
        FundResponseProcessor.openResponseApplySubmitModal($(this));
    });
});

$(document).ready(function () {
    $('.multiselect').multiselect({
        placeholder: 'Выберите значения'
    });
    $(".snils").mask("999-999-999 99");
    $(".phone").mask("(999)999-99-99");
    $(".documentType.form-control").each(function () {
        DocumentProcessor.setPasportMasks($(this));
    });
    $(".representativeType.form-control").each(function () {
        RepresentativeProcessor.setType($(this));
    });
    $(".datepicker").datepicker().mask("99.99.9999");
    $('.toggle').click(function (event) {
        event.preventDefault();
        var target = $(this).attr('href');
        $(target).toggleClass('hidden show');
        var gl = $(this).children('span');
        $(this).children().toggleClass('glyphicon-menu-down glyphicon-menu-up');
    });

    $('.clientVisitRowElement').click(function (event) {
        event.preventDefault();
        var clientVisitId = $(this).closest('tr').attr('data-clientvisitid');
        $(this).closest('table').find('tr').css('background-color', 'white');
        $(this).closest('tr').css('background-color', '#c6d2f5');
        PartialProcessor.getPartial(
            window.location.origin + '/FundRequestProcessing/FundResponse/' + clientVisitId,
            'fundResponcesContainer');
    });
    $(".checkAll").change(function () {
        //$(this).closest("ul").find("input:checkbox").prop('checked', $(this).prop("checked"));
        $(this).closest("ul").find(".generalField, .otherField").prop('checked', $(this).prop("checked"));
    });
    $(".checkGeneralFields").change(function () {
        $(this).closest("ul").find(".generalField").prop('checked', $(this).prop("checked"));
    });
});