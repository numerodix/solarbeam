// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using LibSolar.Types;

namespace SolarbeamGui
{
	/**
	 * Represents widget panel for left side of screen.
	 */
	sealed class GuiControlPanel : Panel
	{
		private const int GROUPBOX_HEIGHT = 20;
		private const int GROUPBOX_WIDTH = 10;
		
		private const int FORM_ROW_HEIGHT = 30;
		private const int FORM_PADDING = 3;
		private const int FORM_MARGIN = 0;
		private const int INPUTS_COUNT = 6;
		private const int OUTPUTS_COUNT = 6;
		private const int INPUTS_HEIGHT = GROUPBOX_HEIGHT + INPUTS_COUNT * FORM_ROW_HEIGHT + 2*FORM_PADDING;
		private const int BUTTONS_HEIGHT = 30;
		private const int OUTPUTS_HEIGHT = GROUPBOX_HEIGHT + OUTPUTS_COUNT * FORM_ROW_HEIGHT + 2*FORM_PADDING;
		private const int PANEL_COUNT = 3;
		private const int PANEL_WIDTH = 300;
		
		public const int WIDTH = GROUPBOX_WIDTH + PANEL_WIDTH + 2*FORM_PADDING;
		public const int HEIGHT = INPUTS_HEIGHT + BUTTONS_HEIGHT + OUTPUTS_HEIGHT + 2*FORM_PADDING;
	
	
		public GuiControlPanel()
		{
			InitializeComponent();
		}
	
		private void InitializeComponent()
		{
			GroupBox inputs = new GroupBox();
			inputs.Text = "Parameters";
			inputs.Size = new Size(PANEL_WIDTH, INPUTS_HEIGHT);
			inputs.Controls.Add(GetInputs());
			
			Control buttons = GetButtons();
			buttons.Size = new Size(PANEL_WIDTH, BUTTONS_HEIGHT);
	
			GroupBox outputs = new GroupBox();
			outputs.Text = "Outputs";
			outputs.Size = new Size(PANEL_WIDTH, OUTPUTS_HEIGHT);
			outputs.Controls.Add(GetOutputs());
	
	
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(PANEL_COUNT, 1, 
			                                              FORM_MARGIN, FORM_PADDING);
			layout.Width = WIDTH;
			layout.Height = HEIGHT;
	
			layout.Controls.Add(inputs, 0, 0);
			layout.Controls.Add(buttons, 0, 1);
			layout.Controls.Add(outputs, 0, 2);
	
			this.Dock = DockStyle.Fill;
			this.Controls.Add(layout);
		}
	
		private Control GetOutputs()
		{
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(OUTPUTS_COUNT, 1,
			                                              FORM_MARGIN, FORM_PADDING);
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
	
			for (int i = 0; i < OUTPUTS_COUNT; i++) {
				layout.RowStyles.Add(new RowStyle(SizeType.Absolute, FORM_ROW_HEIGHT));
			}
	
			Control el = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabel("Sun elevation"),
					Widgets.GetLabel(":"),
					Widgets.GetTextBox(Controller.Id.ELEVATION, "-13.1231")},
					new float[] {19F, 2F, 23F});
	
