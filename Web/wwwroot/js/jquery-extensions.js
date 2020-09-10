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

$.fn.hasData = function (key) {
    return (typeof $(this).data(key) != 'undefined');
};

const rCRLF = /\r?\n/g,
      rsubmitterTypes = /^(?:submit|button|image|reset|file)$/i, 
      rsubmittable = /^(?:input|select|textarea|keygen)/i,
      rcheckableType = /^(?:checkbox|radio)$/i
$.fn.serializeArray = function() {
  return this
    .map(function() {
      // Can add propHook for "elements" to filter or add form elements
      var elements = $.prop( this, "elements" );
      return elements ? $.makeArray( elements ) : this; 
    })
    .filter(function() {
      var type = this.type;

      // Use .is( ":disabled" ) so that fieldset[disabled] works
      return this.name && !$( this ).is( ":disabled" ) &&
        rsubmittable.test( this.nodeName ) && !rsubmitterTypes.test( type ) &&
        (this.checked || !rcheckableType.test( type ));
    })
    .map(function(_i, elem) {
      var val = $(this).val();

      if (val == null) {
        return null;
      }


      if (Array.isArray(val)) {
        return $.map(val, function(val) {
          return { name: elem.name, value: val.replace( rCRLF, "\r\n" ), self: elem, dataType: $(elem).data("value-type") }; // [CUSTOM] Added properties: self, dataType
        });
      }

      return { name: elem.name, value: val.replace(rCRLF, "\r\n"), self: elem, dataType: $(elem).data("value-type") }; // [CUSTOM] Added properties: self, dataType
    }).get();
}

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