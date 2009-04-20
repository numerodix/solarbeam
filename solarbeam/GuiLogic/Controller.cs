// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using LibSolar.Types;
using LibSolar.Util;

namespace SolarbeamGui
{
	/**
	 * Administers all widgets.
	 */
	static partial class Controller
	{
		// Identify all widgets
		public enum Id {
			MENUEXIT_ACTION,
			MENUSESSIONSAVE_ACTION,
			MENUSESSIONLOAD_ACTION,
			MENULOCNEW_ACTION,
			MENULOCSAVE_ACTION,
			MENULOCDELETE_ACTION,
			MENUTIMENOW_ACTION,
			MENURESETFORM_ACTION,
			MENURENDER_ACTION,
			MENUSAVEIMAGE_ACTION,
			MENUABOUT_ACTION,
			LOCATION,
			LOCATIONNEW_ACTION,
			LOCATIONSAVE_ACTION,
			LOCATIONDELETE_ACTION,
			LATITUDE_DEGS,
			LATITUDE_MINS,
			LATITUDE_SECS,
			LATITUDE_DIRECTION,
			LONGITUDE_DEGS,
			LONGITUDE_MINS,
			LONGITUDE_SECS,
			LONGITUDE_DIRECTION,
			TIMEZONE_OFFSET,
			TIMEZONE_NAME,
			DATE_DAY,
			DATE_MONTH,
			DATE_YEAR,
			DATE_DSTSTATUS,
			TIME_HOUR,
			TIME_MINUTE,
			TIME_SECOND,
			TIMENOW_ACTION,
			RESETFORM_ACTION,
			RENDER_ACTION,
			ELEVATION,
			AZIMUTH,
			SOLAR_NOON,
			SUNRISESUNSET,
			DAWNDUSK,
			IMAGE_SIZE,
			IMAGE_CAPTIONTOGGLE,
			IMAGESAVE_ACTION,
			DIAGRAM,
			MAP,

			ABOUTCLOSE_ACTION,
		}
		
		// inputs whose updates force a complete re-rendering
		private static StaticList<Id> ins_position = new StaticList<Id>(new Id[] {
			Id.LOCATION,
			Id.LATITUDE_DEGS, Id.LATITUDE_MINS, Id.LATITUDE_SECS, Id.LATITUDE_DIRECTION,
			Id.LONGITUDE_DEGS, Id.LONGITUDE_MINS, Id.LONGITUDE_SECS, Id.LONGITUDE_DIRECTION,
			Id.TIMEZONE_OFFSET, Id.TIMEZONE_NAME
		});
		
		// inputs whose updates trigger a plot update without re-rendering
		private static StaticList<Id> ins_timedate = new StaticList<Id>(new Id[] {
			Id.DATE_DAY, Id.DATE_MONTH, Id.DATE_YEAR,
			Id.TIME_HOUR, Id.TIME_MINUTE, Id.TIME_SECOND
		});
		
		// latitude inputs
		private static StaticList<Id> ins_latitude = new StaticList<Id>(new Id[] {
			Id.LATITUDE_DEGS, Id.LATITUDE_MINS, Id.LATITUDE_SECS
		});
	
		// longitude inputs
		private static StaticList<Id> ins_longitude = new StaticList<Id>(new Id[] {
			Id.LONGITUDE_DEGS, Id.LONGITUDE_MINS, Id.LONGITUDE_SECS
		});
	
		// date inputs
		private static StaticList<Id> ins_date = new StaticList<Id>(new Id[] {
			Id.DATE_DAY, Id.DATE_MONTH, Id.DATE_YEAR
		});
	
		// time inputs
		private static StaticList<Id> ins_time = new StaticList<Id>(new Id[] {
			Id.TIME_HOUR, Id.TIME_MINUTE, Id.TIME_SECOND
		});
	

		// the registry maps control identifiers onto widget objects
		private static Dictionary<Id,Component> registry =
			new Dictionary<Id,Component>();
		// the reverse registry is the opposite mapping for lookup on widget objects
		private static Dictionary<Component,Id> reg_rev =
			new Dictionary<Component,Id>();
		

		// the cache stores the value of a control when submission occurs
		// when a value change is detected on the widget, we can tell if the
		// value really has changed
		private static Dictionary<Id,string> cache =
			new Dictionary<Id,string>();
		
		// map controls onto tooltip objects for tooltip clustering
		private static Dictionary<Id,ToolTip> tooltips = new Dictionary<Id,ToolTip>();
		

		// central access to widget source providers
		public static LocationsSource LocationsSource;
		public static PositionSource PositionSource;
		public static TimezoneSource TimezoneSource;
								
		// reference point for assembly queries
		public static AsmInfo AsmInfo;
				
		// add strings to be shown in splash screen
		volatile public static Queue<string> SplashQueue = new Queue<string>();
		

		static Controller()
		{
			Controller.AsmInfo = new AsmInfo(Assembly.GetExecutingAssembly());
		}
									
		/**
		 * Initialize data sources, potentially expensive.
		 */
		public static void InitSources()
		{
			if (LocationsSource == null) {
				Controller.SplashQueue.Enqueue("Loading location list");
				LocationsSource = new LocationsSource();
			}
			if (PositionSource == null) {
				PositionSource = new PositionSource();
			}
			if (TimezoneSource == null) {
				Controller.SplashQueue.Enqueue("Loading timezones");
				TimezoneSource = new TimezoneSource();
			}
		}

