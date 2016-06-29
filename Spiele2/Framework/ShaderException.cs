﻿using System;

namespace Framework
{
	public class ShaderException : Exception
	{
		public string Type { get; private set; }
		public string ShaderCode { get; private set; }
		public string Log { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ShaderException"/> class.
		/// </summary>
		/// <param name="msg">The error msg.</param>
		public ShaderException(string type, string msg, string log, string shaderCode) : base(msg)
		{
			Type = type;
			Log = log;
			ShaderCode = shaderCode;
		}
	}
}
