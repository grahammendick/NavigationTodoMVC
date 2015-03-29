var newDiv = document.createElement("div"); 
var newContent = document.createTextNode("test"); 
newDiv.appendChild(newContent);
var currentDiv = document.getElementById("test"); 
document.body.insertBefore(newDiv, currentDiv); 
document.getElementById('test').addEventListener('click', function(){
	console.log('test');
});
