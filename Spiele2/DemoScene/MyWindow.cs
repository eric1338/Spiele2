using DemoScene.DemoObjects;
using DemoScene.Logic;
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
		private Physics physics;

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
			inputManager.AddSingleUserActionMapping(Key.G, UserAction.DecreaseGravity);
			inputManager.AddSingleUserActionMapping(Key.H, UserAction.IncreaseGravity);
			inputManager.AddSingleUserActionMapping(Key.Y, UserAction.DecreaseWindForce);
			inputManager.AddSingleUserActionMapping(Key.U, UserAction.IncreaseWindForce);
			inputManager.AddSingleUserActionMapping(Key.B, UserAction.ToggleBounce);

			Textures.Instance.LoadTextures();

			visual = new MainVisual(demoLevel);
			physics = new Physics(demoLevel);
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

		int count = -120;

		float rand1 = 0;

		private void MyWindow_UpdateFrame(object sender, FrameEventArgs e)
		{
			ProcessInput();
			DoPhysics();
		}

		private void MyWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			visual.Render();
			SwapBuffers();
		}

		private void ProcessInput()
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
				if (userAction == UserAction.IncreaseGravity) demoLevel.IncreaseGravity();
				if (userAction == UserAction.DecreaseGravity) demoLevel.DecreaseGravity();
				if (userAction == UserAction.IncreaseWindForce) demoLevel.IncreaseWindForce();
				if (userAction == UserAction.DecreaseWindForce) demoLevel.DecreaseWindForce();
				if (userAction == UserAction.ToggleBounce) ToggleBounce();
			}
		}

		private void ToggleBounce()
		{
			demoLevel.Player.Bounce = !demoLevel.Player.Bounce;
			demoLevel.Rabbit.Bounce = !demoLevel.Rabbit.Bounce;
		}

		private void DoPhysics()
		{
			Rabbit rabbit = demoLevel.Rabbit;

			count++;

			if (count < 0) return;

			if (count % 400 == 0)
			{
				rand1 = (float)(new Random()).NextDouble() * 0.02f;
			}
			if (count % 400 < 100)
			{
				rabbit.Rotation += rand1;
			}
			if (count % 400 == 100)
			{
				physics.LetRabbitJump(rabbit.GetJumpDirection());
			}

			physics.DoPhysics();
		}
	}
}
