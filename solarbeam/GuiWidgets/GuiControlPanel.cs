// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using LibSolar.Assemblies;
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
	
	
			TableLayoutPanel layout = GuiCommon.GetTableLayoutPanel(PANEL_COUNT, 1, 
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
			TableLayoutPanel layout = GuiCommon.GetTableLayoutPanel(OUTPUTS_COUNT, 1,
			                                              FORM_MARGIN, FORM_PADDING);
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
	
			for (int i = 0; i < OUTPUTS_COUNT; i++) {
				layout.RowStyles.Add(new RowStyle(SizeType.Absolute, FORM_ROW_HEIGHT));
			}
	
			Control el = GetLaidOut(
					new Control[] {
					GetLabel("Sun elevation"),
					GetLabel(":"),
					GetTextBox(Controller.Id.ELEVATION, "-13.1231")},
					new float[] {19F, 2F, 23F});
	
			Control az = GetLaidOut(
					new Control[] {
					GetLabel("Sun azimuth"),
					GetLabel(":"),
					GetTextBox(Controller.Id.AZIMUTH, "212.6669")},
					new float[] {19F, 2F, 23F});
	
			Control rise = GetLaidOut(
					new Control[] {
					GetLabel("Sunrise"),
					GetLabel(":"),
					GetTextBox(Controller.Id.SUNRISE, "06:09")},
					new float[] {19F, 2F, 23F});
	
			Control noon = GetLaidOut(
					new Control[] {
					GetLabel("Solar noon"),
					GetLabel(":"),
					GetTextBox(Controller.Id.SOLAR_NOON, "12:12")},
					new float[] {19F, 2F, 23F});    
	
			Control sset = GetLaidOut(
					new Control[] {
					GetLabel("Sunset"),
					GetLabel(":"),
					GetTextBox(Controller.Id.SUNSET, "18:15")},
					new float[] {19F, 2F, 23F});
	
			Control dlen = GetLaidOut(
					new Control[] {
					GetLabel("Day length"),
					GetLabel(":"),
					GetTextBox(Controller.Id.DAY_LENGTH, "12h 0m")},
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
			Control btns = GetLaidOut(
				new Control[] {
					GetLabel(String.Empty), //layout buffer
					GetButton(Controller.Id.CLEARFORM_ACTION, "Reset"),
					GetLabel(String.Empty),
					GetButton(Controller.Id.RENDER_ACTION, "Render"),
					GetLabel(String.Empty)},
				new float[] {15F, 30F, 15F, 30F, 15F});
	
			return btns;
		}
	
		private Control GetInputs()
		{
			TableLayoutPanel layout = GuiCommon.GetTableLayoutPanel(INPUTS_COUNT, 3, 
			                                              FORM_MARGIN, FORM_PADDING);
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
	
			for (int i = 0; i < INPUTS_COUNT; i++) {
				layout.RowStyles.Add(new RowStyle(SizeType.Absolute, FORM_ROW_HEIGHT));
			}
			
			Label loc_lbl = GetLabel("Location:");
			Control loc_ins = GetLaidOut(
				new Control[] {
					GetComboBoxInputable(
						Controller.Id.LOCATION,
						Controller.locations_source.Locations),
					GetButtonImaged(Controller.Id.LOCATIONSAVE_ACTION, "document-save.png"),
					GetButtonImaged(Controller.Id.LOCATIONDELETE_ACTION, "edit-delete.png")},
				new float[] {74F, 12F, 12F});
	
			Label lat_lbl = GetLabel("Latitude:");
			ComboBox lat_dir = GetComboBox(
				Controller.Id.LATITUDE_DIRECTION,
				Controller.position_source.LatitudeDirections);
			Control lat_ins = GetLaidOut(
				new Control[] {
					GetNumericUpDown(Controller.Id.LATITUDE_DEGS,
				                 Position.LATDEGS_MINVALUE-1,
				                 Position.LATDEGS_MAXVALUE),
					GetNumericUpDown(Controller.Id.LATITUDE_MINS,
				                 Position.LATMINS_MINVALUE-1,
				                 Position.LATMINS_MAXVALUE+1),
					GetNumericUpDown(Controller.Id.LATITUDE_SECS,
				                 Position.LATSECS_MINVALUE-1,
				                 Position.LATSECS_MAXVALUE+1),
				lat_dir},
				new float[] {27F, 23F, 23F, 30F});
			
			Label lon_lbl = GetLabel("Longitude:");
			ComboBox lon_dir = GetComboBox(
				Controller.Id.LONGITUDE_DIRECTION,
				Controller.position_source.LongitudeDirections);
			Control lon_ins = GetLaidOut(
				new Control[] {
					GetNumericUpDown(Controller.Id.LONGITUDE_DEGS,
				                 Position.LONDEGS_MINVALUE-1,
				                 Position.LONDEGS_MAXVALUE),
					GetNumericUpDown(Controller.Id.LONGITUDE_MINS,
				                 Position.LONMINS_MINVALUE-1,
				                 Position.LONMINS_MAXVALUE+1),
					GetNumericUpDown(Controller.Id.LONGITUDE_SECS,
				                 Position.LONSECS_MINVALUE-1,
				                 Position.LONSECS_MAXVALUE+1),
				lon_dir},
				new float[] {27F, 23F, 23F, 30F});
			
			Label tz_lbl = GetLabel("Timezone:");
			Control tz_in = GetLaidOut(
				new Control[] {
				GetComboBox(Controller.Id.TIMEZONE_OFFSET,
					Controller.timezone_source.Offsets),
				GetComboBox(Controller.Id.TIMEZONE_NAME,
					Controller.timezone_source.GetTimezones(
						Controller.timezone_source.Offsets[0]))
					},
					new float[] {24F, 50F});

			Label date_lbl = GetLabel("Date:");
			Control date_scr = GetLabel(String.Empty);
			Control date_ins = GetLaidOut(
				new Control[] {
					GetNumericUpDown(Controller.Id.DATE_DAY,
				                 UTCDate.DAY_MINVALUE-1,
				                 UTCDate.DAY_MAXVALUE+1),
					GetNumericUpDown(Controller.Id.DATE_MONTH,
				                 UTCDate.MONTH_MINVALUE-1,
				                 UTCDate.MONTH_MAXVALUE+1),
					GetNumericUpDown(Controller.Id.DATE_YEAR,
				                 UTCDate.YEAR_MINVALUE,
				                 UTCDate.YEAR_MAXVALUE),
				date_scr},
				new float[] {20F, 20F, 25F, 30F});
			
			Label time_lbl = GetLabel("Time:");
			Control time_scr = GetLabel(String.Empty);
			Control time_ins = GetLaidOut(
				new Control[] {
					GetNumericUpDown(Controller.Id.TIME_HOUR,
				                 UTCDate.HOUR_MINVALUE-1,
				                 UTCDate.HOUR_MAXVALUE+1),
					GetNumericUpDown(Controller.Id.TIME_MINUTE,
				                 UTCDate.MINUTE_MINVALUE-1,
				                 UTCDate.MINUTE_MAXVALUE+1),
					GetNumericUpDown(Controller.Id.TIME_SECOND,
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
		
		private TableLayoutPanel GetLaidOut(Control[] controls, float[] widths)
		{
			TableLayoutPanel layout = new TableLayoutPanel();
			layout.Dock = DockStyle.Fill;
			layout.ColumnCount = controls.Length;
			layout.RowCount = 1;
			
			foreach (float width in widths)
			{
				layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, width));
			}
	
			foreach (Control c in controls)
			{
				c.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				layout.Controls.Add(c);
			}
			return layout;
		}
		
		private Button GetButton(Controller.Id id, string s)
		{
			Button button = new Button();
			button.Text = s;
			Controller.RegisterControl(id, button);	// register control
			return button;
		}
			
		private Button GetButtonImaged(Controller.Id id, string path)
		{
			Button button = new Button();
			button.FlatAppearance.BorderSize = 0;
			button.FlatStyle = FlatStyle.Flat;
			AsmInfo asminfo = new AsmInfo(Assembly.GetExecutingAssembly());
			Stream stream = asminfo.GetResource(path);
			button.Image = new Bitmap(stream);
			Controller.RegisterControl(id, button);	// register control
			return button;
		}
		
		private Label GetLabel(string s)
		{
			Label label = new Label();
			label.Text = s;
			label.AutoSize = true;
			label.Anchor = AnchorStyles.Left;
			return label;
		}
		
		private TextBox GetTextBox(Controller.Id id, string s)
		{
			TextBox textbox = new TextBox();
			textbox.ReadOnly = true;
			textbox.Anchor = AnchorStyles.Left;
			textbox.BorderStyle = BorderStyle.None;
			Controller.RegisterControl(id, textbox);	// register control
			return textbox;
		}

		private ComboBox GetComboBox(Controller.Id id, string[] ss)
		{
			ComboBox combo = new ComboBox();
			combo.DropDownStyle = ComboBoxStyle.DropDownList;
			combo.Items.AddRange(ss);
			combo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			Controller.RegisterControl(id, combo);	// register control
			return combo;
		}
			
		private ComboBox GetComboBoxInputable(Controller.Id id, string[] ss)
		{
			ComboBox combo = GetComboBox(id, ss);
			combo.DropDownStyle = ComboBoxStyle.DropDown;
			combo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			combo.AutoCompleteSource = AutoCompleteSource.ListItems;
			combo.DropDownHeight = 180;
			return combo;
		}
		
		private NumericUpDown GetNumericUpDown(Controller.Id id, int min, int max)
		{
			NumericUpDown num = new NumericUpDown();
			num.Minimum = min;
			num.Maximum = max;
			Controller.RegisterControl(id, num);	// register control
			return num;
		}
	}
}
