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
			
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

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

			inputManager.AddSingleUserActionMapping(Key.Space, UserAction.Jump);
			inputManager.AddSingleUserActionMapping(Key.F, UserAction.ToggleFly);

			inputManager.AddSingleUserActionMapping(Key.L, UserAction.ResetDemoLevel);
			inputManager.AddSingleUserActionMapping(Key.P, UserAction.TogglePause);
			inputManager.AddSingleUserActionMapping(Key.B, UserAction.ToggleBounce);
			inputManager.AddSingleUserActionMapping(Key.G, UserAction.DecreaseGravity);
			inputManager.AddSingleUserActionMapping(Key.H, UserAction.IncreaseGravity);
			inputManager.AddSingleUserActionMapping(Key.Y, UserAction.DecreaseWindForce);
			inputManager.AddSingleUserActionMapping(Key.U, UserAction.IncreaseWindForce);

			inputManager.AddSingleUserActionMapping(Key.Number1, UserAction.ToggleDefaultVisual);
			inputManager.AddSingleUserActionMapping(Key.Number2, UserAction.ToggleFigurinesVisual);
			inputManager.AddSingleUserActionMapping(Key.Number3, UserAction.ToggleEffectFigurinesVisual);
			inputManager.AddSingleUserActionMapping(Key.Number4, UserAction.ToggleBallsVisual);
			inputManager.AddSingleUserActionMapping(Key.Number5, UserAction.ToggleFlagVisual);
			inputManager.AddSingleUserActionMapping(Key.Number6, UserAction.ToggleTetrahedronSphereVisual);
			inputManager.AddSingleUserActionMapping(Key.Number7, UserAction.ToggleParticleSystemVisual);

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

				//visual.OrbitCamera.Heading += h;
				//visual.OrbitCamera.Tilt += v;

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
			ProcessInput();

			DoDemoLevelLogic();
		}

		private void DoDemoLevelLogic()
		{
			if (demoLevel.IsRunning)
			{
				demoLevel.TetrahedronSphere.Tick(demoLevel.Player.Position);
				demoLevel.ParticleSystem.Update();
			}
			physics.DoPhysics();
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

			Vector3 playerMoveDirection = Vector3.Zero;

			FirstPersonCamera camera = visual.FirstPersonCamera;

			if (inputManager.IsUserActionActive(UserAction.MoveForwards)) playerMoveDirection += camera.GetForwardsVector();
			if (inputManager.IsUserActionActive(UserAction.MoveBackwards)) playerMoveDirection += camera.GetBackwardsVector();
			if (inputManager.IsUserActionActive(UserAction.MoveLeft)) playerMoveDirection += camera.GetLeftVector();
			if (inputManager.IsUserActionActive(UserAction.MoveRight)) playerMoveDirection += camera.GetRightVector();

			bool jump = false;

			List<UserAction> singleUserActions = inputManager.GetSingleUserActionsAsList();

			foreach (UserAction userAction in singleUserActions)
			{
				if (userAction == UserAction.ResetDemoLevel) demoLevel.SetInitialValues();
				if (userAction == UserAction.Jump) jump = true;
				if (userAction == UserAction.ToggleFly) demoLevel.Player.ToggleFlying();
				if (userAction == UserAction.TogglePause) TogglePause();
				if (userAction == UserAction.ToggleBounce) ToggleBounce();
				if (userAction == UserAction.IncreaseGravity) demoLevel.IncreaseGravity();
				if (userAction == UserAction.DecreaseGravity) demoLevel.DecreaseGravity();
				if (userAction == UserAction.IncreaseWindForce) demoLevel.IncreaseWindForce();
				if (userAction == UserAction.DecreaseWindForce) demoLevel.DecreaseWindForce();
				if (userAction == UserAction.ToggleDefaultVisual) visual.defaultVisual.ToggleDoRender();
				if (userAction == UserAction.ToggleFigurinesVisual) visual.figurinesVisual.ToggleDoRender();
				if (userAction == UserAction.ToggleEffectFigurinesVisual) visual.effectFigurinesVisual.ToggleDoRender();
				if (userAction == UserAction.ToggleBallsVisual) visual.ballsVisual.ToggleDoRender();
				if (userAction == UserAction.ToggleFlagVisual) visual.flagVisual.ToggleDoRender();
				if (userAction == UserAction.ToggleTetrahedronSphereVisual) visual.tetrahedronSphereVisual.ToggleDoRender();
				if (userAction == UserAction.ToggleParticleSystemVisual) visual.particleSystemVisual.ToggleDoRender();
			}

			Player player = demoLevel.Player;

			if (jump)
			{
				if (!player.IsFlying)
				{
					Vector3 jumpDirection = new Vector3(playerMoveDirection.X, 0, playerMoveDirection.Z);

					if (jumpDirection == Vector3.Zero)
					{
						jumpDirection = new Vector3(0, 1, 0);
					}
					else
					{
						jumpDirection.Normalize();
						jumpDirection *= 0.6f;
						jumpDirection.Y = 0.85f;
					}

					physics.LetPlayerJump(jumpDirection);
				}
			}
			else if (player.IsFlying || player.Position.Y == 0)
			{
				player.Move(playerMoveDirection);
			}
		}

		private void TogglePause()
		{
			demoLevel.IsRunning = !demoLevel.IsRunning;

			visual.PassTime = demoLevel.IsRunning;
			physics.DoNonPlayerPhysics = demoLevel.IsRunning;
		}

		private void ToggleBounce()
		{
			demoLevel.Player.Bounce = !demoLevel.Player.Bounce;
			demoLevel.Rabbit.Bounce = !demoLevel.Rabbit.Bounce;
		}
	}
}
