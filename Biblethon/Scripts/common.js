//$(function () {
//    $(".accordion div").show();
//    setTimeout("$('.accordion div').slideToggle('slow');", 1000);
//    $(".accordion h3").click(function () {
//        $(this).next(".divAccordian").slideToggle("slow").siblings(".divAccordian:visible").slideUp("slow");
//        $(this).toggleClass("current");
//        $(this).siblings("h3").removeClass("current");
//    });
//});
$(function () {
    $('.accordion div').hide();
    $('.accordion h3:first').addClass('current').next().slideDown('slow');
    $('.accordion h3').click(function () {
        if ($(this).next().is(':hidden')) {
            $('.accordion h3').removeClass('current').next().slideUp('slow');
            $(this).toggleClass('current').next().slideDown('slow');
        }
    });
});