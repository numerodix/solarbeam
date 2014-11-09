// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

namespace LibSolar.Util
{
	public static class Memory
	{
		public static void Collect(IDisposable obj)
		{
			if (obj != null) {
				obj.Dispose();
			}
		}
	}
}
