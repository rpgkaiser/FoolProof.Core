;

var FoolProofCore = function() { };

FoolProofCore.is = function (value1, operator, value2, passOnNull) {
    if (passOnNull) {
        var isNullish = function (input) {
            return input == null || input == undefined || input == "";
        };

        var value1nullish = isNullish(value1);
        var value2nullish = isNullish(value2);

        if ((value1nullish && !value2nullish) || (value2nullish && !value1nullish))
            return true;
    }

    var isNumeric = function (input) {
        return (input - 0) == input && input.length > 0;
    };

    var isDate = function (input) {
        var dateTest = new RegExp(/(?=\d)^(?:(?!(?:10\D(?:0?[5-9]|1[0-4])\D(?:1582))|(?:0?9\D(?:0?[3-9]|1[0-3])\D(?:1752)))((?:0?[13578]|1[02])|(?:0?[469]|11)(?!\/31)(?!-31)(?!\.31)|(?:0?2(?=.?(?:(?:29.(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:(?:\d\d)(?:[02468][048]|[13579][26])(?!\x20BC))|(?:00(?:42|3[0369]|2[147]|1[258]|09)\x20BC))))))|(?:0?2(?=.(?:(?:\d\D)|(?:[01]\d)|(?:2[0-8])))))([-.\/])(0?[1-9]|[12]\d|3[01])\2(?!0000)((?=(?:00(?:4[0-5]|[0-3]?\d)\x20BC)|(?:\d{4}(?!\x20BC)))\d{4}(?:\x20BC)?)(?:$|(?=\x20\d)\x20))?((?:(?:0?[1-9]|1[012])(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]\d|2[0-3])(?::[0-5]\d){1,2})?$/);

        return dateTest.test(input);
    };

    var isBool = function (input) {
        return input === true || input === false || input === "true" || input === "false";
    };

    if (isDate(value1)) {
        value1 = Date.parse(value1);
        value2 = Date.parse(value2);
    }
    else if (isBool(value1)) {
        if (value1 == "false") value1 = false;
        if (value2 == "false") value2 = false;
        value1 = !!value1;
        value2 = !!value2;
    }
    else if (isNumeric(value1)) {
        value1 = parseFloat(value1);
        value2 = parseFloat(value2);
    }

    switch (operator) {
		case "EqualTo":
			if (value1 == value2) return true;
			break;
		case "NotEqualTo":
			if (value1 != value2) return true;
			break;
		case "GreaterThan":
			if (value1 > value2) return true;
			break;
		case "LessThan":
			if (value1 < value2) return true;
			break;
		case "GreaterThanOrEqualTo":
			if (value1 >= value2) return true;
			break;
		case "LessThanOrEqualTo":
			if (value1 <= value2) return true;
			break;
		case "RegExMatch":
			return (new RegExp(value2)).test(value1);
		case "NotRegExMatch":
			return !(new RegExp(value2)).test(value1);
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