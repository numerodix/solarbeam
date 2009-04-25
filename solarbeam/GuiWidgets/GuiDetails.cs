// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using LibSolar.Types;
using LibSolar.Util;

namespace SolarbeamGui
{
	/**
	 * Represents details pane.
	 */
	sealed class GuiDetails : Panel
	{
		private const int GROUPBOX_HEIGHT = 22;
		private const int GROUPBOX_WIDTH = 10;
		
		private const int FORM_ROW_HEIGHT = 27;
		private const int FORM_PADDING = 0;
		private const int FORM_MARGIN = 16;
		
		private const int PARAMS_COUNT = 5;
		private const int SOLARPOS_COUNT = 2;
		private const int SOLARTIMES_COUNT = 4;
		private const int DAWNDUSK_COUNT = 3;
		
		private const int PARAMS_HEIGHT = GROUPBOX_HEIGHT + PARAMS_COUNT * FORM_ROW_HEIGHT + 2*FORM_PADDING;
		private const int SOLARPOS_HEIGHT = GROUPBOX_HEIGHT + SOLARPOS_COUNT * FORM_ROW_HEIGHT + 2*FORM_PADDING;
		private const int SOLARTIMES_HEIGHT = GROUPBOX_HEIGHT + SOLARTIMES_COUNT * FORM_ROW_HEIGHT + 2*FORM_PADDING;
		private const int DAWNDUSK_HEIGHT = GROUPBOX_HEIGHT + DAWNDUSK_COUNT * FORM_ROW_HEIGHT + 2*FORM_PADDING;
		private const int BUTTONS_HEIGHT = 30;
		
		private static readonly string key_w = (100).ToString();
		private static readonly string colon_w = (10).ToString();
		private const string value_w = "100%";
		private static readonly string[] col_fmt = new string[] {
			key_w, colon_w, value_w};
		
		
		public GuiDetails()
		{
			InitializeComponent();
		}
		
		private void InitializeComponent()
		{
			this.AutoScroll = true;
			this.AutoScrollMinSize = new Size(1,1);
			this.Dock = DockStyle.Fill;
						
			Control panel = GetPanel();
			
			this.Controls.Add(panel);
		}
		
		private Control GetPanel()
		{
			Control container = Widgets.GetStacked(
				new Control[] {
					GetParameters(),
					GetSolarPosition(),
					GetSolarTimes(),
					GetDawnDusk(),
					GetButtons(),
					new Label()},
				new string[] {
					PARAMS_HEIGHT.ToString(),
					SOLARPOS_HEIGHT.ToString(),
					SOLARTIMES_HEIGHT.ToString(),
					DAWNDUSK_HEIGHT.ToString(),
					BUTTONS_HEIGHT.ToString(),
					"100%"});
			
			return container;
		}
	
		private Control GetParameters()
		{
			Control loc = GetRow(col_fmt, "Location", Controller.Id.DETAIL_LOCATION);
			Control pos = GetRow(col_fmt, "Coordinates", Controller.Id.DETAIL_POSITION);
			Control tz = GetRow(col_fmt, "Timezone", Controller.Id.DETAIL_TIMEZONE);
			Control date = GetRow(col_fmt, "Date", Controller.Id.DETAIL_DATE);
			Control time = GetRow(col_fmt, "Time", Controller.Id.DETAIL_TIME);
			
			Control layout = Widgets.GetStacked(
				new Control[] {loc, pos, tz, date, time},
				FORM_ROW_HEIGHT.ToString());

			return Widgets.GetGroupBox("Parameters", layout);
		}
		
		private Control GetSolarPosition()
		{
			Control el = GetRow(col_fmt, "Solar elevation", Controller.Id.DETAIL_ELEVATION);
			Control az = GetRow(col_fmt, "Solar azimuth", Controller.Id.DETAIL_AZIMUTH);
			
			Control layout = Widgets.GetStacked(
				new Control[] {el, az},
				FORM_ROW_HEIGHT.ToString());
			
			return Widgets.GetGroupBox("Solar position", layout);
		}
		
		private Control GetSolarTimes()
		{
			Control sunrise = GetRow(col_fmt, "Sunrise", Controller.Id.DETAIL_SUNRISE);
			Control solarnoon = GetRow(col_fmt, "Solar noon", Controller.Id.DETAIL_SOLARNOON);
			Control sunset = GetRow(col_fmt, "Sunset", Controller.Id.DETAIL_SUNSET);
			Control solardaylength = GetRow(col_fmt, "Solar day length", Controller.Id.DETAIL_SOLARDAYLENGTH);
			
			Control layout = Widgets.GetStacked(
				new Control[] {sunrise, solarnoon, sunset, solardaylength},
				FORM_ROW_HEIGHT.ToString());
			
			return Widgets.GetGroupBox("Solar times", layout);
		}
		
		private Control GetDawnDusk()
		{
			Control dawn = GetRow(col_fmt, "Dawn", Controller.Id.DETAIL_DAWN);
			Control dusk = GetRow(col_fmt, "Dusk", Controller.Id.DETAIL_DUSK);
			Control daylength = GetRow(col_fmt, "Day length", Controller.Id.DETAIL_DAYLENGTH);

			Control layout = Widgets.GetStacked(
				new Control[] {dawn, dusk, daylength},
				FORM_ROW_HEIGHT.ToString());
			
			return Widgets.GetGroupBox("Civil twilight (elevation -6°)", layout);
		}
		
		private Control GetButtons()
		{
			Control btns = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetButtonImageText(Controller.Id.DETAILSAVE_ACTION,
					                           "Save to file", "save.png"),
					Widgets.GetLabelAnon(String.Empty)},
				new string[] {"120", "100%"});
	
			return btns;
		}
		
		private Control GetRow(string[] col_fmt, string label, Controller.Id id)
		{
			return Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabelAnon(label),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(id, string.Empty)},
				col_fmt);
		}
	}
}
