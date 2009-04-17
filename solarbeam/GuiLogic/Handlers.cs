// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

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
			string location = GetValue(registry[Id.LOCATION]);
			Position pos = ReadPosition();
			UTCDate? date = ReadDate();
			
			if ((pos != null) && (date != null)) {
				UTCDate dt = date.Value;
				
				string filename = Formatter.FormatSessionFilename(location, pos, dt);
				SaveFileDialog dlg = Widgets.GetSaveFileDialog(filename,
				                                               Formatter.SessionFileDesc,
				                                               Formatter.SessionFileFilter);
				DialogResult ans = dlg.ShowDialog();
				filename = dlg.FileName;
				
				if (ans == DialogResult.OK) {
					WriteSession(filename);
				}
			}
		}
		
		private static void LoadSession(object sender, EventArgs args)
		{
			OpenFileDialog dlg = Widgets.GetOpenFileDialog(Formatter.SessionFileDesc,
			                                               Formatter.SessionFileFilter);
			DialogResult ans = dlg.ShowDialog();
			string filename = dlg.FileName;
			
			if (ans == DialogResult.OK) {
				ReadSession(filename);
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
			
			string name = GetValue(registry[Id.LOCATION]);
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
			
			LocationsSource.StoreList();
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
		 * Clear updates to controls that force viewport re-rendering.
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
		 * Viewport forced to re-render.
		 */
		public static void RenderViewport(object sender, EventArgs args)
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
	
				((GuiViewport) registry[Id.VIEWPORT]).ReRender(pos, dt.Value);
				SetOutputs(pos, dt.Value);
			}
		}
		
		/**
		 * Viewport is updated with current day plot, existing rendering used.
		 */
		private static void UpdateViewport(object sender, EventArgs args)
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
						((GuiViewport) registry[Id.VIEWPORT]).Update(dt.Value);
						SetOutputs(pos, dt.Value);
					}
				}
			} catch (KeyNotFoundException) {}
		}
		
		private static void SaveImage(object sender, EventArgs args)
		{
			int dim = GetInt(GetValue(registry[Id.IMAGE_SIZE]));
			string location = GetValue(registry[Id.LOCATION]);
			Position pos = ReadPosition();
			UTCDate? date = ReadDate();
			Colors colors = GuiViewport.colors;
			string font_face = GuiViewport.font_face;
			
			if ((pos != null) && (date != null)) {
				UTCDate dt = date.Value;
				
				string filename = Formatter.FormatImgFilename(location, pos, dt);
				SaveFileDialog dlg = Widgets.GetSaveFileDialog(filename,
				                                               Formatter.ImageFileDesc,
				                                               Formatter.ImageFileFilter);
				DialogResult ans = dlg.ShowDialog();
				filename = dlg.FileName;
			
				if (ans == DialogResult.OK) {
					GraphBitmap grbit = new GraphBitmap(true, dim, colors, font_face);
					Bitmap bitmap_plain = grbit.RenderBaseImage(pos, dt);
					Bitmap bitmap_final = grbit.RenderCurrentDay(bitmap_plain, dim, pos, dt);
					grbit.SaveBitmap(bitmap_final, filename);
				}
			}
		}
		
		private static void ShowAboutDialog(object sender, EventArgs args)
		{
			GuiMainForm.aboutform.Show();
		}
		
		private static void HideAboutDialog(object sender, EventArgs args)
		{
			GuiMainForm.aboutform.Close();
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
		 * Handle updates to controls that force viewport re-rendering by marking
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
