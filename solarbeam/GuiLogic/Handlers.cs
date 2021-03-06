// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using LibSolar;
using LibSolar.Formatting;
using LibSolar.Graphing;
using LibSolar.Types;
using LibSolar.Util;

namespace SolarbeamGui
{
	/**
	 * Handlers for gui events.
	 */
	partial class Controller
	{
		private static void SaveSession(object sender, EventArgs args)
		{
			string location = ReadLocation();
			Position pos = ReadPosition();
			UTCDate? date = ReadDate();
			
			if ((pos != null) && (date != null)) {
				UTCDate dt = date.Value;
				
				string filename = Formatter.FormatSessionFilename(location, pos, dt);
				SaveFileDialog dlg = Widgets.GetSaveFileDialog(filename,
				                                               Constants.SessionFileDesc,
				                                               Constants.SessionFileFilter);
				DialogResult ans = dlg.ShowDialog();
				filename = dlg.FileName;
				
				if (ans == DialogResult.OK) {
					try {
						WriteSession(filename);
						Controller.Report(new Message(Result.OK, "Saved session to: " + filename));
					} catch {
						Controller.Report(new Message(Result.Fail, "Failed to save session to: " + filename));
					}
				}
			}
		}
		
		private static void LoadSession(object sender, EventArgs args)
		{
			OpenFileDialog dlg = Widgets.GetOpenFileDialog(Constants.SessionFileDesc,
			                                               Constants.SessionFileFilter);
			DialogResult ans = dlg.ShowDialog();
			string filename = dlg.FileName;
			
			if (ans == DialogResult.OK) {
				try {
					ReadSession(filename);
					Controller.Report(new Message(Result.OK, "Loaded session from: " + filename));
				} catch {
					Controller.Report(new Message(Result.Fail, "Failed to load session from: " + filename));
				}
			}
		}
		
		private static void Exit(object sender, EventArgs args)
		{
			MainGui.Quit();
		}
			
		private static void NewLocation(object sender, EventArgs args)
		{
			ComboBox loc_control = (ComboBox) registry[Id.LOCATION];
			loc_control.Items.Add(String.Empty);
			
			SetLocation(String.Empty);
			SetPosition(new Position(Position.LATITUDE_POS, 0, 0, 0,
			                         Position.LONGITUDE_POS, 0, 0, 0));
			SetTimezone("UTC");
			UnMark(ins_position);
			
			// set focus on the control
			loc_control.Focus();
		}

		private static void SaveLocation(object sender, EventArgs args)
		{
			ComboBox loc_control = (ComboBox) registry[Id.LOCATION];
			
			string name = ReadLocation();
			Position pos = ReadPosition();
			string tz = GetValue(registry[Id.TIMEZONE_NAME]);
			
			if (name == String.Empty) {
				return;
			}
			
			// new name, treat as a new location
			if (!LocationsSource.ContainsLocation(name)) {
				// add to combobox list
				if (!loc_control.Items.Contains(name)) {
					loc_control.Items.Add(name);
				}
				// add to LocationsSource
				LocationsSource.AddLocation(name, tz, pos);
			// same name, parameters changed
			} else {
				LocationsSource.UpdateLocation(name, tz, pos);
			}
			
			try {
				LocationsSource.StoreList();
				Controller.Report(new Message(Result.OK, "Saved location to: " + LocationsSource.File));
			} catch {
				Controller.Report(new Message(Result.Fail, "Failed to save location to: " + LocationsSource.File));
			}
		}
		
		private static void DeleteLocation(object sender, EventArgs args)
		{
			ComboBox control = (ComboBox) registry[Id.LOCATION];
			string val = GetValue(control);
			int idx = control.SelectedIndex;
			
			control.Items.RemoveAt(idx);
			LocationsSource.RemoveLocation(val);
			
			// move selection to item below in the list
			idx = Math.Max(0, idx - 1);
			control.SelectedIndex = idx;
		}
		
		private static void SetTimeNow(object sender, EventArgs args)
		{
			SetDateTime(DateTime.Now);
		}
		
		/**
		 * Clear updates to controls that force diagram re-rendering.
		 */
		private static void ResetForm(object sender, EventArgs args)
		{
			foreach (Id id in ins_position)
			{
				Component control = (Component) registry[id];
				SetValue(control, cache[id]);
				UnMark(control);
			}
			foreach (Id id in ins_timedate)
			{
				Component control = (Component) registry[id];
				UnMark(control);
			}
		}
		
		/**
		 * Diagram forced to re-render.
		 */
		public static void RenderDiagram(object sender, EventArgs args)
		{
			Position pos = ReadPosition();
			UTCDate? dt = ReadDate();
	
			if ((pos != null) && (dt != null))
			{
				// clear form changes, update cache with new values
				foreach (Id id in ins_position)
				{
					Component control = (Component) registry[id];
					UnMark(control);
					cache[id] = GetValue(control);
				}
	
				((GuiDiagram) registry[Id.DIAGRAM]).ReRender(pos, dt.Value);
				SetOutputs(pos, dt.Value);
			}
		}
		
