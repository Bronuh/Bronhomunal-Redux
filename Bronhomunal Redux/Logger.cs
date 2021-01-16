﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Bronuh
{
	public static class Logger
	{

		public static List<LogMessage> GlobalLog = new List<LogMessage>();
		public static List<LogMessage> ErrorsLog = new List<LogMessage>();
		public static List<LogMessage> WarningsLog = new List<LogMessage>();
		public static List<LogMessage> SuccessesLog = new List<LogMessage>();
		public static List<LogMessage> MessagesLog = new List<LogMessage>();
		public static List<LogMessage> DebugsLog = new List<LogMessage>();

		public static bool DEBUG = true;


		public static void Log(String text)
		{
			new LogMessage(text);
		}

		public static void Error(String text)
		{
			new LogMessage(text, LogMessage.Type.Error);
		}

		public static void Warning(String text)
		{
			new LogMessage(text, LogMessage.Type.Warning);
		}

		public static void Success(String text)
		{
			new LogMessage(text, LogMessage.Type.Success);
		}

		public static void Debug(String text)
		{
			new LogMessage(text, LogMessage.Type.Debug);
		}
	}

	public class LogMessage
	{
		public String text;
		public Type type;
		public DateTime time;
		public string fullText;

		public enum Type
		{
			Error,
			Warning,
			Message,
			Success,
			Debug
		}

		public LogMessage(string text)
		{
			this.text = text;
			type = Type.Message;
			time = DateTime.Now;

			string _type = type == Type.Message ? "" : type.ToString().ToUpper();
			fullText = "[" + time.ToLongTimeString() + "] " + "(" + Thread.CurrentThread.Name + ") " + _type + ": " + text;
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(fullText);
			Logger.GlobalLog.Add(this);
			Logger.MessagesLog.Add(this);
		}

		public LogMessage(string text, Type type)
		{
			this.text = text;
			this.type = type;
			time = DateTime.Now;

			string _type = type == Type.Message ? "" : type.ToString().ToUpper();
			fullText = "[" + time.ToLongTimeString() + "] " + "(" + Thread.CurrentThread.Name + ") " + _type + ": " + text;

			Logger.GlobalLog.Add(this);

			if (type == Type.Message)
			{
				Console.ForegroundColor = ConsoleColor.White;
				Logger.MessagesLog.Add(this);
			}

			if (type == Type.Error)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Logger.ErrorsLog.Add(this);
			}

			if (type == Type.Warning)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Logger.WarningsLog.Add(this);
			}

			if (type == Type.Success)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Logger.SuccessesLog.Add(this);
			}

			if (type == Type.Debug)
			{
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Logger.DebugsLog.Add(this);
			}

			if (type == Type.Debug && !Logger.DEBUG)
			{
				return;
			}

			Console.WriteLine(fullText);
		}
	}
}