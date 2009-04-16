function Collapse(id)
{
	var node = document.getElementById(id);
	var value = node.style.display;

	if (value == "none") {
		node.style.display = "";
	} else {
		node.style.display = "none";
	}
}
