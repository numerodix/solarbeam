// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections;
using System.Collections.Generic;

namespace LibSolar.Types
{
	/**
	 * Provide enumerable collection with O(1) lookup, backed by hashtable.
	 */
	public class StaticList<T>
	{
		private Hashtable hash;
	
		public StaticList(T[] es)
		{
			hash = new Hashtable();
			foreach (T e in es)
			{
				hash.Add(e, null);
			}
		}
		
		public bool Contains(T e)
		{
			return hash.ContainsKey(e);
		}
		
		public IEnumerator<T> GetEnumerator()
		{
			foreach (object e in hash.Keys)
			{
				yield return ((T) e);
			}
		}
	}
}