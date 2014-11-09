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
							name = "Windows 7";
							break;
						case 2:
							name = "Windows 8";
							break;
						case 3:
							name = "Windows 8.1";
							break;
						case 4:
							name = "Windows 10";
							break;
					}
					break;
				}
				break;
			}
			return name;
		}

		/**
		 * 1. Use uname to establish Unix platform.
		 * 2. Try lsb_release to find distro info.
		 * 3. Try /etc/lsb-release as last resort.
		 */
		public static string GetUnixPlatformName()
		{
			string name = "Unix";
			try {
				name = Processes.Run("uname", string.Empty);
				if (name == "Linux") {
					string distro = string.Empty;
					string release = string.Empty;

					try {
						string distro_t = Processes.Run("lsb_release", "-i");
						distro_t = distro_t.Split(new char[] {':'})[1].Trim();
						if ((distro_t == null) || (distro_t == string.Empty))
							throw new Exception();
						distro = distro_t;

						string release_t = Processes.Run("lsb_release", "-r");
						release_t = release_t.Split(new char[] {':'})[1].Trim();
						if ((release_t != null) && (release_t != string.Empty))
							release = " " + release_t;
					} catch {
						string[] lines = File.ReadAllLines("/etc/lsb-release");
						foreach (string line in lines) {
							if (line.StartsWith("DISTRIB_ID")) {
								string distro_t = line.Split(new char[] {'='})[1].Trim().Replace("\"", "");
								if ((distro_t == null) || (distro_t == string.Empty))
									throw new Exception();
								distro = distro_t;
							}
							if (line.StartsWith("DISTRIB_RELEASE")) {
								string release_t = line.Split(new char[] {'='})[1].Trim().Replace("\"", "");
								if ((release_t != null) && (release_t != string.Empty))
									release = " " + release_t;
							}
						}
						if ((distro == null) || (distro == string.Empty))
							throw new Exception();
					}

					name = string.Format("{0}{1}", distro, release);
				}
			} catch {}
			return name;
		}
	}
}
