// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.IO;
using System.Reflection;

using ICSharpCode.SharpZipLib.GZip;

namespace LibSolar.Util
{
	/**
	 * Offers queries on assemblies.
	 */
	public class AsmInfo
	{
		private Assembly asm;

		public AsmInfo(Assembly asm)
		{
			this.asm = asm;
		}
		
		public string GetAppPath()
		{
			string mod = asm.GetModules()[0].FullyQualifiedName;
			return Path.GetDirectoryName(mod);
		}
		
		public string InAppDir(string path)
		{
			string app_path = GetAppPath();
			return Path.Combine(app_path, path);
		}
		
		public string GetSerializePath(string path)
		{
			return InAppDir(path);
		}
		
		public string GetAtt(string att_name)
		{
			string att_val = String.Empty;
			
			if (att_name == "Version") {
				Version v = asm.GetName().Version;
				att_val = String.Format("{0}.{1}", v.Major, v.Minor);
			} else {
				object[] atts = asm.GetCustomAttributes(false);
				foreach (object obj in atts) {
					Type type = obj.GetType();
					if (type.Name == "Assembly" + att_name + "Attribute") {
						PropertyInfo[] props = type.GetProperties();
						foreach (PropertyInfo prop in props) {
							if (prop.Name == att_name) {
								object val = prop.GetValue(obj, null);
								att_val = val.ToString();
								break;
							}
						}
					}
				}
			}
			
			return att_val;
		}
		
		public string GetString(string name)
		{
			Stream stream = GetResource(name);
			byte[] bts = new byte[(int) stream.Length];
			stream.Read(bts, 0, bts.Length);
			string s = System.Text.Encoding.ASCII.GetString(bts).Trim();
			return s;
		}
		
		public Bitmap GetBitmap(string name)
		{
			return new Bitmap(GetResource(name));
		}
		
		public Icon GetIcon(string name)
		{
			return new Icon(GetResource(name));
		}
		
		private Stream GetResource(string name)
		{
			Stream stream = asm.GetManifestResourceStream(name);
			if (stream == null) {
				stream = asm.GetManifestResourceStream(name + ".gz");
				stream = Unzip(stream);
			}
			return stream;
		}
		
		private static Stream Unzip(Stream stream)
		{
			GZipInputStream zipstream = new GZipInputStream(stream);
			Stream unzipstream = new MemoryStream();
			Copy(zipstream, unzipstream);
			return unzipstream;
		}
	
		private static void Copy(Stream source, Stream target)
		{
			byte[] buf = new byte[2048];
			int cur = 0;
			int len = 0;
			while (true) {
				len = source.Read(buf, cur, buf.Length);
				if (len > 0)
					target.Write(buf, cur, buf.Length);
				else
					break;
			}
			target.Seek(0, SeekOrigin.Begin);
		}
	}
}
