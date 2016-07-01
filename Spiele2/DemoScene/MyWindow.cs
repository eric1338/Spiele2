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
	class MyWindow : GameWindow
	{

		private InputManager inputManager = new InputManager();

		private MainVisual visual;

		public MyWindow() : base(1280, 800)
		{
			Title = "Spiele2 DemoScene";

			MouseMove += MyWindow_MouseMove;
			MouseWheel += MyWindow_MouseWheel;
			KeyUp += MyWindow_KeyUp;
			KeyDown += MyWindow_KeyDown;

			UpdateFrame += MyWindow_UpdateFrame;
			RenderFrame += MyWindow_RenderFrame;

			Resize += (s, arg) => GL.Viewport(0, 0, Width, Height);

			inputManager.AddProlongedUserActionMapping(Key.W, UserAction.MoveForwards);
			inputManager.AddProlongedUserActionMapping(Key.S, UserAction.MoveBackwards);
			inputManager.AddProlongedUserActionMapping(Key.A, UserAction.MoveLeft);
			inputManager.AddProlongedUserActionMapping(Key.D, UserAction.MoveRight);

			inputManager.AddProlongedUserActionMapping(Key.M, UserAction.MoveSunMoon);

			visual = new MainVisual();
		}

		private void MyWindow_MouseMove(object sender, MouseMoveEventArgs e)
		{
			if (ButtonState.Pressed == e.Mouse.LeftButton)
			{
				visual.OrbitCamera.Heading += 10 * e.XDelta / (float) Width;
				visual.OrbitCamera.Tilt += 10 * e.YDelta / (float) Height;
			}
		}

		private void MyWindow_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			visual.OrbitCamera.Distance -= e.DeltaPrecise;
		}

		private void MyWindow_KeyUp(object sender, KeyboardKeyEventArgs e)
		{
			inputManager.ProcessKeyUp(e.Key);
		}

		private void MyWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			inputManager.ProcessKeyDown(e.Key);
		}

		private void MyWindow_UpdateFrame(object sender, FrameEventArgs e)
		{
			if (inputManager.IsUserActionActive(UserAction.MoveSunMoon))
			{
				visual.SunMoon.IncreaseAngle();
			}
		}

		private void MyWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			visual.Render();
			SwapBuffers();
		}
	}
}
