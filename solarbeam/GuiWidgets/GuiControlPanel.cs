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
			imagesave.Text = "Save image";
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
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(INPUTS_COUNT, 3, 
			                                              FORM_MARGIN, FORM_PADDING);
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19F));
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 81F));
	
			for (int i = 0; i < INPUTS_COUNT; i++) {
				layout.RowStyles.Add(new RowStyle(SizeType.Absolute, FORM_ROW_HEIGHT));
			}
			
			Label loc_lbl = Widgets.GetLabel("Location:");
			Control loc_ins = Widgets.GetLaidOut(
				new Control[] {
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
				new float[] {80F, 11F, 11F, 11F});
	
			Label lat_lbl = Widgets.GetLabel("Latitude:");
			ComboBox lat_dir = Widgets.GetComboBox(
				Controller.Id.LATITUDE_DIRECTION,
				Controller.PositionSource.LatitudeDirections);
			Control lat_ins = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetNumericUpDown(Controller.Id.LATITUDE_DEGS,
						Position.LATDEGS_MIN-1,
						Position.LATDEGS_MAX),
					Widgets.GetLabel("°"),
					Widgets.GetNumericUpDown(Controller.Id.LATITUDE_MINS,
						Position.LATMINS_MIN-1,
						Position.LATMINS_MAX+1),
					Widgets.GetLabel("'"),
					Widgets.GetNumericUpDown(Controller.Id.LATITUDE_SECS,
						Position.LATSECS_MIN-1,
						Position.LATSECS_MAX+1),
					Widgets.GetLabel("\""),
					lat_dir},
				new float[] {27F, 6F, 23F, 6F, 23F, 6F, 30F});
			
			Label lon_lbl = Widgets.GetLabel("Longitude:");
			ComboBox lon_dir = Widgets.GetComboBox(
				Controller.Id.LONGITUDE_DIRECTION,
				Controller.PositionSource.LongitudeDirections);
			Control lon_ins = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetNumericUpDown(Controller.Id.LONGITUDE_DEGS,
						Position.LONDEGS_MIN-1,
						Position.LONDEGS_MAX),
					Widgets.GetLabel("°"),
					Widgets.GetNumericUpDown(Controller.Id.LONGITUDE_MINS,
						Position.LONMINS_MIN-1,
						Position.LONMINS_MAX+1),
					Widgets.GetLabel("'"),
					Widgets.GetNumericUpDown(Controller.Id.LONGITUDE_SECS,
						Position.LONSECS_MIN-1,
						Position.LONSECS_MAX+1),
					Widgets.GetLabel("\""),
					lon_dir},
				new float[] {27F, 6F, 23F, 6F, 23F, 6F, 30F});
			
			Label tz_lbl = Widgets.GetLabel("Timezone:");
			Control tz_in = Widgets.GetLaidOut(
				new Control[] {
				Widgets.GetComboBox(Controller.Id.TIMEZONE_OFFSET,
					Controller.TimezoneSource.Offsets),
				Widgets.GetComboBox(Controller.Id.TIMEZONE_NAME,
					Controller.TimezoneSource.GetTimezones(
						Controller.TimezoneSource.Offsets[0]))
					},
					new float[] {25F, 75F});

			Label date_lbl = Widgets.GetLabel("Date:");
			Control date_ins = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetNumericUpDown(Controller.Id.DATE_DAY,
						UTCDate.DAY_MIN-1,
						UTCDate.DAY_MAX+1),
					Widgets.GetLabel("."),
					Widgets.GetNumericUpDown(Controller.Id.DATE_MONTH,
						UTCDate.MONTH_MIN-1,
						UTCDate.MONTH_MAX+1),
					Widgets.GetLabel("."),
					Widgets.GetNumericUpDown(Controller.Id.DATE_YEAR,
						UTCDate.YEAR_MIN,
						UTCDate.YEAR_MAX),
					Widgets.GetLabelImage(Controller.Id.DATE_DSTSTATUS,
					    "dst-status-nodst.png")},
				new float[] {22F, 5F, 22F, 5F, 27F, 30F});
			
			Label time_lbl = Widgets.GetLabel("Time:");
			Control time_ins = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetNumericUpDown(Controller.Id.TIME_HOUR,
						UTCDate.HOUR_MIN-1,
						UTCDate.HOUR_MAX+1),
					Widgets.GetLabel(":"),
					Widgets.GetNumericUpDown(Controller.Id.TIME_MINUTE,
						UTCDate.MINUTE_MIN-1,
						UTCDate.MINUTE_MAX+1),
					Widgets.GetLabel(":"),
					Widgets.GetNumericUpDown(Controller.Id.TIME_SECOND,
						UTCDate.SECOND_MIN-1,
						UTCDate.SECOND_MAX+1),
					Widgets.GetButtonImageTextAnon(Controller.Id.TIMENOW_ACTION,
					                               "Now", "time-now.png")},
				new float[] {23F, 5F, 23F, 5F, 23F, 30F});

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

		private Control GetButtons()
		{
			Control btns = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabel(String.Empty), //layout buffer
					Widgets.GetButtonImageTextAnon(Controller.Id.RESETFORM_ACTION,
					                               "Reset", "reset.png"),
					Widgets.GetLabel(String.Empty),
					Widgets.GetButtonImageText(Controller.Id.RENDER_ACTION,
					                           "Render", "render.png"),
					Widgets.GetLabel(String.Empty)},
				new float[] {15F, 30F, 15F, 30F, 15F});
	
			return btns;
		}
	
		private Control GetOutputs()
		{
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(OUTPUTS_COUNT, 1,
			                                              FORM_MARGIN, FORM_PADDING);
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
	
			for (int i = 0; i < OUTPUTS_COUNT; i++) {
				layout.RowStyles.Add(new RowStyle(SizeType.Absolute, FORM_ROW_HEIGHT));
			}
	
			Control el = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabel("Sun elevation"),
					Widgets.GetLabel(":"),
					Widgets.GetTextBox(Controller.Id.ELEVATION, "-13.1231")},
					new float[] {19F, 2F, 25F});
	
			Control az = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabel("Sun azimuth"),
					Widgets.GetLabel(":"),
					Widgets.GetTextBox(Controller.Id.AZIMUTH, "212.6669")},
					new float[] {19F, 2F, 25F});
	
			Control noon = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabel("Solar noon"),
					Widgets.GetLabel(":"),
					Widgets.GetTextBox(Controller.Id.SOLAR_NOON, "12:12")},
					new float[] {19F, 2F, 25F});    
	
			Control rise = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabel("Sunrise/Sunset"),
					Widgets.GetLabel(":"),
					Widgets.GetTextBox(Controller.Id.SUNRISESUNSET, "06:09")},
					new float[] {19F, 2F, 25F});
	
			Control sset = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabel("Dawn/Dusk"),
					Widgets.GetLabel(":"),
					Widgets.GetTextBox(Controller.Id.DAWNDUSK, "18:15")},
					new float[] {19F, 2F, 25F});
	
			layout.Controls.Add(el, 0, 0);
			layout.Controls.Add(az, 0, 1);
			layout.Controls.Add(noon, 0, 2);
			layout.Controls.Add(rise, 0, 3);
			layout.Controls.Add(sset, 0, 4);
	
			return layout;
		}

		private Control GetImageSave()
		{
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(IMAGESAVE_COUNT, 1,
				                                                  FORM_MARGIN, FORM_PADDING);
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
	
			for (int i = 0; i < IMAGESAVE_COUNT; i++) {
				layout.RowStyles.Add(new RowStyle(SizeType.Absolute, FORM_ROW_HEIGHT));
			}
				
			Control btns = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabel(String.Empty), //layout buffer
					Widgets.GetNumericUpDown(
						Controller.Id.IMAGE_SIZE,
						300,
						10000),
					Widgets.GetLabel("pixels"),
					Widgets.GetButtonImageText(Controller.Id.IMAGESAVE_ACTION,
				                           "Save", "image-save.png"),
					Widgets.GetLabel(String.Empty)},
				new float[] {20F, 20F, 15F, 30F, 20F});

			Control check = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabel(String.Empty), //layout buffer
					Widgets.GetCheckBox(
						Controller.Id.IMAGE_CAPTIONTOGGLE,
						"Print caption below diagram",
						true),
					Widgets.GetLabel(String.Empty)},
				new float[] {20F, 64F, 20F});
	
			layout.Controls.Add(btns, 0, 0);
			layout.Controls.Add(check, 0, 1);
				
			return layout;
		}
	}
}
