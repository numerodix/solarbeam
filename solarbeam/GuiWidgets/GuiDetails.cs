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
		
		private const string key_w = "100";
		private const string colon_w = "10";
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
			Control loc = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Location"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_LOCATION, string.Empty)},
					col_fmt);
			
			Control pos = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Coordinates"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_POSITION, string.Empty)},
					col_fmt);
			
			Control tz = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Timezone"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_TIMEZONE, string.Empty)},
					col_fmt);
			
			Control date = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Date"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_DATE, string.Empty)},
					col_fmt);			
			
			Control time = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Time"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_TIME, string.Empty)},
					col_fmt);
			
			Control layout = Widgets.GetStacked(
				new Control[] {
					loc,
					pos,
					tz,
					date,
					time},
				FORM_ROW_HEIGHT.ToString());

			return Widgets.GetGroupBox("Parameters", layout);
		}
		
		private Control GetSolarPosition()
		{
			Control el = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Solar elevation"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_ELEVATION, string.Empty)},
					col_fmt);
			
			Control az = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Solar azimuth"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_AZIMUTH, string.Empty)},
					col_fmt);	
			
			Control layout = Widgets.GetStacked(
				new Control[] {
					el,
					az},
				FORM_ROW_HEIGHT.ToString());
			
			return Widgets.GetGroupBox("Solar position", layout);
		}
		
		private Control GetSolarTimes()
		{
			Control sunrise = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Sunrise"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_SUNRISE, string.Empty)},
					col_fmt);
			
			Control solarnoon = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Solar noon"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_SOLARNOON, string.Empty)},
					col_fmt);	
			
			Control sunset = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Sunset"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_SUNSET, string.Empty)},
					col_fmt);	
			
			Control solardaylength = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Solar day length"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_SOLARDAYLENGTH, string.Empty)},
					col_fmt);	
			
			Control layout = Widgets.GetStacked(
				new Control[] {
					sunrise,
					solarnoon,
					sunset,
					solardaylength},
				FORM_ROW_HEIGHT.ToString());
			
			return Widgets.GetGroupBox("Solar times", layout);
		}
		
		private Control GetDawnDusk()
		{
			Control dawn = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Dawn"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_DAWN, string.Empty)},
					col_fmt);
			
			Control dusk = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Dusk"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_DUSK, string.Empty)},
					col_fmt);	
			
			Control daylength = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Day length"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_DAYLENGTH, string.Empty)},
					col_fmt);	
			
			Control layout = Widgets.GetStacked(
				new Control[] {
					dawn,
					dusk,
					daylength},
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
	}
}
