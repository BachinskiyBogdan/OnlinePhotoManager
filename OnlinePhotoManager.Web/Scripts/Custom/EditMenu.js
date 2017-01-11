$('document').ready(
    $('.edit-block .right').click(
        function() {
            if ($('.left').attr('style') == "display: inline-block")
                $('.left').attr('style', "display: none");
            else
                $('.left').attr('style', "display: inline-block");
        }));