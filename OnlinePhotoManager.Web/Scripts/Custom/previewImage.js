function PreviewImg(input)
{
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        $('#cover-preview').attr('style', 'display: block');
        reader.onload = function(e) {
            $('#cover-preview').attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    }
}
