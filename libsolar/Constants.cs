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

		public const string VERSION = "0.6.0.0";

		public const string COPYRIGHT = "Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>";
		
		public const string GUI_FILENAME = "solarbeam.exe";
		public const string ICON_FILENAME = "icon.ico";
		
		public const string APP_URL = "http://solarbeam.sourceforge.net/";
		public const string URL_HELP = APP_URL + "explanation.php";
		
		// filenames 
		public const string SessionFileExtension = ".solarbeam";
		public const string SessionFileFilter = "*" + SessionFileExtension;
		public const string SessionFileDesc = "SolarBeam sessions";
		
		public const string ImageFileExtension = ".png";
		public const string ImageFileFilter = "*" + ImageFileExtension;
		public const string ImageFileDesc = "Png images";
		
		public const string BinaryFileExtension = ".bin";
		public const string AutoSessionFilename = "autosave" + BinaryFileExtension;
		public const string LocationListFilename = "locations" + BinaryFileExtension;
	}
}