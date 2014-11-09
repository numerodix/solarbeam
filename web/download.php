<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<script type="text/javascript" src="scripts.js"></script>
	<link type="text/css" href="styles.css" rel="stylesheet"/>
	<link rel="icon" type="image/png" href="imgs/favicon.png"/>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	<title>SolarBeam : Download</title>
</head>
<body>

	<? include("header.php"); ?>

	<? include("menu.php"); ?>

	<div class="content">
		<h1>Download</h1>
		SolarBeam is released under the <a href="http://www.gnu.org/copyleft/gpl.html">GNU General Public License</a>.

		<h2>Releases</h2>
		Not sure how to run/install the program? Look over the instructions for <a href="install_windows.php">Windows</a>, <a href="install_unix.php">Unix</a>.
<? include("releases.php"); ?>

		<h2>System requirements</h2>
		<p>SolarBeam needs .NET 2.0+ or Mono 2.0+ to run and a display resolution of 1024x768 or higher.</p>

		<p>On a Windows system, .NET is most likely already installed (comes preinstalled on Windows Vista), but if not you can download it <a href="http://www.microsoft.com/NET/">here</a>.</p>

		<p>If you are not running Windows (or if you prefer Mono), Mono runs on every major operating system. You can download it <a href="http://www.go-mono.com/mono-downloads/download.html">here</a>.</p>

		<h2>Source code</h2>
		<p>Are you a developer? Source code is to be had from the git repo at:</p>
		<blockquote><p>git://git.code.sf.net/p/solarbeam/code</p></blockquote>
		<p>SolarBeam is written in C&#35;. You can find some code metrics on <a href="https://www.ohloh.net/p/solarbeam">ohloh</a>.</p>
	</div>

</body>
</html>