		/**
		 * Diagram is updated with current day plot, existing rendering used.
		 */
		private static void UpdateDiagram(object sender, EventArgs args)
		{
			// value changes will occur during control initialization, sometimes 
			// before all controls have been registered. ignore this early case
			try {
				// don't update while validating, input may be partial
				if (!validate_lock)
				{
					Position pos = ReadPosition();
					UTCDate? dt = ReadDate();
	
					if ((pos != null) && (dt != null )) {
						((GuiDiagram) registry[Id.DIAGRAM]).Update(dt.Value);
						SetOutputs(pos, dt.Value);
					}
				}
			} catch (KeyNotFoundException) {}
		}
		
		public static void UpdateDetails(object sender, EventArgs args)
		{
			// value changes will occur during control initialization, sometimes 
			// before all controls have been registered. ignore this early case
			try {
				// don't update while validating, input may be partial
				if (!validate_lock)
				{
					Position pos = ReadPosition();
					string location = ReadLocation();
					UTCDate? dt = ReadDate();
					
					if ((pos != null) && (dt != null )) {
						SetDetails(location, pos, dt.Value);
					}
				}
			} catch (KeyNotFoundException) {}
		}
		
		/**
		 * Map is updated with current day plot, existing rendering used.
		 */
		public static void UpdateMap(object sender, EventArgs args)
		{
			// value changes will occur during control initialization, sometimes 
			// before all controls have been registered. ignore this early case
			try {
				// don't update while validating, input may be partial
				if (!validate_lock)
				{
					Position pos = ReadPosition();
					string location = ReadLocation();
	
					if (pos != null) {
						((GuiMap) registry[Id.MAP]).Update(location, pos);
					}
				}
			} catch (KeyNotFoundException) {}
		}
		
		private static void MapClick(object sender, MouseEventArgs args)
		{
			GuiMap guimap = ((GuiMap) registry[Id.MAP]);
			Position pos = guimap.FindPosition(args.X, args.Y);
			
			int tz = Position.GetGeographicTimezoneOffset(pos);
			string sign = tz < 0 ? "+" : "-";
			
			// use validate lock to prevent mouse jumps
			validate_lock = true;
			SetLocation(string.Empty);
			SetPosition(pos);
			try {
				SetTimezone(string.Format("Etc/GMT{0}{1}", sign, Math.Abs(tz)));
			} catch (KeyNotFoundException) {
				SetTimezone("UTC");
			}
			validate_lock = false;
			
			UpdateDetails(sender, args); // update details pane
		}
		
		private static void SaveImage(object sender, EventArgs args)
		{
			int dim = GetInt(GetValue(registry[Id.IMAGE_SIZE]));
			string location = ReadLocation();
			Position pos = ReadPosition();
			UTCDate? date = ReadDate();
			Colors colors = GuiDiagram.colors;
			string font_face = GuiDiagram.font_face;
			
			if ((pos != null) && (date != null)) {
				UTCDate dt = date.Value;
				
				string filename = Formatter.FormatImgFilename(location, pos, dt);
				SaveFileDialog dlg = Widgets.GetSaveFileDialog(filename,
				                                               Constants.ImageFileDesc,
				                                               Constants.ImageFileFilter);
				DialogResult ans = dlg.ShowDialog();
				filename = dlg.FileName;
			
				if (ans == DialogResult.OK) {
					bool caption_b = GetBool(GetValue(registry[Id.IMAGE_CAPTIONTOGGLE]));
					bool sun_b = GetBool(GetValue(registry[Id.IMAGE_SUNTOGGLE]));
					
					GraphBitmap grbit = new GraphBitmap(caption_b, dim, colors, font_face);
					Bitmap bitmap = grbit.RenderBaseImage(pos, dt);
					if (sun_b)
						bitmap = grbit.RenderCurrentDay(bitmap, pos, dt);
					if (caption_b)
						bitmap = grbit.RenderCaption(new CaptionInfo(location, pos, dt));
					
					try {
						grbit.SaveBitmap(bitmap, filename);
						Controller.Report(new Message(Result.OK, "Saved image to: " + filename));
					} catch {
						Controller.Report(new Message(Result.Fail, "Failed to save image to: " + filename));
					}
				}
			}
		}
		
