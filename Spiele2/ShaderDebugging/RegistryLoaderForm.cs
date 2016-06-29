using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace ShaderDebugging
{
	public static class RegistryLoaderForm
	{
		public static RegistryKey GetAppKey()
		{
			return Application.UserAppDataRegistry;
		}

		public static void LoadLayout(this Form form)
		{
			RegistryKey keyApp = GetAppKey();
			if (null == keyApp) return;
			var key = keyApp.CreateSubKey(form.Name);
			if (null == key) return;
			form.WindowState = (FormWindowState)Convert.ToInt32(key.GetValue("WindowState", (int)form.WindowState));
			form.Visible = Convert.ToBoolean(key.GetValue("visible", form.Visible));
			form.Width = Convert.ToInt32(key.GetValue("Width", form.Width));
			form.Height = Convert.ToInt32(key.GetValue("Height", form.Height));
			form.Top = Convert.ToInt32(key.GetValue("Top", form.Top));
			form.Left = Convert.ToInt32(key.GetValue("Left", form.Left));
		}

		public static void SaveLayout(this Form form)
		{
			RegistryKey keyApp = GetAppKey();
			if (null == keyApp) return;
			var key = keyApp.CreateSubKey(form.Name);
			if (null == key) return;
			key.SetValue("WindowState", (int)form.WindowState);
			key.SetValue("visible", form.Visible);
			key.SetValue("Width", form.Width);
			key.SetValue("Height", form.Height);
			key.SetValue("Top", form.Top);
			key.SetValue("Left", form.Left);
		}
	}
}
