;

var FoolProofCore = function () { };

FoolProofCore.is = function (value1, operator, value2, passOnNull, dataType) {
	function isNullish (input) {
        return input == null || input == undefined || input == "";
    }

    passOnNull = (/true/i).test(passOnNull + "");
    if (passOnNull) {
        var value1nullish = isNullish(value1);
        var value2nullish = isNullish(value2);

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

	if (isBool(value1) || isBool(value2)) {
		value1 = getBool(value1);
		value2 = getBool(value2);
	}
	else if (isNumeric(value1) || isNumeric(value2)) {
		value1 = parseFloat(value1);
		value2 = parseFloat(value2);
	}
	else if (isDate(value1) || isDate(value2)) {
		value1 = Date.parse(value1);
		value2 = Date.parse(value2);
	}
	else if (isTime(value1) || isTime(value2)) {
		value1 = getTime(value1);
		value2 = getTime(value2);
	}
	else if (dataType && dataType !== DataTypes.string
			 && (!isNullish(value1) || !isNullish(value2)))
		return false; //Some of the provided values do not correspond with the specified data type

    switch (operator) {
		case "EqualTo":
			return value1 == value2;
		case "NotEqualTo":
			return value1 != value2;
		case "GreaterThan":
			return value1 > value2;
		case "LessThan":
			return value1 < value2;
		case "GreaterThanOrEqualTo":
			return value1 >= value2;
		case "LessThanOrEqualTo":
			return value1 <= value2;
		case "RegExMatch":
			return value2 && (new RegExp(value2).test(value1));
		case "NotRegExMatch":
			return value2 && !(new RegExp(value2).test(value1));
		case "In":
			try {
				var valArr = JSON.parse(value2);
				if (typeof (valArr) == "object")
					for (var key in valArr)
						if (valArr[key] == value1)
							return true;
			} catch (e) { }

			return value1 == value2;
		case "NotIn":
			try {
				var valArr = JSON.parse(value2);
				if (typeof (valArr) == "object")
					for (var key in valArr)
						if (valArr[key] == value1)
							return false;
			} catch (e) { }

			return value1 != value2;
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