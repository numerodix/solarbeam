// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.ComponentModel;

namespace SolarbeamGui
{
	enum Result {
		OK,
		Fail,
	}
	
	class Message
	{
		private Result result;
		private DateTime date;
		private string message;
		
		public Message(Result result, string message)
		{
			this.result = result;
			this.date = DateTime.Now;
			this.message = message;
		}
		
		public override string ToString()
		{
			string fmt = "HH':'mm':'ss";
			string date_s = date.ToString(fmt);
			return string.Format("{0} - {1}", date_s, message);
		}
		
		public Result Result
		{ get { return result; } }
	}
}