			Control az = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabel("Sun azimuth"),
					Widgets.GetLabel(":"),
					Widgets.GetTextBox(Controller.Id.AZIMUTH, "212.6669")},
					new float[] {19F, 2F, 23F});
	
			Control rise = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabel("Sunrise"),
					Widgets.GetLabel(":"),
					Widgets.GetTextBox(Controller.Id.SUNRISE, "06:09")},
					new float[] {19F, 2F, 23F});
	
			Control noon = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabel("Solar noon"),
					Widgets.GetLabel(":"),
					Widgets.GetTextBox(Controller.Id.SOLAR_NOON, "12:12")},
					new float[] {19F, 2F, 23F});    
	
			Control sset = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabel("Sunset"),
					Widgets.GetLabel(":"),
					Widgets.GetTextBox(Controller.Id.SUNSET, "18:15")},
					new float[] {19F, 2F, 23F});
	
			Control dlen = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabel("Day length"),
					Widgets.GetLabel(":"),
					Widgets.GetTextBox(Controller.Id.DAY_LENGTH, "12h 0m")},
					new float[] {19F, 2F, 23F}); 
	
			layout.Controls.Add(el, 0, 0);
			layout.Controls.Add(az, 0, 1);
			layout.Controls.Add(rise, 0, 2);
			layout.Controls.Add(noon, 0, 3);
			layout.Controls.Add(sset, 0, 4);
			layout.Controls.Add(dlen, 0, 5);
	
			return layout;
		}
	
		private Control GetButtons()
		{
			Control btns = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabel(String.Empty), //layout buffer
					Widgets.GetButton(Controller.Id.RESETFORM_ACTION, "Reset"),
					Widgets.GetLabel(String.Empty),
					Widgets.GetButton(Controller.Id.RENDER_ACTION, "Render"),
					Widgets.GetLabel(String.Empty)},
				new float[] {15F, 30F, 15F, 30F, 15F});
	
			return btns;
		}
	
		private Control GetInputs()
		{
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(INPUTS_COUNT, 3, 
			                                              FORM_MARGIN, FORM_PADDING);
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
	
			for (int i = 0; i < INPUTS_COUNT; i++) {
				layout.RowStyles.Add(new RowStyle(SizeType.Absolute, FORM_ROW_HEIGHT));
			}
			
			Label loc_lbl = Widgets.GetLabel("Location:");
			Control loc_ins = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetComboBoxInputable(
						Controller.Id.LOCATION,
						Tooltips.LocTip,
						Controller.LocationsSource.Locations)},
				new float[] {100F});
	
			Label lat_lbl = Widgets.GetLabel("Latitude:");
			ComboBox lat_dir = Widgets.GetComboBox(
				Controller.Id.LATITUDE_DIRECTION,
			    null,
				Controller.PositionSource.LatitudeDirections);
			Control lat_ins = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetNumericUpDown(Controller.Id.LATITUDE_DEGS,
						Tooltips.LatTipDegree,
						Position.LATDEGS_MINVALUE-1,
						Position.LATDEGS_MAXVALUE),
					Widgets.GetNumericUpDown(Controller.Id.LATITUDE_MINS,
						Tooltips.LatTipMinute,
						Position.LATMINS_MINVALUE-1,
						Position.LATMINS_MAXVALUE+1),
					Widgets.GetNumericUpDown(Controller.Id.LATITUDE_SECS,
						Tooltips.LatTipSecond,
						Position.LATSECS_MINVALUE-1,
						Position.LATSECS_MAXVALUE+1),
				lat_dir},
				new float[] {27F, 23F, 23F, 30F});
			
			Label lon_lbl = Widgets.GetLabel("Longitude:");
			ComboBox lon_dir = Widgets.GetComboBox(
				Controller.Id.LONGITUDE_DIRECTION,
				null,
				Controller.PositionSource.LongitudeDirections);
			Control lon_ins = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetNumericUpDown(Controller.Id.LONGITUDE_DEGS,
						Tooltips.LonTipDegree,
						Position.LONDEGS_MINVALUE-1,
						Position.LONDEGS_MAXVALUE),
					Widgets.GetNumericUpDown(Controller.Id.LONGITUDE_MINS,
						Tooltips.LonTipMinute,
						Position.LONMINS_MINVALUE-1,
						Position.LONMINS_MAXVALUE+1),
					Widgets.GetNumericUpDown(Controller.Id.LONGITUDE_SECS,
				        Tooltips.LonTipSecond,
						Position.LONSECS_MINVALUE-1,
						Position.LONSECS_MAXVALUE+1),
				lon_dir},
				new float[] {27F, 23F, 23F, 30F});
			
			Label tz_lbl = Widgets.GetLabel("Timezone:");
			Control tz_in = Widgets.GetLaidOut(
				new Control[] {
				Widgets.GetComboBox(Controller.Id.TIMEZONE_OFFSET,
					Tooltips.TzTipOffset,
					Controller.TimezoneSource.Offsets),
				Widgets.GetComboBox(Controller.Id.TIMEZONE_NAME,
					Tooltips.TzTipZone,
					Controller.TimezoneSource.GetTimezones(
						Controller.TimezoneSource.Offsets[0]))
					},
					new float[] {24F, 50F});

			Label date_lbl = Widgets.GetLabel("Date:");
			Control date_scr = Widgets.GetLabel(String.Empty);
			Control date_ins = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetNumericUpDown(Controller.Id.DATE_DAY,
						Tooltips.DateTipDay,
						UTCDate.DAY_MINVALUE-1,
						UTCDate.DAY_MAXVALUE+1),
					Widgets.GetNumericUpDown(Controller.Id.DATE_MONTH,
						Tooltips.DateTipMonth,
						UTCDate.MONTH_MINVALUE-1,
						UTCDate.MONTH_MAXVALUE+1),
					Widgets.GetNumericUpDown(Controller.Id.DATE_YEAR,
						Tooltips.DateTipTitle,
						UTCDate.YEAR_MINVALUE,
						UTCDate.YEAR_MAXVALUE),
				date_scr},
				new float[] {20F, 20F, 25F, 30F});
			
			Label time_lbl = Widgets.GetLabel("Time:");
			Control time_scr = Widgets.GetLabel(String.Empty);
			Control time_ins = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetNumericUpDown(Controller.Id.TIME_HOUR,
						Tooltips.TimeTipHour,
						UTCDate.HOUR_MINVALUE-1,
						UTCDate.HOUR_MAXVALUE+1),
					Widgets.GetNumericUpDown(Controller.Id.TIME_MINUTE,
						Tooltips.TimeTipMinute,
						UTCDate.MINUTE_MINVALUE-1,
						UTCDate.MINUTE_MAXVALUE+1),
					Widgets.GetNumericUpDown(Controller.Id.TIME_SECOND,
						Tooltips.TimeTipSecond,
						UTCDate.SECOND_MINVALUE-1,
						UTCDate.SECOND_MAXVALUE+1),
				time_scr},
				new float[] {20F, 20F, 20F, 30F});

			layout.Controls.Add(loc_lbl, 0, 0);
			layout.Controls.Add(loc_ins, 1, 0);
			layout.Controls.Add(lat_lbl, 0, 1);
			layout.Controls.Add(lat_ins, 1, 1);
			layout.Controls.Add(lon_lbl, 0, 2);
			layout.Controls.Add(lon_ins, 1, 2);
			layout.Controls.Add(tz_lbl, 0, 3);
			layout.Controls.Add(tz_in, 1, 3);
			layout.Controls.Add(date_lbl, 0, 4);
			layout.Controls.Add(date_ins, 1, 4);
			layout.Controls.Add(time_lbl, 0, 5);
			layout.Controls.Add(time_ins, 1, 5);
		
			return layout;
		}
	}
}
