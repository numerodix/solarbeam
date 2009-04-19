// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
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
		
		public Stream Unzip(Stream stream)
		{
			GZipInputStream zipstream = new GZipInputStream(stream);
			
			byte[] bts = new byte[(int) zipstream.Length];
			int unziplen = zipstream.Read(bts, 0, bts.Length);
			for (int i=0; i<bts.Length; i++) {
				Console.Write(bts[i]);
			}
			
			Stream unzipstream = new MemoryStream();
			unzipstream.Write(bts, 0, unziplen);
			
			return unzipstream;
		}
	
		public Stream GetResource(string name)
		{
			Stream stream = asm.GetManifestResourceStream(name);
			if (name == "COPYING.short.gz") {
				stream = Unzip(stream);
			}
			return stream;
		}
	}
}
