// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

using LibSolar.Types;

namespace SolarbeamGui
{
	/**
	 * Handlers for gui events.
	 */
	partial class Controller
	{
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
				foreach (Id id in ins_render)
				{
					Control control = registry[id];
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
				if (!validating)
				{
					Position pos = ReadPosition();
					UTCDate? dt = ReadDate();
	
					if ((pos != null) && (dt != null ))
					{
						((GuiViewport) registry[Id.VIEWPORT]).Update(dt.Value);
						SetOutputs(pos, dt.Value);
					}
				}
			} catch (KeyNotFoundException) {}
		}
	
		/**
		 * Clear updates to controls that force viewport re-rendering.
		 */
		private static void ClearForm(object sender, EventArgs args)
		{
			foreach (Id id in ins_render)
			{
				Control control = registry[id];
				SetValue(control, cache[id]);
				UnMark(control);
			}
			foreach (Id id in ins_update)
			{
				Control control = registry[id];
				UnMark(control);
			}
		}
		
		/**
		 * Handle updates to controls that force viewport re-rendering by marking
		 * the control as having changed.
		 */
		private static void ValueChange(object sender, EventArgs args)
		{
			Control control = (Control) sender;
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
				if (!validating)
				{
					validating = true;
	
					Control control = (Control) sender;
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
	
					validating = false;      
				}
			} catch (KeyNotFoundException) {
				validating = false;  
			}
		}
		
		private static void GotFocus(object sender, EventArgs args)
		{
			// ignore exception when trying to obtain value of control that has none
			try {
				Control control = (Control) sender;
				string val = GetValue(control);
			
				if (control is NumericUpDown) {
					((NumericUpDown) control).Select(0, val.Length);
				}
			} catch (ArgumentException) {}
		}
		
		private static void LostFocus(object sender, EventArgs args)
		{
			Control control = (Control) sender;
	
			if (control is NumericUpDown) {
				((NumericUpDown) control).Select(0, 0);
			}
		}
	}
}