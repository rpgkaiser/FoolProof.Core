;

var FoolProofCore = function () { };

FoolProofCore.isNullish = function (input, checkArray) {
    return input === null || input === undefined || input === ""
        || (!!checkArray && Array.isArray(input) && input.length == 0);
}   

FoolProofCore.is = function (compValue, operator, dependValue, passOnNull, dataType) {
    var isNullish = FoolProofCore.isNullish;

	passOnNull = (/true/i).test(passOnNull + "");
    if (passOnNull) {
        var value1nullish = isNullish(compValue);
        var value2nullish = isNullish(dependValue, operator === "In");

        if ((value1nullish && !value2nullish) || (value2nullish && !value1nullish))
            return true;
	}

	var DataTypes = {
		string: "String",
		number: "Number",
		bool: "Bool",
		date: "Date",
		time: "Time",
		dateTime: "DateTime"
	};

	var numberRegex = new RegExp(/^[+-]?(?:\d+\.?\d*|\d*\.?\d+)$/);
	var isNumeric = function (input) {
		if (dataType && dataType !== DataTypes.number)
			return false;

		return numberRegex.test(input);
    };

	var isDate = function (input) {
		if (dataType && dataType !== DataTypes.date && dataType !== DataTypes.dateTime)
			return false;

		return input && Date.parse(input);
    };

	var isBool = function (input) {
		if (dataType && dataType !== DataTypes.bool)
			return false;

		return typeof (input) === "boolean"
				|| input === "true"
				|| input === "True"
				|| input === "false"
				|| input === "False";
	};

	var isTime = function (input) {
		if (dataType && dataType !== DataTypes.time)
			return false;

		return input && Date.parse(new Date().toLocaleDateString() + ' ' + input);
	};

	var getTime = function (input) {
		var parsedDate = Date.parse(new Date().toLocaleDateString() + ' ' + input);
		return input && parsedDate
			? new Date(parsedDate).getTime()
			: NaN;
	};

	var getBool = function (input) {
		return input !== false && input !== "false" && input !== 'False' && !!input;
	};

	function convertValue(input, force) {
		if (isBool(input))
			return getBool(input);

		if (isNumeric(input))
			return parseFloat(input);

		if (isDate(input))
			return Date.parse(input);

		if (isTime(input))
			return getTime(input);

		if (!dataType || dataType === DataTypes.string)
			return input || "";

		return force || !isNullish(input) //Invalid value for the given data type
				? undefined 
				: input;
	}

	function verifyInclusion() {
		compValue = convertValue(compValue, !!dataType);
		if (compValue === undefined)
			return false; //The compare value do not correspond with the provided data type

		if (typeof (dependValue) === "string") {
			try { dependValue = JSON.parse(dependValue); }
			catch (e) {}
		}

        if (!Array.isArray(dependValue)) {
            dependValue = convertValue(dependValue, !!dataType);
		    if (dependValue === undefined)
                return false; //The dependant value do not correspond with the provided data type

            dependValue = [dependValue];
        }

        for (var key in dependValue) {
			var currVal = convertValue(dependValue[key], !!dataType);
			if (compValue == currVal)
				return operator == "In" ? true : false;
		}

        return operator != "In" || (!dependValue.length && isNullish(compValue));
	}

	switch (operator) {
		case "EqualTo":
			if (isNullish(compValue) && isNullish(dependValue))
				return true;

			compValue = convertValue(compValue, !!dataType);
			dependValue = convertValue(dependValue, !!dataType);
			if (compValue === undefined || dependValue === undefined)
				return false; //A value do not correspond with the provided data type

			return compValue == dependValue;
		case "NotEqualTo":
			compValue = convertValue(compValue, !!dataType);
			dependValue = convertValue(dependValue, !!dataType);
			if (compValue === undefined || dependValue === undefined)
				return false; //A value do not correspond with the provided data type

			return compValue != dependValue;
		case "GreaterThan":
			compValue = convertValue(compValue, !!dataType);
			dependValue = convertValue(dependValue, !!dataType);
			if (compValue === undefined || dependValue === undefined)
				return false; //A value do not correspond with the provided data type

			return compValue > dependValue;
		case "LessThan":
			compValue = convertValue(compValue, !!dataType);
			dependValue = convertValue(dependValue, !!dataType);
			if (compValue === undefined || dependValue === undefined)
				return false; //A value do not correspond with the provided data type

			return compValue < dependValue;
		case "GreaterThanOrEqualTo":
			if (isNullish(compValue) && isNullish(dependValue))
				return true;

			compValue = convertValue(compValue, !!dataType);
			dependValue = convertValue(dependValue, !!dataType);
			if (compValue === undefined || dependValue === undefined)
				return false; //A value do not correspond with the provided data type

			return compValue >= dependValue;
		case "LessThanOrEqualTo":
			if (isNullish(compValue) && isNullish(dependValue))
				return true;

			compValue = convertValue(compValue, !!dataType);
			dependValue = convertValue(dependValue, !!dataType);
			if (compValue === undefined || dependValue === undefined)
				return false; //A value do not correspond with the provided data type

			return compValue <= dependValue;
		case "RegExMatch":
			return dependValue && (new RegExp(dependValue).test(compValue));
		case "NotRegExMatch":
			return dependValue && !(new RegExp(dependValue).test(compValue));
        case "In":
            return (isNullish(compValue) && isNullish(dependValue, true)) || verifyInclusion();
		case "NotIn":
			return isNullish(compValue, true) || verifyInclusion();
    }

    return false;
};

FoolProofCore.getId = function (element, dependentPropety) {
    var prefixAttr = element.attributes.getNamedItem("data-model-prefix");
    var prefix = prefixAttr ? prefixAttr.value : null;
    if (!prefix) {
        var pos = element.id.lastIndexOf("_") + 1;
        prefix = element.id.substr(0, pos)
    }
    else if (!prefix.endsWith("_"))
        prefix += "_";

    return prefix + dependentPropety.replace(/\./g, "_");
};

FoolProofCore.getName = function (element, dependentPropety) {
    var prefixAttr = element.attributes.getNamedItem("data-model-prefix");
    var prefix = prefixAttr ? prefixAttr.value : null;
    if (!prefix) {
        var pos = element.name.lastIndexOf(".") + 1;
        prefix = element.name.substr(0, pos)
    }
    else if (!prefix.endsWith("_"))
        prefix += ".";

    return prefix + dependentPropety;
};