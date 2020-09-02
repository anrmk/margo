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

var originalSerializeArray = $.fn.serializeArray;
$.fn.serializeArray = function () {
    const mainValues = originalSerializeArray.apply(this);
    const extendedValues = $(this).find('input, select').map(function () {
        return { 
            name: this.name,
            value: this.value,
            self: this, 
            dataType: $(this).data("value-type")
        };
    }).get();
    const mainKeys = $.map(mainValues, function (element) { return element.name; });
    const result = $.grep(extendedValues, function (element) {
        return $.inArray(element.name, mainKeys) !== -1;
    });

    return result.length > 0 ? result : mainValues;
};

Object.defineProperty(Array.prototype, 'chunk', {
  value: function(chunkSize) {
    const array = this;
    return [].concat.apply([],
      array.map(function(elem, i) {
        return i % chunkSize ? [] : [array.slice(i, i + chunkSize)];
      })
    );
  }
});