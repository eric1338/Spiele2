using DemoScene.DemoObjects;
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

		private DemoLevel demoLevel = new DemoLevel();

		private MainVisual visual;

		public MyWindow(int width, int height) : base(width, height)
		{
			Title = "Spiele2 DemoScene";

			float aspect = width / (float)height;

			GL.Viewport(0, 0, width, height);
			GL.MatrixMode(MatrixMode.Projection);
			GL.Ortho(-aspect, aspect, -1, 1, -1, 1);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

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
			inputManager.AddProlongedUserActionMapping(Key.T, UserAction.RotateWind);

			inputManager.AddSingleUserActionMapping(Key.F, UserAction.ToggleFly);
			inputManager.AddSingleUserActionMapping(Key.Z, UserAction.DecreaseWindForce);
			inputManager.AddSingleUserActionMapping(Key.U, UserAction.IncreaseWindForce);

			Textures.Instance.LoadTextures();

			visual = new MainVisual(demoLevel);
		}

		private void MyWindow_MouseMove(object sender, MouseMoveEventArgs e)
		{
			if (ButtonState.Pressed == e.Mouse.RightButton)
			{
				float h = 10 * e.XDelta / (float) Width;
				float v = 10 * e.YDelta / (float) Height;

				visual.OrbitCamera.Heading += h;
				visual.OrbitCamera.Tilt += v;

				visual.FirstPersonCamera.ChangeTarget(h, v);
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
			if (inputManager.IsUserActionActive(UserAction.MoveSunMoon)) demoLevel.SunMoon.IncreaseAngle();
			if (inputManager.IsUserActionActive(UserAction.RotateModels)) visual.RotateFigurines();
			if (inputManager.IsUserActionActive(UserAction.RotateWind)) demoLevel.RotateWind();

			if (inputManager.IsUserActionActive(UserAction.MoveForwards)) visual.FirstPersonCamera.MoveForwards();
			if (inputManager.IsUserActionActive(UserAction.MoveBackwards)) visual.FirstPersonCamera.MoveBackwards();
			if (inputManager.IsUserActionActive(UserAction.MoveLeft)) visual.FirstPersonCamera.MoveLeft();
			if (inputManager.IsUserActionActive(UserAction.MoveRight)) visual.FirstPersonCamera.MoveRight();

			List<UserAction> singleUserActions = inputManager.GetSingleUserActionsAsList();

			foreach (UserAction userAction in singleUserActions)
			{
				if (userAction == UserAction.ToggleFly) visual.FirstPersonCamera.ToggleFixY();
				if (userAction == UserAction.IncreaseWindForce) demoLevel.IncreaseWindForce();
				if (userAction == UserAction.DecreaseWindForce) demoLevel.DecreaseWindForce();

			}
		}

		private void MyWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			visual.Render();
			SwapBuffers();
		}
	}
}
