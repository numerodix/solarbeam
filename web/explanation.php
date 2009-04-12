<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html>
<head>
	<link type="text/css" href="styles.css" rel="stylesheet">
	<title>SolarBeam</title>
</head>
<body>

	<? //include("header.php"); ?>

	<? include("menu.php"); ?>

	<div class="content">
		<h1>Explanation</h1>

		<p>A solar diagram shows the position (and in general, the trajectory) of the <a href="http://en.wikipedia.org/wiki/The_sun">sun</a> at a given time of day, for a given day of the year. It answers the questions:
		<ul>
			<li>Where is the sun in the sky?</li>
			<li>How high in the sky is the sun at noon?</li>
			<li>When does the sun rise and set?</li>
			<li>How many hours of sunlight are there in January?</li>
			<li>How different (in terms of sunlight) is a day in summer from a day in winter?</li>
		</ul>
		As you might expect, the answers differ depending on where you are on Earth.</p>

		<h2>Reading the diagram</h2>
		<p>Suppose there is an observer somewhere on Earth. A solar diagram is then a chart of where the observer sees the sun in the sky, how high (elevation) and in what <a href="http://en.wikipedia.org/wiki/Cardinal_direction">direction</a> he has to look (azimuth).
		</p>
		<p>This diagram shows the sun's trajectory during some particular day of the year. Along the trajectory (the red line) there are particular points of interest:
		<ul>
			<li>The sun rises in the East. This is shown in the diagram by the fact that the angle of elevation exceeds 0°.</li>
			<li>The sun sets in the West. This means the angle of elevation drops below 0°.</li>
			<li>The highest elevation of the sun in a day is called the <a href="http://en.wikipedia.org/wiki/Noon#Solar_noon">solar noon</a>.</li>
		</ul>
		</p>
		<img src="imgs/exp_diagram.png">
	</div>

</body>
</html>
