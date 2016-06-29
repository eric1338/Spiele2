using System;
using System.Text;

namespace ShaderDebugging
{
	//todo1: remove this class
	public static class ShaderLogParser
	{
		//todo: ErrorsFirstMessage should later return parsed message
		public static string ErrorsFirstMessage(string message)
		{
			char[] delimiters = new char[] { '\r', '\n' };
			string[] lines = message.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
			StringBuilder outputWarnings = new StringBuilder();
			StringBuilder outputErrors = new StringBuilder();
			foreach (var line in lines)
			{
				var trimmedLine = line.Trim();
				if (trimmedLine.ToUpper().StartsWith("ERROR"))
				{
					outputErrors.AppendLine(trimmedLine);
				}
				else
				{
					outputWarnings.AppendLine(trimmedLine);
				}
			}
			outputErrors.AppendLine();
			outputErrors.Append(outputWarnings);
			return outputErrors.ToString();
		}

	}
}
