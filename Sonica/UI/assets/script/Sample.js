
Solution.Ajax.get(
	"tracks",
	{
		PageLength: 10
	}
).then(

	function (data) {
		$("#content").append("<ul>");
		for (var t in data) {
			$("#content ul").append("<li>" + data[t].Title +"</li>");
		}
	}


);

console.log("// Dichiarazione e inizializzazione variabili");

var variable1;
variable1 = "valore 1";

var variable2 = 2;

console.log(variable1);
console.log(variable2);

console.log("// Controllo di flusso");

console.log("// -- if");

if (variable2 == 2) {
	console.log(variable2);
}

if (variable1 == 2) {
	console.log(variable1);
}

console.log("// -- switch");

switch (variable2) {
	case 1:
		console.log(1);
		break;
	case 2:
		console.log(2);
		break;
	case 3:
		console.log(3);
		break;
	case 4:
		console.log(4);
		break;
}

console.log("// -- for");

variable2 = [
	"uno",
	"due",
	"tre",
	"quattro",
	"cinque"
];

for (var i = 0; i < variable2.length; i++) {
	console.log(variable2[i]);
}

for (var i in variable2) {
	console.log(variable2[i]);
}

console.log("// -- while");

var i = 0;
while (variable2[i] != "due" ){
	console.log(variable2[i]);
	i++;
}

