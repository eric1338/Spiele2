using DemoScene.Utils;
using DemoScene.Visual;
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

		public MyWindow() : base(800, 800)
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
			inputManager.AddProlongedUserActionMapping(Key.R, UserAction.RotateModels);

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

			if (inputManager.IsUserActionActive(UserAction.RotateModels))
			{
				visual.RotateModels();
			}

			if (inputManager.IsUserActionActive(UserAction.MoveForwards)) visual.Looki.MoveForwards();
			if (inputManager.IsUserActionActive(UserAction.MoveBackwards)) visual.Looki.MoveBackwards();
			if (inputManager.IsUserActionActive(UserAction.MoveLeft)) visual.Looki.MoveLeft();
			if (inputManager.IsUserActionActive(UserAction.MoveRight)) visual.Looki.MoveRight();
		}

		private void MyWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			visual.Render();
			SwapBuffers();
		}
	}
}
