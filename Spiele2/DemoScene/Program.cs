using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene
{
	class Program
	{
		
		private MyWindow myWindow;

		[STAThread]
		public static void Main()
		{
			var app = new Program();
			app.myWindow.Run();
		}

		private Program()
		{
			int width = 1366;
			int height = 768;

			myWindow = new MyWindow(width, height);
		}

		/*
		private GameWindow gameWindow;
		private MainVisual visual;

		[STAThread]
		public static void Main()
		{
			var app = new Program();
			app.gameWindow.Run();
		}

		private Program()
		{
			gameWindow = new GameWindow(800, 800);
			//gameWindow.WindowState = WindowState.Fullscreen;
			gameWindow.MouseMove += GameWindow_MouseMove;
			gameWindow.MouseWheel += GameWindow_MouseWheel;
			gameWindow.KeyDown += GameWindow_KeyDown;
			gameWindow.Resize += (s, arg) => GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			gameWindow.RenderFrame += (s, arg) => visual.Render();
			gameWindow.RenderFrame += (s, arg) => gameWindow.SwapBuffers();
			visual = new MainVisual();
		}

		private void GameWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			if (e.Key == Key.Escape) gameWindow.Close();
		}

		private void GameWindow_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			visual.OrbitCamera.Distance -= e.DeltaPrecise;
		}

		private void GameWindow_MouseMove(object sender, MouseMoveEventArgs e)
		{
			if (ButtonState.Pressed == e.Mouse.LeftButton)
			{
				visual.OrbitCamera.Heading += 10 * e.XDelta / (float)gameWindow.Width;
				visual.OrbitCamera.Tilt += 10 * e.YDelta / (float)gameWindow.Height;
			}
		}
		*/

	}

}
