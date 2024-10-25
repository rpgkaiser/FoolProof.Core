;

var FoolProofCore = function () { };

FoolProofCore.is = function (compValue, operator, dependValue, passOnNull, dataType) {
	function isNullish (input) {
        return input == null || input == undefined || input == "";
    }

    passOnNull = (/true/i).test(passOnNull + "");
    if (passOnNull) {
        var value1nullish = isNullish(compValue);
        var value2nullish = isNullish(dependValue);

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

	if (isBool(compValue) || isBool(dependValue)) {
		compValue = getBool(compValue);
		dependValue = getBool(dependValue);
	}
	else if (isNumeric(compValue) || isNumeric(dependValue)) {
		compValue = parseFloat(compValue);
		dependValue = parseFloat(dependValue);
	}
	else if (isDate(compValue) || isDate(dependValue)) {
		compValue = Date.parse(compValue);
		dependValue = Date.parse(dependValue);
	}
	else if (isTime(compValue) || isTime(dependValue)) {
		compValue = getTime(compValue);
		dependValue = getTime(dependValue);
	}
	else if (dataType && dataType !== DataTypes.string
			 && (!isNullish(compValue) || !isNullish(dependValue)))
		return false; //Some of the provided values do not correspond with the specified data type

    switch (operator) {
		case "EqualTo":
			return compValue == dependValue;
		case "NotEqualTo":
			return compValue != dependValue;
		case "GreaterThan":
			return compValue > dependValue;
		case "LessThan":
			return compValue < dependValue;
		case "GreaterThanOrEqualTo":
			return compValue >= dependValue;
		case "LessThanOrEqualTo":
			return compValue <= dependValue;
		case "RegExMatch":
			return dependValue && (new RegExp(dependValue).test(compValue));
		case "NotRegExMatch":
			return dependValue && !(new RegExp(dependValue).test(compValue));
		case "In":
			try {
				var valArr = JSON.parse(dependValue);
				if (typeof (valArr) == "object")
					for (var key in valArr)
						if (valArr[key] == compValue)
							return true;
			} catch (e) { }

			return compValue == dependValue;
		case "NotIn":
			try {
				var valArr = JSON.parse(dependValue);
				if (typeof (valArr) == "object")
					for (var key in valArr)
						if (valArr[key] == compValue)
							return false;
			} catch (e) { }

			return compValue != dependValue;
    }

    return false;
};

FoolProofCore.getId = function (element, dependentPropety) {
    var pos = element.id.lastIndexOf("_") + 1;
    return element.id.substr(0, pos) + dependentPropety.replace(/\./g, "_");
};

FoolProofCore.getName = function (element, dependentPropety) {
    var pos = element.name.lastIndexOf(".") + 1;
    return element.name.substr(0, pos) + dependentPropety;
};