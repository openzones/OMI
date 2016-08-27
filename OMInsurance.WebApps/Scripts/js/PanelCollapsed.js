$(document).on('click', '.panel-heading .clickable', function (e) {
    var $this = $(this);
    if (!$this.hasClass('panel-collapsed')) {
        $this.closest('.panel').children('.panel-body').slideUp();
        $this.addClass('panel-collapsed');
        $this.next().find('i').removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-up');
    } else {
        $this.closest('.panel').children('.panel-body').slideDown();
        $this.removeClass('panel-collapsed');
        $this.next().find('i').removeClass('glyphicon-chevron-up').addClass('glyphicon-chevron-down');
    }
})

$(document).on('click', '.uncollapseAll', function (e) {
    var $this = $(this);
    $this.hide();
    $this.siblings('.collapseAll').show();
    var container = $this.closest('.container');
    var panels = container.children('.panel');
    container.find('.panel').each(function () {
        $(this).children('.panel-body').slideDown();
        $(this).children('h3').removeClass('panel-collapsed');
        $(this).children('i').removeClass('glyphicon-chevron-up').addClass('glyphicon-chevron-down');
    });
})

$(document).on('click', '.collapseAll', function (e) {
    var $this = $(this);
    $this.hide();
    $this.siblings('.uncollapseAll').show();
    var container = $this.closest('.container');
    var panels = container.children('.panel');
    container.find('.panel').each(function () {
        $(this).children('.panel-body').slideUp();
        $(this).children('h3').addClass('panel-collapsed');
        $(this).children('i').addClass('glyphicon-chevron-up').removeClass('glyphicon-chevron-down');
    });
})