		/**
		 * A single point of call where all widgets get their handlers set up.
		 */
		public static void RegisterControl(Id id, Component control)
		{
			// register in registry
			registry.Add(id, control);
			reg_rev.Add(control, id);
			cache.Add(id, String.Empty);
			
			// activate buttons
			if ((control is Button)
					|| (control is ToolStripButton)
					|| (control is ToolStripMenuItem)) {
				ActivateButton(control);
			}
		
			// select on focus
			RegisterFocus(control);
		
			// validate all inputs before dispatching handler acting on new value
			if (ins_position.Contains(id) || ins_timedate.Contains(id)) {
				EventHandler handler = new EventHandler(Validate);
				RegisterValueChange(control, handler);
			}
	
			// register diagram re-rendering inputs for value changes
			if (ins_position.Contains(id)) {
				EventHandler handler = new EventHandler(ValueChange);
				RegisterValueChange(control, handler);
			}

			// register handler for daylight time status update
			if (ins_date.Contains(id) || (ins_position.Contains(id))) {
				EventHandler handler = new EventHandler(UpdateDSTStatus);
				RegisterValueChange(control, handler);
			}
			
			// register diagram updating inputs for value changes
			if (ins_timedate.Contains(id)) {
				EventHandler handler = new EventHandler(UpdateDiagram);
				RegisterValueChange(control, handler);
			}
									
			// register map updating inputs for value changes
			if (ins_position.Contains(id)) {
				EventHandler handler = new EventHandler(UpdateMap);
				RegisterValueChange(control, handler);
			}
		
			// activate tooltip
			ActivateTooltip(id, control);
		
			// set initial value (make sure to disable validation)
			InitControl(control);
		}
		
		private static void ActivateButton(Component button)
		{
			// MENU ITEMS
		
			// File
			if (reg_rev[button] == Id.MENUSESSIONSAVE_ACTION) {
				((ToolStripMenuItem) button).Click += new EventHandler(SaveSession);
			} else if (reg_rev[button] == Id.MENUSESSIONLOAD_ACTION) {
				((ToolStripMenuItem) button).Click += new EventHandler(LoadSession);
			} else if (reg_rev[button] == Id.MENUEXIT_ACTION) {
				((ToolStripMenuItem) button).Click += new EventHandler(Exit);
			// Locations
			} else if (reg_rev[button] == Id.MENULOCNEW_ACTION) {
				((ToolStripMenuItem) button).Click += new EventHandler(NewLocation);
			} else if (reg_rev[button] == Id.MENULOCSAVE_ACTION) {
				((ToolStripMenuItem) button).Click += new EventHandler(SaveLocation);
			} else if (reg_rev[button] == Id.MENULOCDELETE_ACTION) {
				((ToolStripMenuItem) button).Click += new EventHandler(DeleteLocation);
			// Actions
			} else if (reg_rev[button] == Id.MENUTIMENOW_ACTION) {
				((ToolStripMenuItem) button).Click += new EventHandler(SetTimeNow);
			} else if (reg_rev[button] == Id.MENURESETFORM_ACTION) {
				((ToolStripMenuItem) button).Click += new EventHandler(ResetForm);
			} else if (reg_rev[button] == Id.MENURENDER_ACTION) {
				((ToolStripMenuItem) button).Click += new EventHandler(RenderDiagram);
			} else if (reg_rev[button] == Id.MENUSAVEIMAGE_ACTION) {
				((ToolStripMenuItem) button).Click += new EventHandler(SaveImage);
			// Help
			} else if (reg_rev[button] == Id.MENUABOUT_ACTION) {
				((ToolStripMenuItem) button).Click += new EventHandler(ShowAboutDialog);

			// FORM BUTTONS
			
			} else if (reg_rev[button] == Id.LOCATIONNEW_ACTION) {
				((Button) button).Click += new EventHandler(NewLocation);
			} else if (reg_rev[button] == Id.LOCATIONSAVE_ACTION) {
				((Button) button).Click += new EventHandler(SaveLocation);
			} else if (reg_rev[button] == Id.LOCATIONDELETE_ACTION) {
				((Button) button).Click += new EventHandler(DeleteLocation);
										
			} else if (reg_rev[button] == Id.TIMENOW_ACTION) {
				((Button) button).Click += new EventHandler(SetTimeNow);

			} else if (reg_rev[button] == Id.RESETFORM_ACTION) {
				((Button) button).Click += new EventHandler(ResetForm);
			} else if (reg_rev[button] == Id.RENDER_ACTION) {
				((Button) button).Click += new EventHandler(RenderDiagram);

			} else if (reg_rev[button] == Id.IMAGESAVE_ACTION) {
				((Button) button).Click += new EventHandler(SaveImage);
			

			// About form
			} else if (reg_rev[button] == Id.ABOUTCLOSE_ACTION) {
				((Button) button).Click += new EventHandler(HideAboutDialog);
			}
		}
									
		private static void RegisterFocus(Component control)
		{
			if (control is Control) {
				((Control) control).GotFocus += new EventHandler(GotFocus);
				((Control) control).LostFocus += new EventHandler(LostFocus);
			}
		}
			
		private static void RegisterValueChange(Component control, 
									            EventHandler handler)
		{
			if (control is ComboBox) {
				((ComboBox) control).SelectedValueChanged += handler;
			} else if (control is NumericUpDown) {
				((NumericUpDown) control).ValueChanged += handler;
			} else if (control is TextBox) {
				((TextBox) control).TextChanged += handler;
			}
		}

		private static void ActivateTooltip(Id id, Component control)
		{
			string tip = Tooltips.GetTip(id);
			if (tip != null) {
				if (control is Control) {
					tooltips[id] = Widgets.GetToolTipInfo(Tooltips.GetTitle(id));
					tooltips[id].StripAmpersands = true; // tips also used for menu items
					tooltips[id].SetToolTip((Control) control, tip);
				}
			}
		}
	}
}
