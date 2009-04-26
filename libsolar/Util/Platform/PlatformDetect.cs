// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.IO;

namespace LibSolar.Util
{
	static class PlatformDetect
	{
		public static string GetPlatformName()
		{
			if (Platform.GetPlatform() == PlatformName.Windows) {
				return GetWindowsPlatformName();
			} else {
				return GetUnixPlatformName();
			}
		}
		
		public static string GetUnixPlatformName()
		{
			string name = "Unix";
			try {
				name = Processes.Run("uname", string.Empty);
				if (name == "Linux") {
					string distro = Processes.Run("lsb_release", "-i");
					name = distro = distro.Split(new char[] {':'})[1].Trim();
					
					string release = Processes.Run("lsb_release", "-r");
					release = release.Split(new char[] {':'})[1].Trim();
					
					name = string.Format("{0} {1}", distro, release);
				}
			} catch {}
			return name;
		}
		
		/**
		 * Reference: http://support.microsoft.com/kb/304283
		 * Reference: http://www.codeproject.com/KB/system/osversion.aspx
		 * Reference: http://www.codeguru.com/cpp/w-p/system/systeminformation/article.php/c8973/
		 */
		public static string GetWindowsPlatformName()
		{
			OperatingSystem os = Environment.OSVersion;
			
			string name = "Windows";
			switch (os.Platform) {
			case PlatformID.Win32Windows:
				switch (os.Version.Minor) {
				case 0:
					name = "Windows 95";
					break;
				case 10:
					if(os.Version.Revision.ToString() == "2222A")
						name = "Windows 98 Second Edition";
					else
						name = "Windows 98";
					break;
				case  90:
					name = "Windows Me";
					break;
				}
				break;
			case PlatformID.Win32NT:
				switch (os.Version.Major) {
				case 3:
					name = "Windows NT 3.51";
					break;
				case 4:
					name = "Windows NT 4.0";
					break;
				case 5:
					switch (os.Version.Minor) {
						case 0:
							name = "Windows 2000";
							break;
						case 1:
							name = "Windows XP";
							break;
						case 2:
							name = "Windows 2003";
							break;
					}
					break;
				case 6:
					switch (os.Version.Minor) {
						case 0:
							name = "Windows Vista";
							break;
						case 1:
							name = "Windows 2008";
							break;
					}
					break;
				}
				break;
			}
			return name;
		}
	}
}
