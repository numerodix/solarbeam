<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<script type="text/javascript" src="scripts.js"></script>
	<link type="text/css" href="styles.css" rel="stylesheet"/>
	<link rel="icon" type="image/png" href="imgs/favicon.png"/>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	<title>SolarBeam : Home</title>
</head>
<body>

	<? include("header.php"); ?>

	<? include("menu.php"); ?>

	<div class="content">
		<h1>Home</h1>
		<p>
		<a href="imgs/ss/linux.png">
			<img src="imgs/ss/linux_t.png" alt="screenshot" style="float: right; padding-left: 10px;"/>
		</a>
		SolarBeam is an application for drawing solar diagrams. That is to say a diagram that shows the position of the sun at a given time. For a description, read the <a href="explanation.php">explanation</a>.</p>

		<h2>Features</h2>
		<ul>
			<li>2000 predefined locations worldwide with accurate geographic coordinates and timezone information. (User defined locations are also supported.)</li>
			<li>Daylight saving time information included where applicable.</li>
			<li>Interactive input allows tracing paths along the diagram.</li>
			<li>Diagrams are scalable to any size for inclusion in documents.</li>
			<li>Command line client for automated querying.</li>
			<li>SolarBeam is <a href="http://www.gnu.org/philosophy/free-sw.html">Free Software</a>. That is, you can freely modify it, improve it etc.</li>
			<li>Runs on any major operating system with .NET/Mono.</li>
		</ul>

		<h2>About</h2>
		<p>SolarBeam is written by Martin Matusiak &lt;numerodix@gmail.com&gt;.</p>

		<p> This application was developed in cooperation with the Norwegian University of Science and Technology (NTNU), Norway, and sponsored by The Research Council of Norway.</p>

		<p>Coordinator: Professor Barbara Matusiak, <a href="http://www.ntnu.no/bff/english">Department of Architectural Design, Form and Colour Studies</a>, NTNU.</p>
	</div>

</body>
</html>
