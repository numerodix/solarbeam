// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using LibSolar.Types;

namespace LibSolar.Locations
{
	/**
	 * Describe a geographical location.
	 */
	public struct Location
	{
		private string name;
		private Position position;
		private string timezone;
		
		public Location(string name, Position position, string timezone)
		{
			this.name = name;
			this.position = position;
			this.timezone = timezone;
		}
		
		public string Name
		{ get { return name; } }
		
		public Position Position
		{ get { return position; } }
		
		public string Timezone
		{ get { return timezone; } }
	}
}
