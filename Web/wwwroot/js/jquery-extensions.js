$.fn.disabled = function () {
    $(this).attr('disabled', 'disabled')
        .addClass('disabled');
}

$.fn.enabled = function () {
    $(this).removeAttr('disabled')
        .removeClass('disabled');
}

$.fn.isGuid = function (val) {
    var pattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;
    return pattern.test(val);
}