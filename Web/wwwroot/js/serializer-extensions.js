$.extend($.serializeJSON, { 
    parseValue: function(valStr, inputName, type, opts, $form, inputData) {
      var f, parsedVal;
      f = $.serializeJSON;
      parsedVal = valStr; // if no parsing is needed, the returned value will be the same

      if (opts.typeFunctions && type && opts.typeFunctions[type]) { // use a type if available
        parsedVal = opts.typeFunctions[type](valStr, $form, inputData);
      } else if (opts.parseNumbers && f.isNumeric(valStr)) { // auto: number
        parsedVal = Number(valStr);
      } else if (opts.parseBooleans && (valStr === "true" || valStr === "false")) { // auto: boolean
        parsedVal = (valStr === "true");
      } else if (opts.parseNulls && valStr == "null") { // auto: null
        parsedVal = null;
      } else if (opts.typeFunctions && opts.typeFunctions["string"]) { // make sure to apply :string type if it was re-defined
        parsedVal = opts.typeFunctions["string"](valStr);
      }
      
      // Custom parse function: apply after parsing options, unless there's an explicit type.
      if ((opts.parseWithFunction && !type)) {
        parsedVal = opts.parseWithFunction(parsedVal, inputName);
      }

      return parsedVal;
    }
});

// Extend/Override the 'jquery.serializeJSON' functional
$.fn.serializeJSON = function (options) {
    var f, $form, opts, formAsArray, serializedObject, name, value, parsedValue, _obj, nameWithNoType, type, keys, skipFalsy;
    f = $.serializeJSON;
    $form = this; // NOTE: the set of matched elements is most likely a form, but it could also be a group of inputs
    opts = f.setupOpts(options); // calculate values for options {parseNumbers, parseBoolens, parseNulls, ...} with defaults

    // Use native `serializeArray` function to get an array of {name, value} objects.
    formAsArray = $form.serializeArray();
    f.readCheckboxUncheckedValues(formAsArray, opts, $form); // add objects to the array from unchecked checkboxes if needed

    // Convert the formAsArray into a serializedObject with nested keys
    serializedObject = {};
    $.each(formAsArray, function (i, obj) {
        name = obj.name; // original input name
        value = obj.value; // input value
        _obj = f.extractTypeAndNameWithNoType(name);
        nameWithNoType = _obj.nameWithNoType; // input name with no type (i.e. "foo:string" => "foo")
        type = _obj.type || obj.dataType; // [CUSTOM](added dataType) [DEFAULT](type defined from the input name in :type colon notation)
        if (!type) type = f.attrFromInputWithName($form, name, 'data-value-type');
        f.validateType(name, type, opts); // make sure that the type is one of the valid types if defined

        if (type !== 'skip') { // ignore inputs with type 'skip'
            keys = f.splitInputNameIntoKeysArray(nameWithNoType);
            parsedValue = f.parseValue(value, name, type, opts, $form, obj); // convert to string, number, boolean, null or customType

            skipFalsy = !parsedValue && f.shouldSkipFalsy($form, name, nameWithNoType, type, opts); // ignore falsy inputs if specified
            if (!skipFalsy) {
                f.deepSet(serializedObject, keys, parsedValue, opts);
            }
        }
    });
    return serializedObject;
};