		private static void SaveDetails(object sender, EventArgs args)
		{
			string location = ReadLocation();
			Position position = ReadPosition();
			UTCDate? nudt = ReadDate();
			
			string loc = GetValue(registry[Id.DETAIL_LOCATION]);
			string pos = GetValue(registry[Id.DETAIL_POSITION]);
			string tz = GetValue(registry[Id.DETAIL_TIMEZONE]);
			string date = GetValue(registry[Id.DETAIL_DATE]);
			string time = GetValue(registry[Id.DETAIL_TIME]);
			string el = GetValue(registry[Id.DETAIL_ELEVATION]);
			string az = GetValue(registry[Id.DETAIL_AZIMUTH]);
			string sunrise = GetValue(registry[Id.DETAIL_SUNRISE]);
			string noon = GetValue(registry[Id.DETAIL_SOLARNOON]);
			string sunset = GetValue(registry[Id.DETAIL_SUNSET]);
			string solardaylength = GetValue(registry[Id.DETAIL_SOLARDAYLENGTH]);
			string dawn = GetValue(registry[Id.DETAIL_DAWN]);
			string dusk = GetValue(registry[Id.DETAIL_DUSK]);
			string daylength = GetValue(registry[Id.DETAIL_DAYLENGTH]);
			Details details = new Details(loc, pos, tz, date, time, el, az,
			                              sunrise, noon, sunset, solardaylength,
			                              dawn, dusk, daylength);

			if ((position != null) && (nudt != null)) {
				UTCDate udt = nudt.Value;
				
				string filename = Formatter.FormatTxtFilename(location, position, udt);
				SaveFileDialog dlg = Widgets.GetSaveFileDialog(filename,
				                                               Constants.TextFileDesc,
				                                               Constants.TextFileFilter);
				DialogResult ans = dlg.ShowDialog();
				filename = dlg.FileName;
			
				if (ans == DialogResult.OK) {
					try {
						details.Write(filename);
						Controller.Report(new Message(Result.OK, "Saved details to: " + filename));
					} catch {
						Controller.Report(new Message(Result.Fail, "Failed to save details to: " + filename));
					}
				}
			}
		}
		
		private static void ShowHelpDesc(object sender, EventArgs args)
		{
			Process.Start(Constants.URL_HELP);
		}
		
		private static void UpdateDSTStatus(object sender, EventArgs args)
		{
			try { // handle trigger before all controls have been registered
				if (!validate_lock) { // don't read while validating
					UTCDate? n_udt = ReadDate();
					
					if (n_udt != null) {
						UTCDate udt = n_udt.Value;
						DSTStatus status = TimezoneSource.GetDSTStatus(udt);
						
						string img = "dst-status-nodst.png";
						switch (status) {
						case DSTStatus.Standard:
							img = "dst-status-standard.png"; break;
						case DSTStatus.Daylight:
							img = "dst-status-daylight.png"; break;
						}
						SetImage(registry[Id.DATE_DSTSTATUS], img);
						
						string tip = Tooltips.GetTipDst(status);
						UpdateTooltip(registry[Id.DATE_DSTSTATUS], tip);
					}
				}
			} catch (KeyNotFoundException) {}
		}
	
		/**
		 * Handle updates to controls that force diagram re-rendering by marking
		 * the control as having changed.
		 */
		private static void ValueChange(object sender, EventArgs args)
		{
			Component control = (Component) sender;
			Id id = reg_rev[control];
			string val = GetValue(control);
			
			if (val != cache[id])
			{
				Mark(control);
			} else {
				UnMark(control);
			}
		}
	
		private static void Validate(object sender, EventArgs args)
		{
			// value changes will occur during control initialization, sometimes 
			// before all controls have been registered. ignore this early case
			try {
				// awful hack to prevent recursive calls causing infinite loop
				if (!validate_lock)
				{
					validate_lock = true;
	
					Component control = (Component) sender;
					Id id = reg_rev[control];
	
					if (id == Id.LOCATION) {
						ValidateLocation();
					} else if (ins_longitude.Contains(id)) {
						ValidatePosition(PositionAxis.Longitude);
					} else if (ins_latitude.Contains(id)) {
						ValidatePosition(PositionAxis.Latitude);
					} else if (id == Id.TIMEZONE_OFFSET) {
						ValidateTimezoneOffset();
					} else if (ins_date.Contains(id)) {
						ValidateDate();
					} else if (ins_time.Contains(id)) {
						ValidateTime();
					}
	
					validate_lock = false;      
				}
			} catch (KeyNotFoundException) {
				validate_lock = false;  
			}
		}
		
		private static void GotFocus(object sender, EventArgs args)
		{
			// ignore exception when trying to obtain value of control that has none
			try {
				Component control = (Component) sender;
				string val = GetValue(control);
			
				if (control is NumericUpDown) {
					((NumericUpDown) control).Select(0, val.Length);
				}
			} catch (ArgumentException) {}
		}
		
		private static void LostFocus(object sender, EventArgs args)
		{
			Component control = (Component) sender;
	
			if (control is NumericUpDown) {
				((NumericUpDown) control).Select(0, 0);
			}
		}
	}
}
