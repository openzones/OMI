$(document).ready(function () {
    function close_accordion_section() {
        $('.accordion .accordion-section-title').removeClass('active');
        $('.accordion .accordion-section-content').slideUp(300).removeClass('open');
    }

    $('.accordion-section-title').click(function (e) {
        if ($(this).is('.active')) {
            close_accordion_section();
        } else {
            var container = $(this).next();
            var containerId = container.attr('id');
            var elementId = containerId.match(/\d+/)[0];
            var getUrl = container.attr('getUrl');
            PartialProcessor.getPartial(getUrl, containerId, open(containerId))
        }
    });
    
    function open(containerId) {
        var container = $('#' + containerId);
        var prev = container.prev();

        if (prev.is('.active')) {
            close_accordion_section();
        } else {
            close_accordion_section();
            prev.addClass('active');
            container.slideDown(300).addClass('open');
        }
    }
});