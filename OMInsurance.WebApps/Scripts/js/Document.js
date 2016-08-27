var DocumentProcessorConstants = DocumentProcessorConstants || {
    rfPassportCode: "14", //Паспорт гражданина Российской Федерации (тип 14)
                         
    // я не стал придумывать кодам названия, т.к. лень из этих длинных словосочетаний делать англ. транскрипцию
                          //
    Code3: "3",           //Свидетельство о рождении, выданное в Российской Федерации (тип 3)
    Code10: "10",         //Свидетельство о регистрации ходатайства о признании иммигранта беженцем (тип 10)
    Code12: "12",         //Удостоверение беженца в Российской Федерации (тип 12)
    Code21: "21",         //Документ иностранного гражданина (тип 21)
    Code9: "9",           //Паспорт иностранного гражданина (тип 9)
    Code11: "11",         //Вид на жительство (тип 11)
    Code13: "13",         //Временное удостоверение личности гражданина Российской Федерации (тип 13)
    Code23: "23",         //Разрешение на временное проживание (тип 23)
    Code24: "24",         //Свидетельство о рождении, выданное не в Российской Федерации (тип 24)
    Code26: "26",         //Удостоверение сотрудника Евразийской экономической комиссии (тип 26)
    Code25: "25",         //Свидетельство о предоставлении временного убежища на территории Российской федерации (тип 25)
    Code1: "1",           //Паспорт гражданина СССР (тип 1)
}

var DocumentProcessor = DocumentProcessor || {
    setPasportMasks: function (ddl) {
        var doc = ddl.parents(".document");
        var series = doc.find(".series")[0];
        var number = doc.find(".number")[0];
        var type;

        if (doc.find(".entityType").length > 0) {
            type = doc.find(".entityType")[0].value;
        }
        // RF Pasport
        if (ddl.val() === DocumentProcessorConstants.rfPassportCode) {
            $(series).mask("99 99", { placeholder: "__ __" });
            $(number).mask("999999");

            //Свидетельство о рождении, выданное в Российской Федерации (тип 3)
        } else if (ddl.val() === DocumentProcessorConstants.Code3) {
            $(series).mask('ZZZZZZZ-ЯЯ', {
                translation: {
                    Z: { pattern: /[IVXLCDMБб\/\\Нн]/, optional: true },
                    Я: { pattern: /[А-Я]/ }
                }, placeholder: " "
            });
            $(number).mask("999999");

            //Свидетельство о регистрации ходатайства о признании иммигранта беженцем (тип 10)
            //Удостоверение беженца в Российской Федерации (тип 12)
        } else if (ddl.val() === DocumentProcessorConstants.Code10 || ddl.val() === DocumentProcessorConstants.Code12) {
            $(series).mask('ZZZZZZZZZZ', { translation: { Z: { pattern: /[0-9A-Za-zА-Яа-я\-\s\/\\]/, optional: true } }, placeholder: " " });
            $(number).mask("9999999999");

            //Документ иностранного гражданина (тип 21)
            //Вид на жительство (тип 11)
            //Временное удостоверение личности гражданина Российской Федерации (тип 13)
            //Разрешение на временное проживание (тип 23)
            //Свидетельство о рождении, выданное не в Российской Федерации (тип 24)
        } else if (ddl.val() === DocumentProcessorConstants.Code21 ||
                    ddl.val() === DocumentProcessorConstants.Code11 ||
                    ddl.val() === DocumentProcessorConstants.Code23||
                    ddl.val() === DocumentProcessorConstants.Code24) {
            $(series).mask('ZZZZZZZZZZ', { translation: { Z: { pattern: /[0-9A-Za-zА-Яа-я\-\s\/\\]/, optional: true } }, placeholder: " " });
            $(number).mask("9999999999");

            //Паспорт иностранного гражданина (тип 9)
        } else if (ddl.val() === DocumentProcessorConstants.Code9) {
            $(series).mask('ZZZZZZZZZZ', { translation: { Z: { pattern: /[0-9A-Za-z\-\sБб\/\\Нн]/, optional: true } }, placeholder: " " });
            $(number).mask("9999999999");

            //Удостоверение сотрудника Евразийской экономической комиссии (тип 26)
        } else if (ddl.val() === DocumentProcessorConstants.Code26) {
            $(series).mask('ZZZ', { translation: { Z: { pattern: /[Бб\/\\Нн]/, optional: true } }, placeholder: "б/н" });
            $(number).mask("999999");

            //Свидетельство о предоставлении временного убежища на территории Российской федерации (тип 25)
        } else if (ddl.val() === DocumentProcessorConstants.Code25) {
            $(series).mask('ZZ', { translation: { Z: { pattern: /[0-9А-Я]/, optional: true } }, placeholder: "__" });
            $(number).mask("9999999");

            //Паспорт гражданина СССР (тип 1)
        } else if (ddl.val() === DocumentProcessorConstants.Code1) {
            $(series).mask('ZZZZZZZ-ЯЯ', {
                translation: {
                    Z: { pattern: /[IVXLCDMБб\/\\Нн]/, optional: true },
                    Я: { pattern: /[А-Я]/, optional: true }
                }, placeholder: " "
            });
            $(number).mask("999999");
        } else {
            $(series).mask('ZZZZZZZZZZ', { translation: { Z: { pattern: /[0-9A-Za-zА-Яа-я\-\s\/\\]/, optional: true } }, placeholder: " " });
            $(number).unmask();
        }

        if (type === "New") {
            $(series).prop('required', true);
            $(number).prop('required', true);
        } else {
            $(series).prop('required', false).removeClass("input-validation-error");
            $(number).prop('required', false).removeClass("input-validation-error");
        }
    }
}