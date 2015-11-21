
$(window).scroll(function () {
    var sticky = $('.recommendation-text-fixed-container'),
      scroll = $(window).scrollTop();
    var width = $(window).width();
    if (scroll >= 60 && width >= 768) sticky.addClass('fixed');
    else if (scroll >= 360 && width < 768) sticky.addClass('fixed');
    else sticky.removeClass('fixed');
});
