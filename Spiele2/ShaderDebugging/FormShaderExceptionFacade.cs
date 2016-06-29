using Framework;
using System;
using System.Drawing;

namespace ShaderDebugging
{
	public class FormShaderExceptionFacade
	{
		public event EventHandler<EventArgs> OnSave;

		public FormShaderExceptionFacade()
		{
			form = new FormShaderException();
			form.OnSave += (s, a) => OnSave?.Invoke(s, a);
		}

		public void Clear()
		{
			lastException = null;
			form.richTextBox.Clear();
			form.Errors.Clear();
		}

		public void Hide()
		{
			form.Hide();
		}

		public void Show(ShaderException e)
		{
			if (null == lastException || e.Log != lastException.Log)
			{
				Clear(); //clears last log too -> need to store lastLog afterwards
				lastException = e;
				FillData(e);
			}
			form.Show();
			//todo: bring to front
			form.TopMost = true;
			form.TopMost = false;
		}

		public static void ShowModal(ShaderException e)
		{
			var facade = new FormShaderExceptionFacade();
			//facade.OnSave += (s, a) => facade.
			facade.Clear();
			facade.FillData(e);
			facade.form.ShowDialog();
		}

		private readonly FormShaderException form;
		private ShaderException lastException;

		private void FillData(ShaderException e)
		{
			var rtf = form.richTextBox;
			var font = rtf.Font;
			var errorFont = new Font(font, FontStyle.Strikeout);
			char[] newline = new char[] { '\n' };
			var sourceLines = e.ShaderCode.Split(newline);
			foreach (var sourceLine in sourceLines)
			{
				rtf.AppendText(sourceLine);
			}

			var log = new ShaderLog(e.Log);
			foreach (var logLine in log.Lines)
			{
				form.Errors.Add(logLine);
			}
		}
	}
}
