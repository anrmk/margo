$.fn.disabled = function () {
    $(this).attr('disabled', 'disabled');
}

$.fn.enabled = function () {
    $(this).removeAttr('disabled');
}