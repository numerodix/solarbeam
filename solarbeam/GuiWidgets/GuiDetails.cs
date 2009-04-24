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
		
		private const int FORM_ROW_HEIGHT = 28;
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
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(2, 1,
			                                                      FORM_MARGIN, FORM_PADDING);
			
			layout.Controls.AddRange(new Control[] {
				GetParameters(),
				GetSolarPosition(),
				GetSolarTimes(),
				GetDawnDusk(),
				GetButtons(),
				new Label(),
			});

			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, PARAMS_HEIGHT));
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, SOLARPOS_HEIGHT));
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, SOLARTIMES_HEIGHT));
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, DAWNDUSK_HEIGHT));
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, BUTTONS_HEIGHT));
			layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
			
			return layout;
		}
	
		private Control GetParameters()
		{
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(PARAMS_COUNT, 1,
			                                                      FORM_MARGIN, FORM_PADDING);
			
			Control loc = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Location"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_LOCATION, "Equator")},
					new float[] {7F, 2F, 25F});
			
			Control pos = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Position"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_POSITION, "Equator")},
					new float[] {7F, 2F, 25F});
			
			Control tz = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Timezone"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_TIMEZONE, "Equator")},
					new float[] {7F, 2F, 25F});
			
			Control date = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Date"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_DATE, "Equator")},
					new float[] {7F, 2F, 25F});			
			
			Control time = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Time"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_TIME, "Equator")},
					new float[] {7F, 2F, 25F});			
						
			layout.Controls.AddRange(new Control[] {
				loc,
				pos,
				tz,
				date,
				time,
			});
			
			for (int i = 0; i < layout.Controls.Count; i++) {
				layout.RowStyles.Add(new RowStyle(SizeType.Absolute, FORM_ROW_HEIGHT));
			}
			
			GroupBox group = new GroupBox();
			group.Text = "Parameters";
			group.Dock = DockStyle.Fill;
			group.Controls.Add(layout);
	
			return group;
		}
		
		private Control GetSolarPosition()
		{
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(SOLARPOS_COUNT, 1,
			                                                      FORM_MARGIN, FORM_PADDING);
			
			Control el = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Solar elevation"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_ELEVATION, "Equator")},
					new float[] {7F, 2F, 25F});
			
			Control az = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Solar azimuth"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_AZIMUTH, "Equator")},
					new float[] {7F, 2F, 25F});	
						
			layout.Controls.AddRange(new Control[] {
				el,
				az,
			});
			
			for (int i = 0; i < layout.Controls.Count; i++) {
				layout.RowStyles.Add(new RowStyle(SizeType.Absolute, FORM_ROW_HEIGHT));
			}
			
			GroupBox group = new GroupBox();
			group.Text = "Solar position";
			group.Dock = DockStyle.Fill;
			group.Controls.Add(layout);
	
			return group;
		}
		
		private Control GetSolarTimes()
		{
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(SOLARTIMES_COUNT, 1,
			                                                      FORM_MARGIN, FORM_PADDING);
			
			Control sunrise = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Sunrise"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_SUNRISE, "Equator")},
					new float[] {7F, 2F, 25F});
			
			Control solarnoon = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Solar noon"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_SOLARNOON, "Equator")},
					new float[] {7F, 2F, 25F});	
			
			Control sunset = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Sunset"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_SUNSET, "Equator")},
					new float[] {7F, 2F, 25F});	
			
			Control solardaylength = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Solar day length"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_SOLARDAYLENGTH, "Equator")},
					new float[] {7F, 2F, 25F});	
			
			layout.Controls.AddRange(new Control[] {
				sunrise,
				solarnoon,
				sunset,
				solardaylength,
			});
			
			for (int i = 0; i < layout.Controls.Count; i++) {
				layout.RowStyles.Add(new RowStyle(SizeType.Absolute, FORM_ROW_HEIGHT));
			}
			
			GroupBox group = new GroupBox();
			group.Text = "Solar times";
			group.Dock = DockStyle.Fill;
			group.Controls.Add(layout);
	
			return group;
		}
		
		private Control GetDawnDusk()
		{
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(DAWNDUSK_COUNT, 1,
			                                                      FORM_MARGIN, FORM_PADDING);
			
			Control dawn = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Dawn"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_DAWN, "Equator")},
					new float[] {7F, 2F, 25F});
			
			Control dusk = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Dusk"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_DUSK, "Equator")},
					new float[] {7F, 2F, 25F});	
			
			Control daylength = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Day length"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_DAYLENGTH, "Equator")},
					new float[] {7F, 2F, 25F});	
			
			layout.Controls.AddRange(new Control[] {
				dawn,
				dusk,
				daylength,
			});
			
			for (int i = 0; i < layout.Controls.Count; i++) {
				layout.RowStyles.Add(new RowStyle(SizeType.Absolute, FORM_ROW_HEIGHT));
			}
			
			GroupBox group = new GroupBox();
			group.Text = "Civil twilight (elevation -6°)";
			group.Dock = DockStyle.Fill;
			group.Controls.Add(layout);
	
			return group;
		}
		
		private Control GetButtons()
		{
			Control btns = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetButtonImageText(Controller.Id.DETAILSAVE_ACTION,
					                           "Save to file", "save.png"),
					Widgets.GetLabelAnon(String.Empty)},
				new float[] {20F, 80F});
	
			return btns;
		}
	}
}
