<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<script type="text/javascript" src="scripts.js"></script>
	<link type="text/css" href="styles.css" rel="stylesheet"/>
	<link rel="icon" type="image/png" href="imgs/favicon.png"/>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	<title>SolarBeam : Installation on Unix</title>
</head>
<body>

	<? include("header.php"); ?>

	<? include("menu.php"); ?>

	<div class="content">
		<h1>Installation on Unix</h1>
		SolarBeam is a portable application and does not need to be installed. You can place it anywhere on the filesystem and run it from there. SolarBeam can also set up launchers for itself in the Applications menu.

        <h2>Dependencies</h2>
        <p>On <b>Ubuntu</b> you will need to install the packages:
        <ul>
            <li>mono-runtime</li>
            <li>libmono-i18n2.0-cil</li>
            <li>libmono-winforms2.0-cil</li>
        </ul>
        </p>

		<h2>Download</h2>
		<p>First download a SolarBeam release from this website.</p>
		<img src="imgs/guide/unix_0.png" alt="unix_0"/>
	
		<h2>Extracting</h2>
		<p>Open a terminal and extract the .zip file with unzip.</p>
		<img src="imgs/guide/unix_1.png" alt="unix_1"/>
	
		<p>The extracted directory contains the program and can be placed anywhere on the filesystem.</p>

		<p>Inside the directory you'll find the program. <i>solarbeam.exe</i> is the main executable. Run it using mono.</p>
		<img src="imgs/guide/unix_2.png" alt="unix_2"/>

		<h2>Creating shortcuts</h2>
		<p>Once you've started the program you can create launchers to SolarBeam in the Applications menu by choosing <i>Help &gt; Create shortcuts...</i>.</p>
		<img src="imgs/guide/unix_3.png" alt="unix_3"/>

		<p>Click <i>Create</i>.</p>
		<img src="imgs/guide/unix_4.png" alt="unix_4"/>

		<p>If you are running <a href="http://gnome.org">Gnome</a> you should now see the launcher in the menu.</p>
		<img src="imgs/guide/unix_5.png" alt="unix_5"/>

		<p>If you are running <a href="http://kde.org">KDE</a> you should now see the launcher in the menu.</p>
		<img src="imgs/guide/unix_6.png" alt="unix_6"/>
	</div>

</body>
</html>
