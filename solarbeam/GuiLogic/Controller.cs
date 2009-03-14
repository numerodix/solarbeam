// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using LibSolar.Types;

namespace SolarbeamGui
{
	/**
	 * Administers all widgets.
	 */
	static partial class Controller
	{
		// Identify all widgets
		public enum Id {
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
			TIME_HOUR,
			TIME_MINUTE,
			TIME_SECOND,
			CLEARFORM_ACTION,
			RENDER_ACTION,
			ELEVATION,
			AZIMUTH,
			SUNRISE,
			SOLAR_NOON,
			SUNSET,
	    	DAY_LENGTH,
			VIEWPORT
		}
		
		// inputs whose updates force a complete re-rendering
		private static StaticList<Id> ins_render = new StaticList<Id>(new Id[] {
			Id.LATITUDE_DEGS, Id.LATITUDE_MINS, Id.LATITUDE_SECS, Id.LATITUDE_DIRECTION,
			Id.LONGITUDE_DEGS, Id.LONGITUDE_MINS, Id.LONGITUDE_SECS, Id.LONGITUDE_DIRECTION,
			Id.TIMEZONE_OFFSET
		});
		
		// inputs whose updates trigger a plot update without re-rendering
		private static StaticList<Id> ins_update = new StaticList<Id>(new Id[] {
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
		private static Dictionary<Id, Control> registry =
			new Dictionary<Id, Control>();
		// the reverse registry is the opposite mapping for lookup on widget objects
		private static Dictionary<Control, Id> reg_rev =
			new Dictionary<Control, Id>();
		
		private static Dictionary<Id, string> cache =
			new Dictionary<Id, string>();
	
		
		public static void RegisterControl(Id id, Control control)
		{
			// register in registry
			registry.Add(id, control);
			reg_rev.Add(control, id);
			cache.Add(id, String.Empty);
			
			// activate buttons
			if (control is Button) {
				ActivateButton((Button) control);
			}
									
			// select on focus
			control.GotFocus += new EventHandler(GotFocus);
			control.LostFocus += new EventHandler(LostFocus);
			
			// validate all inputs before dispatching handler acting on new value
			if (ins_render.Contains(id) || ins_update.Contains(id)) {
				EventHandler handler = new EventHandler(Validate);
				RegisterValueChange(control, handler);
			}
	
			// register re-rendering inputs for value changes
			if (ins_render.Contains(id)) {
				EventHandler handler = new EventHandler(ValueChange);
				RegisterValueChange(control, handler);
			}
			
			// register updating inputs for value changes
			if (ins_update.Contains(id)) {
				EventHandler handler = new EventHandler(UpdateViewport);
				RegisterValueChange(control, handler);
			}
		}
		
		private static void ActivateButton(Button button)
		{
			if (reg_rev[button] == Id.CLEARFORM_ACTION) {
				button.Click += new EventHandler(ClearForm);
			}	else if (reg_rev[button] == Id.RENDER_ACTION) {
				button.Click += new EventHandler(RenderViewport);
			}
		}
			
		private static void RegisterValueChange(Control control, EventHandler handler)
		{
			if (control is ComboBox) {
				((ComboBox) control).SelectedValueChanged += handler;
			} else if (control is HScrollBar) {
				((HScrollBar) control).ValueChanged += handler;
			} else if (control is NumericUpDown) {
				((NumericUpDown) control).ValueChanged += handler;
			} else if (control is TextBox) {
				((TextBox) control).TextChanged += handler;
			}
		}
	
		// **************************************************************
	
		private static List<string> Save()
		{
			List<string> dict_s = new List<string> ();
			foreach (KeyValuePair<Id, Control> pair in registry)
			{
				dict_s.Add(pair.Key.ToString());
				dict_s.Add(pair.Value.Text);
			}
			return dict_s;
		}
	/*
		public static void Load(Dictionary<string, string> dict_s)
		{
			foreach (KeyValuePair<string, Control> pair in registry)
			{
				if (dict_s.ContainsKey(pair.Key))
				{
					registry[pair.Key].Text = dict_s[pair.Key];
				}
			}
		}
	*/
		private static void Serialize()
		{
			List<string> dict_s = Save();
			
			XmlSerializer ser = new XmlSerializer(dict_s.GetType());
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(sb);
			ser.Serialize(writer, dict_s);
	//		XmlDocument doc = new XmlDocument();
	//		doc.LoadXml(sb.ToString());
			string s = sb.ToString();
			Console.WriteLine(s);
	/*		
	//		XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
	//		XmlSerializer ser = new XmlSerializer(objType);
			object obj = ser.Deserialize(new StringReader(s));
			// Then you just need to cast obj into whatever type it is eg:
			List<string> dict_s_restored = (List<string>)obj;
			
			foreach (string sr in dict_s_restored)
			{
				Console.WriteLine(sr);
			}
	*/	}
	}	
}