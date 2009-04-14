// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LibSolar.Util
{
	public class Serializer
	{
		public static void Serialize(string path, object obj)
		{
			using (Stream stream = File.OpenWrite(path)) {
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, obj);
			}
		}
		
		public static object Deserialize(string path)
		{
			object obj = null;
			using (Stream stream = File.OpenRead(path)) {
				BinaryFormatter formatter = new BinaryFormatter();
				obj = formatter.Deserialize(stream);
			}
			return obj;
		}
	}
}