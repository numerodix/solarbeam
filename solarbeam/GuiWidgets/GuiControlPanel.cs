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
		private const int FORM_MARGIN = 16;
		
		private const int INPUTS_COUNT = 6;
		private const int OUTPUTS_COUNT = 5;
		private const int IMAGESAVE_COUNT = 2;
		
		private const int INPUTS_HEIGHT = GROUPBOX_HEIGHT + INPUTS_COUNT * FORM_ROW_HEIGHT + 2*FORM_PADDING;
		private const int BUTTONS_HEIGHT = 30;
		private const int OUTPUTS_HEIGHT = GROUPBOX_HEIGHT + OUTPUTS_COUNT * FORM_ROW_HEIGHT + 2*FORM_PADDING;
		private const int IMAGESAVE_HEIGHT = GROUPBOX_HEIGHT + IMAGESAVE_COUNT * FORM_ROW_HEIGHT + 2*FORM_PADDING;
		
		private const int PANEL_COUNT = 4;
		private const int PANEL_WIDTH = 360;
		
		public const int WIDTH = GROUPBOX_WIDTH + PANEL_WIDTH + 2*FORM_PADDING;
		public const int HEIGHT = INPUTS_HEIGHT + BUTTONS_HEIGHT + OUTPUTS_HEIGHT + IMAGESAVE_HEIGHT + 2*FORM_PADDING + 2*FORM_MARGIN;
	
		private static readonly string KEY_WIDTH = (120).ToString();
			
	
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
	
			GroupBox imagesave = new GroupBox();
			imagesave.Text = "Save diagram to image";
			imagesave.Size = new Size(PANEL_WIDTH, IMAGESAVE_HEIGHT);
			imagesave.Controls.Add(GetImageSave());
			
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(PANEL_COUNT, 1, 
			                                                      FORM_MARGIN, FORM_PADDING);
			layout.Width = WIDTH;
			layout.Height = HEIGHT;

			layout.Controls.Add(inputs, 0, 0);
			layout.Controls.Add(buttons, 0, 1);
			layout.Controls.Add(outputs, 0, 2);
			layout.Controls.Add(imagesave, 0, 3);
	
			this.Dock = DockStyle.Fill;
			this.Controls.Add(layout);
		}
	
		private Control GetInputs()
		{
			string key_w = (70).ToString();
			
			Control loc_ins = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabelAnon("Location:"),
					Widgets.GetComboBoxInputable(
						Controller.Id.LOCATION,
						Controller.LocationsSource.Locations),
					Widgets.GetButtonImage(Controller.Id.LOCATIONNEW_ACTION,
						"new.png"),
					Widgets.GetButtonImage(Controller.Id.LOCATIONSAVE_ACTION,
						"save.png"),
					Widgets.GetButtonImage(Controller.Id.LOCATIONDELETE_ACTION,
						"delete.png"),
					},
				new string[] {key_w, "80%", "11%", "11%", "11%"});
	
			ComboBox lat_dir = Widgets.GetComboBox(
				Controller.Id.LATITUDE_DIRECTION,
				Controller.PositionSource.LatitudeDirections);
			Control lat_ins = Widgets.GetLaidOut(
				new Control[] {
				Widgets.GetLabelAnon("Latitude:"),
					Widgets.GetNumericUpDown(Controller.Id.LATITUDE_DEGS,
						Position.LATDEGS_MIN-1,
						Position.LATDEGS_MAX),
					Widgets.GetLabelAnon("°"),
					Widgets.GetNumericUpDown(Controller.Id.LATITUDE_MINS,
						Position.LATMINS_MIN-1,
						Position.LATMINS_MAX+1),
					Widgets.GetLabelAnon("'"),
					Widgets.GetNumericUpDown(Controller.Id.LATITUDE_SECS,
						Position.LATSECS_MIN-1,
						Position.LATSECS_MAX+1),
					Widgets.GetLabelAnon("\""),
					lat_dir},
				new string[] {key_w, "27%", "6%", "23%", "6%", "23%", "6%", "30%"});
			
			ComboBox lon_dir = Widgets.GetComboBox(
				Controller.Id.LONGITUDE_DIRECTION,
				Controller.PositionSource.LongitudeDirections);
			Control lon_ins = Widgets.GetLaidOut(
				new Control[] {
				Widgets.GetLabelAnon("Longitude:"),
					Widgets.GetNumericUpDown(Controller.Id.LONGITUDE_DEGS,
						Position.LONDEGS_MIN-1,
						Position.LONDEGS_MAX),
					Widgets.GetLabelAnon("°"),
					Widgets.GetNumericUpDown(Controller.Id.LONGITUDE_MINS,
						Position.LONMINS_MIN-1,
						Position.LONMINS_MAX+1),
					Widgets.GetLabelAnon("'"),
					Widgets.GetNumericUpDown(Controller.Id.LONGITUDE_SECS,
						Position.LONSECS_MIN-1,
						Position.LONSECS_MAX+1),
					Widgets.GetLabelAnon("\""),
					lon_dir},
				new string[] {key_w, "27%", "6%", "23%", "6%", "23%", "6%", "30%"});
			
			Control tz_in = Widgets.GetLaidOut(
				new Control[] {
					 Widgets.GetLabelAnon("Timezone:"),
					 Widgets.GetComboBox(Controller.Id.TIMEZONE_OFFSET,
						 Controller.TimezoneSource.Offsets),
					 Widgets.GetComboBox(Controller.Id.TIMEZONE_NAME,
						 Controller.TimezoneSource.GetTimezones(
							 Controller.TimezoneSource.Offsets[0]))
					},
				new string[] {key_w, "25%", "75%"});

			Control date_ins = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabelAnon("Date:"),
					Widgets.GetNumericUpDown(Controller.Id.DATE_DAY,
						UTCDate.DAY_MIN-1,
						UTCDate.DAY_MAX+1),
					Widgets.GetLabelAnon("."),
					Widgets.GetNumericUpDown(Controller.Id.DATE_MONTH,
						UTCDate.MONTH_MIN-1,
						UTCDate.MONTH_MAX+1),
					Widgets.GetLabelAnon("."),
					Widgets.GetNumericUpDown(Controller.Id.DATE_YEAR,
						UTCDate.YEAR_MIN,
						UTCDate.YEAR_MAX),
					Widgets.GetLabelImage(Controller.Id.DATE_DSTSTATUS,
					    "dst-status-nodst.png")},
				new string[] {key_w, "22%", "5%", "22%", "5%", "27%", "30%"});
			
			Control time_ins = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabelAnon("Time:"),
					Widgets.GetNumericUpDown(Controller.Id.TIME_HOUR,
						UTCDate.HOUR_MIN-1,
						UTCDate.HOUR_MAX+1),
					Widgets.GetLabelAnon(":"),
					Widgets.GetNumericUpDown(Controller.Id.TIME_MINUTE,
						UTCDate.MINUTE_MIN-1,
						UTCDate.MINUTE_MAX+1),
					Widgets.GetLabelAnon(":"),
					Widgets.GetNumericUpDown(Controller.Id.TIME_SECOND,
						UTCDate.SECOND_MIN-1,
						UTCDate.SECOND_MAX+1),
					Widgets.GetButtonImageTextAnon(Controller.Id.TIMENOW_ACTION,
					                               "Now", "time-now.png")},
				new string[] {key_w, "23%", "5%", "23%", "5%", "23%", "30%"});
	
			Control layout = Widgets.GetStacked(
				new Control[] {loc_ins, lat_ins, lon_ins, tz_in, date_ins, time_ins},
				FORM_ROW_HEIGHT.ToString());
		
			return layout;
		}

		private Control GetButtons()
		{
			Control btns = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabelAnon(String.Empty), //layout buffer
					Widgets.GetButtonImageTextAnon(Controller.Id.RESETFORM_ACTION,
					                               "Reset", "reset.png"),
					Widgets.GetLabelAnon(String.Empty),
					Widgets.GetButtonImageText(Controller.Id.RENDER_ACTION,
					                           "Render", "render.png"),
					Widgets.GetLabelAnon(String.Empty)},
				new string[] {"15%", Widgets.BUTTON_WIDTH, "15%", Widgets.BUTTON_WIDTH, "15%"});
	
			return btns;
		}
	
		private Control GetOutputs()
		{
			string[] fmt = new string[] {KEY_WIDTH, Widgets.COLON_WIDTH, "100%"};
				
			Control el = GetOutputRow(fmt, "Solar elevation", Controller.Id.ELEVATION);
			Control az = GetOutputRow(fmt, "Solar azimuth", Controller.Id.AZIMUTH);
			Control noon = GetOutputRow(fmt, "Solar noon", Controller.Id.SOLAR_NOON);
			Control rise_set = GetOutputRow(fmt, "Sunrise/Sunset", Controller.Id.SUNRISESUNSET);
			Control dawn_dusk = GetOutputRow(fmt, "Dawn/Dusk", Controller.Id.DAWNDUSK);
			
			Control layout = Widgets.GetStacked(
				new Control[] {el, az, noon, rise_set, dawn_dusk},
				FORM_ROW_HEIGHT.ToString());
				
			return layout;
		}

		private Control GetImageSave()
		{
			Control btns = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabelAnon(String.Empty), //layout buffer
					Widgets.GetNumericUpDown(
						Controller.Id.IMAGE_SIZE,
						300,
						10000),
					Widgets.GetLabelAnon("pixels"),
					Widgets.GetButtonImageText(Controller.Id.IMAGESAVE_ACTION,
				                           "Save", "image-save.png"),
					Widgets.GetLabelAnon(String.Empty)},
				new string[] {"20%", "20%", "15%", Widgets.BUTTON_WIDTH, "20%"});

			Control checkboxes = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetCheckBox(
						Controller.Id.IMAGE_SUNTOGGLE,
						"Current day trajectory",
						true),
					Widgets.GetCheckBox(
						Controller.Id.IMAGE_CAPTIONTOGGLE,
						"Caption below diagram",
						true)},
				new string[] {"50%", "50%"});
							
			Control layout = Widgets.GetStacked(
				new Control[] {btns, checkboxes},
				FORM_ROW_HEIGHT.ToString());
			
			return layout;
		}
					
		private Control GetOutputRow(string[] col_fmt, string label, Controller.Id id)
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
