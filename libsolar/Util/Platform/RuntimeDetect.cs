// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.IO;
using System.Reflection;

namespace LibSolar.Util
{
	static class RuntimeDetect
	{
		public static string GetRuntimeName()
		{
			string name = null;
			
			Type mono_runtime = Type.GetType("Mono.Runtime", false);
			if (mono_runtime != null) {
				MethodInfo display_method = mono_runtime.GetMethod("GetDisplayName", 
							BindingFlags.Static
							| BindingFlags.NonPublic 
							| BindingFlags.DeclaredOnly 
							| BindingFlags.ExactBinding);
				if (display_method != null) {
					name = (string) display_method.Invoke(null, new object[0]);
				} else {
					name = string.Format("Mono {0}", Environment.Version);
				}
			} else {
				name = string.Format(".NET {0}", Environment.Version);
			}
			
			return name;
		}
	}
}
