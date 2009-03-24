// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;

using LibSolar.Types;

namespace SolarbeamGui
{
	static class Tooltips
	{
		private static Dictionary<Controller.Id,string> titles =
			new Dictionary<Controller.Id,string>();
		private static Dictionary<Controller.Id,string> tips =
			new Dictionary<Controller.Id,string>();

		static Tooltips()
		{
			titles.Add(Controller.Id.LOCATION, "Location");
			tips.Add(Controller.Id.LOCATION,
					"Select an existing location or enter a new one");
			titles.Add(Controller.Id.LOCATIONNEW_ACTION, "New location");
			tips.Add(Controller.Id.LOCATIONNEW_ACTION, "&Add new location");
			titles.Add(Controller.Id.LOCATIONSAVE_ACTION, "Save location");
			tips.Add(Controller.Id.LOCATIONSAVE_ACTION, "&Save this location");
			titles.Add(Controller.Id.LOCATIONDELETE_ACTION, "Delete location");
			tips.Add(Controller.Id.LOCATIONDELETE_ACTION, "&Delete this location");

			string lat_title = "Latitude";
			titles.Add(Controller.Id.LATITUDE_DEGS, lat_title);
			titles.Add(Controller.Id.LATITUDE_MINS, lat_title);
			titles.Add(Controller.Id.LATITUDE_SECS, lat_title);
			titles.Add(Controller.Id.LATITUDE_DIRECTION, lat_title);
			tips.Add(Controller.Id.LATITUDE_DEGS,
					String.Format("Enter the number of degrees latitude ({0}-{1})",
						Position.LATDEGS_MINVALUE,
						Position.LATDEGS_MAXVALUE));
			tips.Add(Controller.Id.LATITUDE_MINS,
					String.Format("Enter the number of minutes latitude ({0}-{1})",
						Position.LATMINS_MINVALUE,
						Position.LATMINS_MAXVALUE));
			tips.Add(Controller.Id.LATITUDE_SECS,
					String.Format("Enter the number of seconds latitude ({0}-{1})",
						Position.LATSECS_MINVALUE,
						Position.LATSECS_MAXVALUE));
			tips.Add(Controller.Id.LATITUDE_DIRECTION, "Select the direction latitude");

			string lon_title = "Longitude";
			titles.Add(Controller.Id.LONGITUDE_DEGS, lon_title);
			titles.Add(Controller.Id.LONGITUDE_MINS, lon_title);
			titles.Add(Controller.Id.LONGITUDE_SECS, lon_title);
			titles.Add(Controller.Id.LONGITUDE_DIRECTION, lon_title);
			tips.Add(Controller.Id.LONGITUDE_DEGS,
					String.Format("Enter the number of degrees longitude ({0}-{1})",
						Position.LONDEGS_MINVALUE,
						Position.LONDEGS_MAXVALUE));
			tips.Add(Controller.Id.LONGITUDE_MINS,
					String.Format("Enter the number of minutes longitude ({0}-{1})",
						Position.LONMINS_MINVALUE,
						Position.LONMINS_MAXVALUE));
			tips.Add(Controller.Id.LONGITUDE_SECS,
					String.Format("Enter the number of seconds longitude ({0}-{1})",
						Position.LONSECS_MINVALUE,
						Position.LONSECS_MAXVALUE));
			tips.Add(Controller.Id.LONGITUDE_DIRECTION, "Select the direction longitude");

			titles.Add(Controller.Id.TIMEZONE_OFFSET, "Timezone offset");
			tips.Add(Controller.Id.TIMEZONE_OFFSET,
					"Select the UTC offset your timezone is in");
			titles.Add(Controller.Id.TIMEZONE_NAME, "Timezone name");
			tips.Add(Controller.Id.TIMEZONE_NAME, "Select your timezone by name");

			string dt_title = "Date";
			titles.Add(Controller.Id.DATE_DAY, dt_title);
			titles.Add(Controller.Id.DATE_MONTH, dt_title);
			titles.Add(Controller.Id.DATE_YEAR, dt_title);
			tips.Add(Controller.Id.DATE_DAY,
					String.Format("Enter the day of the month ({0}-{1})",
						UTCDate.DAY_MINVALUE,
						UTCDate.DAY_MAXVALUE));
			tips.Add(Controller.Id.DATE_MONTH,
					String.Format("Enter the month of the year ({0}-{1})",
						UTCDate.MONTH_MINVALUE,
						UTCDate.MONTH_MAXVALUE));
			tips.Add(Controller.Id.DATE_YEAR,
					String.Format("Enter the year ({0}-{1})",
						UTCDate.YEAR_MINVALUE,
						UTCDate.YEAR_MAXVALUE));

			string tm_title = "Time";
			titles.Add(Controller.Id.TIME_HOUR, tm_title);
			titles.Add(Controller.Id.TIME_MINUTE, tm_title);
			titles.Add(Controller.Id.TIME_SECOND, tm_title);
			tips.Add(Controller.Id.TIME_HOUR,
					String.Format("Enter the number of hours ({0}-{1})",
						UTCDate.HOUR_MINVALUE,
						UTCDate.HOUR_MAXVALUE));
			tips.Add(Controller.Id.TIME_MINUTE,
					String.Format("Enter the number of minutes ({0}-{1})",
						UTCDate.MINUTE_MINVALUE,
						UTCDate.MINUTE_MAXVALUE));
			tips.Add(Controller.Id.TIME_SECOND,
					String.Format("Enter the number of seconds ({0}-{1})",
						UTCDate.SECOND_MINVALUE,
						UTCDate.SECOND_MAXVALUE));

			titles.Add(Controller.Id.RESETFORM_ACTION, "Reset form");
			tips.Add(Controller.Id.RESETFORM_ACTION,
					"R&eset inputs to previous rendering");
			titles.Add(Controller.Id.RENDER_ACTION, "Render diagram");
			tips.Add(Controller.Id.RENDER_ACTION, "&Render a new diagram");
			
			titles.Add(Controller.Id.IMAGE_SIZE, "Save image");
			titles.Add(Controller.Id.IMAGESAVE_ACTION, "Save image");
			tips.Add(Controller.Id.IMAGE_SIZE, "Enter a size for the image");
			tips.Add(Controller.Id.IMAGESAVE_ACTION, "&Save diagram to image");
		}

		public static string GetTitle(Controller.Id id)
		{
			string val = null;
			titles.TryGetValue(id, out val);
			return val;
		}

		public static string GetTip(Controller.Id id)
		{
			string val = null;
			tips.TryGetValue(id, out val);
			return val;
		}
	}
}
