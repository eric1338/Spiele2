using System;
using System.Collections.Generic;

namespace ShaderDebugging
{
	public class ShaderLogLine
	{
		public string Type;
		public int FileNumber;
		public int LineNumber;
		public string Message;
		public override string ToString()
		{
			return "Line " + LineNumber + " : " + Message;
		}
	}

	public class ShaderLog
	{
		public ShaderLog(string log)
		{
			//parse error log
			char[] splitChars = new char[] { '\n' };
			foreach (var line in log.Split(splitChars, StringSplitOptions.RemoveEmptyEntries))
			{
				try
				{
					var logLine = ParseLogLineNVIDIA(line);
					lines.Add(logLine);
				}
				catch
				{
					var logLine = ParseLogLine(line);
					lines.Add(logLine);
				}
			}
		}

		public IList<ShaderLogLine> Lines { get { return lines; } }

		private ShaderLogLine ParseLogLine(string line)
		{
			ShaderLogLine logLine = new ShaderLogLine();
			char[] splitChars = new char[] { ':' };
			var elements = line.Split(splitChars, 4);
			switch(elements.Length)
			{
				case 4:
					logLine.Type = ParseType(elements[0]);
					logLine.FileNumber = Parse(elements[1]);
					logLine.LineNumber = Parse(elements[2]);
					logLine.Message = elements[3];
					break;
				case 3:
					logLine.Type = ParseType(elements[0]);
					logLine.Message = elements[1] + ":" + elements[2];
					break;
				case 2:
					logLine.Type = ParseType(elements[0]);
					logLine.Message = elements[1];
					break;
				case 1:
					logLine.Message = elements[0];
					break;
				default:
					throw new ArgumentException(line);
			}
			logLine.Message = logLine.Message.Trim();
			return logLine;
		}

		private ShaderLogLine ParseLogLineNVIDIA(string line)
		{
			ShaderLogLine logLine = new ShaderLogLine();
			char[] splitChars = new char[] { ':' };
			var elements = line.Split(splitChars, 3);
			switch (elements.Length)
			{
				case 3:
					logLine.FileNumber = ParseNVFileNumber(elements[0]);
					logLine.LineNumber = ParseNVLineNumber(elements[0]);
					logLine.Type = ParseNVType(elements[1]);
					logLine.Message = elements[1] + ":" + elements[2];
					break;
				default:
					throw new ArgumentException(line);
			}
			logLine.Message = logLine.Message.Trim();
			return logLine;
		}

		private string ParseNVType(string v)
		{
			char[] splitChars = new char[] { ' ', '\t' };
			var elements = v.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
			return ParseType(elements[0]);
		}

		private int ParseNVLineNumber(string v)
		{
			char[] splitChars = new char[] { '(',')', ' ', '\t' };
			var elements = v.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
			return Parse(elements[1]);
		}

		private int ParseNVFileNumber(string v)
		{
			char[] splitChars = new char[] { '(', ')', ' ', '\t' };
			var elements = v.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
			return Parse(elements[0]);
		}

		private string ParseType(string typeString)
		{
			return typeString.ToUpperInvariant().Trim();
		}

		private int Parse(string number)
		{
			int output;
			if (int.TryParse(number, out output))
			{
				return output;
			}
			else
			{
				return -1;
			}
		}

		private List<ShaderLogLine> lines = new List<ShaderLogLine>();
	}
}
