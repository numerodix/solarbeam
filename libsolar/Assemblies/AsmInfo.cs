// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Reflection;

namespace LibSolar.Assemblies
{
	/**
	 * Offers queries on assemblies.
	 */
	public class AsmInfo
	{
		private object[] atts;

		public AsmInfo(Assembly asm)
		{
			atts = asm.GetCustomAttributes(false);
		}
		
		public string GetAtt(string att_name)
		{
			string att_val = String.Empty;
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
			return att_val;
		}
	}
}
