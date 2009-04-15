<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<link type="text/css" href="styles.css" rel="stylesheet"/>
	<link rel="icon" type="image/png" href="imgs/favicon.png"/>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	<title>SolarBeam : Explanation</title>
</head>
<body>

	<? include("header.php"); ?>

	<? include("menu.php"); ?>

	<div class="content">
		<h1>Explanation</h1>

		<p>A solar diagram shows the position (and in general, the trajectory) of the <a href="http://en.wikipedia.org/wiki/The_sun">sun</a> at a given time of day, for a given day of the year. It answers the questions:</p>
		<ul>
			<li>Where is the sun in the sky?</li>
			<li>How high in the sky is the sun at noon?</li>
			<li>When does the sun rise and set?</li>
			<li>How many hours of sunlight are there in January?</li>
			<li>How different (in terms of sunlight) is a day in summer from a day in winter?</li>
		</ul>
		<p>As you might expect, the answers differ depending on where you are on Earth.</p>

		<h2>Reading the diagram</h2>
		<p>Suppose there is an observer somewhere on Earth. A solar diagram is then a chart of where the observer sees the sun in the sky, how high (elevation) and in what <a href="http://en.wikipedia.org/wiki/Cardinal_direction">direction</a> he has to look (<a href="http://en.wikipedia.org/wiki/Azimuth">azimuth</a>).
		</p>

		<p>This diagram shows the sun's trajectory during some particular day of the year. Along the trajectory (the red line) there are particular points of interest:</p>
		<ul>
			<li>The sun rises in the East. This is shown in the diagram by the fact that the angle of elevation exceeds 0&deg;.</li>
			<li>The sun sets in the West. This means the angle of elevation drops below 0&deg;.</li>
			<li>The highest elevation of the sun in a day is called the <a href="http://en.wikipedia.org/wiki/Noon#Solar_noon">solar noon</a>.</li>
		</ul>
		<img src="imgs/exp_diagram.png" alt="exp_diagram"/>
		<p>Let us now look at a complete diagram.</p>

		<h2>Case study: Copenhagen</h2>
		<p>A complete diagram shows not only the trajectory for a single day, but also the trajectories for certain characteristic days of the year:</p>
		<ul>
			<li>June 21 is the summer <a href="http://en.wikipedia.org/wiki/Solstice">solstice</a> and the longest day of the year in the <a href="http://en.wikipedia.org/wiki/Northern_hemisphere">Northern Hemisphere</a> (shortest in the Southern).</li>
			<li>December 21 is the winter solstice and the shortest day of the year in the Northern Hemisphere (longest in the Southern).</li>
		</ul>
		<p>A number of days in between are also plotted.</p>

		<p>Each hour of the day is plotted, showing how the sun's position changes over the course of a year. This is called an <a href="http://en.wikipedia.org/wiki/Analemma">analemma curve</a>.</p>

		<p>Any position plotted in the first half of the year appears in <i>blue</i>, while any position in the second half appears in <i>green</i>. In addition, for timezones that use <a href="http://en.wikipedia.org/wiki/Daylight_saving_time">Daylight Saving Time</a> (DST), any position during daylight saving time is shown in a lighter shade of the same color.</p>
		<img src="imgs/exp_copenhagen.png" alt="exp_copenhagen"/>
		<p>In the case of Copenhagen we can see that summer days and winter days differ dramatically in the amount of sunlight (17 hours vs 7 hours at the most). Moreover, the sun reaches a much higher elevation in summer than it does in winter.</p>

		<p>The selected point (the red dot) is 11:00 standard time (12:00 daylight saving time) on October 3.</p>
	</div>

</body>
</html>
