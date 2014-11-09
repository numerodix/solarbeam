// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

namespace LibSolar
{
	public static class Constants
	{
		public const string LIBSOLAR_APPTITLE = "LibSolar";
		public const string CONSOLE_APPTITLE = "SolarBeam Console";
		public const string GUI_APPTITLE = "SolarBeam";
		public const string GUI_APPDESC = "An application for drawing solar diagrams";

		public const string VERSION = "1.2.0.0";

		public const string COPYRIGHT = "Copyright (c) 2013 Martin Matusiak <numerodix@gmail.com>";
		
		public const string GUI_FILENAME = "solarbeam.exe";
		public const string WIN_ICON_FILENAME = "icon.ico";
		public const string UNIX_ICON_FILENAME = "icon64.png";
		
		public const string APP_URL = "http://solarbeam.sourceforge.net/";
		public const string URL_HELP = APP_URL + "explanation.php";
		
		// filenames 
		public const string SessionFileExtension = ".solarbeam";
		public const string SessionFileFilter = "*" + SessionFileExtension;
		public const string SessionFileDesc = "SolarBeam sessions";
		
		public const string ImageFileExtension = ".png";
		public const string ImageFileFilter = "*" + ImageFileExtension;
		public const string ImageFileDesc = "Png images";
		
		public const string TextFileExtension = ".txt";
		public const string TextFileFilter = "*" + TextFileExtension;
		public const string TextFileDesc = "Text files";
		
		public const string BinaryFileExtension = ".bin";
		public const string AutoSessionFilename = "autosave" + BinaryFileExtension;
		public const string LocationListFilename = "locations" + BinaryFileExtension;
		
		// shortcuts
		public const string WinShortcutFile = "SolarBeam.url";
		
		public const string UnixShortcutFile = "solarbeam.desktop";
		public const string UnixShortcutGenericName = "Solar diagrammer";
		public const string UnixShortcutTerminal = "false";
		public static readonly string[] UnixShortcutCategories = new string[] {
			"Education", "Science", "Astronomy", "Engineering"};
		
		// paths
		public static readonly string[] UnixGlobalXDGBasePaths = new string[] {
			"/usr/share", "/usr/local/share"};
		public const string UnixXDGApplicationsDirName = "applications";
	}
}
