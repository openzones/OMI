var RepresentativeProcessorConstants = RepresentativeProcessorConstants || {
    rfPassportCode: "14",
    selfRepresentativeTypeCode: "1"
}

var RepresentativeProcessor = RepresentativeProcessor || {
    setType: function (ddl) {
        var representative = $(ddl).parents('.representative');
        var documentType = representative.find(".documentType")[0];
        var firstname = representative.find(".firstname")[0];
        var lastname = representative.find(".lastname")[0];
        var series = representative.find(".series")[0];
        var number = representative.find(".number")[0];
        var departmentName = representative.find(".departmentName")[0];
        var birthday = representative.find(".birthday")[0];
        var docDate = representative.find(".issueDate")[0];
        var representativeType = $(ddl).val();
        if (representativeType !== RepresentativeProcessorConstants.selfRepresentativeTypeCode &&
            representativeType !== '') {
            $(documentType).prop('required', true);
            $(firstname).prop('required', true);
            $(lastname).prop('required', true);
            $(series).prop('required', true);
            $(number).prop('required', true);
            $(departmentName).prop('required', true);
            $(docDate).prop('required', true);
        } else {
            $(documentType).prop('required', false).removeClass("input-validation-error");
            $(firstname).prop('required', false).removeClass("input-validation-error");
            $(lastname).prop('required', false).removeClass("input-validation-error");
            $(series).prop('required', false).removeClass("input-validation-error");
            $(number).prop('required', false).removeClass("input-validation-error");
            $(departmentName).prop('required', false).removeClass("input-validation-error");
            $(docDate).prop('required', false).removeClass("input-validation-error");
        }
    },
    setPasportMasks: function (ddl) {
        var representative = $(ddl).parents('.representative');
        var series = representative.find(".series")[0];
        var number = representative.find(".number")[0];
        var docType = $(representative.find('.representativeDocumentType')[0]).val();
        var representative = $(ddl).parents('.representative');
        var representativeType = $(ddl).val();
        if (representativeType !== RepresentativeProcessorConstants.selfRepresentativeTypeCode) {
            if (docType === RepresentativeProcessorConstants.rfPassportCode) {
                $(series).mask("99 99");
                $(number).mask("999999");
            } else {
                $(series).unmask();
                $(number).unmask();
            }
        }
    }
}