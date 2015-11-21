$(window).scroll(function(){
    var sticky = $('.recommendation-text-fixed-container'),
      scroll = $(window).scrollTop();

  if (scroll >= 60) sticky.addClass('fixed');
  else sticky.removeClass('fixed');
});
