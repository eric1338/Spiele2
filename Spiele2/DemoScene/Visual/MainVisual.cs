using DemoScene.DemoObjects;
using Framework;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using ShaderDebugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.Visual
{
	class MainVisual
	{

		public FirstPersonCamera FirstPersonCamera { get; set; }

		public CameraOrbit OrbitCamera { get { return camera; } }

		private CameraOrbit camera = new CameraOrbit();

		private Shader defaultShader;
		private Shader colorShader;
		private Shader cellShader;
		private Shader cellAndToonShader;
		private Shader flagShader;
		private Shader skyboxShader;

		private DemoLevel demoLevel;
		private Models models;

		public bool PassTime { get; set; }

		private float time = 0;

		private Vector3 LightningBugPosition;

		public MainVisual(DemoLevel demoLevel)
		{
			this.demoLevel = demoLevel;

			FirstPersonCamera = new FirstPersonCamera(demoLevel.Player);
			
			camera.FarClip = 2000;
			camera.Distance = 5;
			camera.FovY = 50;

			defaultShader = CreateShader(Resources.vertex, Resources.fragment);
			colorShader = CreateShader(Resources.vertex, Resources.colorfragment);
			cellShader = CreateShader(Resources.vertex, Resources.cellfragment);
			cellAndToonShader = CreateShader(Resources.vertex, Resources.celltoonfragment);
			flagShader = CreateShader(Resources.flagvertex, Resources.fragment);
			skyboxShader = CreateShader(Resources.vertex, Resources.simplefragment);

			models = new Models();

			models.CreateFigurines(defaultShader);
			models.CreateBalls(colorShader, cellShader, cellAndToonShader);

			models.CreateRabbit(defaultShader);
			models.CreateFlag(flagShader);

			models.CreateSkyboxes(skyboxShader);
			models.CreateGround(defaultShader);

			PassTime = true;

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
		}

		private Shader CreateShader(byte[] vertexShader, byte[] fragmentShader)
		{
			string vertexShaderString = Encoding.UTF8.GetString(vertexShader);
			string fragmentShaderString = Encoding.UTF8.GetString(fragmentShader);

			return ShaderLoader.FromStrings(vertexShaderString, fragmentShaderString);
		}

		public void RotateFigurines()
		{
			foreach (Model model in models.Figurines)
			{
				model.RenderSettings.Rotation += 0.02f;
			}
		}

		private Matrix4 GetCurrentCameraMatrix()
		{
			return FirstPersonCamera.GetMatrix();

			//return camera.CalcMatrix();
		}

		float lbx = -6f;

		public void Render()
		{
			if (PassTime) time += 0.02f;

			lbx += 0.02f;

			if (lbx > 6) lbx = -6;

			LightningBugPosition = new Vector3(lbx, 2, 1);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			Matrix4 cam = GetCurrentCameraMatrix();

			RenderDefault(cam);
			RenderBalls(cam);
			RenderFlag(cam);
			RenderSkybox(cam);
		}

		private void RenderDefault(Matrix4 camera)
		{
			defaultShader.Begin();

			SetDefaultVertexUniforms(defaultShader, camera);

			SetSunMoonUniforms(defaultShader);

			GL.Uniform3(defaultShader.GetUniformLocation("lightningBugPosition"), LightningBugPosition);

			foreach (Model figurine in models.Figurines)
			{
				RenderModel(defaultShader, figurine);
			}

			foreach (Model groundModel in models.Ground)
			{
				RenderModel(defaultShader, groundModel);
			}

			RenderSettings rabbitRenderSettings = models.Rabbit.RenderSettings;
			Rabbit rabbit = demoLevel.Rabbit;

			rabbitRenderSettings.Position = rabbit.Position;
			rabbitRenderSettings.Rotation = rabbit.Rotation;

			RenderModel(defaultShader, models.Rabbit);

			defaultShader.End();
		}

		private void RenderBalls(Matrix4 camera)
		{
			Vector3 offsetVector = new Vector3(0, 0.5f, 0);

			models.DefaultBall.RenderSettings.Position = demoLevel.DefaultBall.Position + offsetVector;
			models.CellShadingBall.RenderSettings.Position = demoLevel.CellShadingBall.Position + offsetVector;
			models.CellAndToonShadingBall.RenderSettings.Position = demoLevel.CellAndToonShadingBall.Position + offsetVector;

			colorShader.Begin();

			SetDefaultVertexUniforms(colorShader, camera);
			SetSunMoonUniforms(colorShader);

			RenderModel(colorShader, models.DefaultBall);
			colorShader.End();


			cellShader.Begin();

			SetDefaultVertexUniforms(cellShader, camera);
			SetSunMoonUniforms(cellShader);

			RenderModel(cellShader, models.CellShadingBall);
			cellShader.End();


			cellAndToonShader.Begin();

			SetDefaultVertexUniforms(cellAndToonShader, camera);
			SetSunMoonUniforms(cellAndToonShader);

			RenderModel(cellAndToonShader, models.CellAndToonShadingBall);
			cellAndToonShader.End();
		}

		private void RenderSkybox(Matrix4 camera)
		{
			skyboxShader.Begin();

			SetDefaultVertexUniforms(skyboxShader, camera);

			GL.Uniform1(skyboxShader.GetUniformLocation("brightness"), demoLevel.SunMoon.GetIntensity());

			List<Model> skybox = demoLevel.SunMoon.IsDay() ? models.DaySkybox : models.NightSkybox;

			foreach (Model skyboxModel in skybox)
			{
				RenderModel(skyboxShader, skyboxModel);
			}

			skyboxShader.End();
		}

		private void RenderFlag(Matrix4 camera)
		{
			flagShader.Begin();

			VAO vao = models.Flag.Vao;
			RenderSettings renderSettings = models.Flag.RenderSettings;
			Texture flagTexture = models.Flag.RenderSettings.DiffuseTexture;

			SetDefaultVertexUniforms(flagShader, camera);
			SetSunMoonUniforms(flagShader);

			GL.Uniform1(flagShader.GetUniformLocation("diffuseTexture"), renderSettings.DiffuseTexture.ID);

			GL.Uniform3(flagShader.GetUniformLocation("lightningBugPosition"), LightningBugPosition);

			GL.Uniform3(flagShader.GetUniformLocation("instancePosition"), new Vector3(8, 3, 6));
			GL.Uniform1(flagShader.GetUniformLocation("instanceScale"), 0.15f);
			
			Vector3 windDirection = demoLevel.WindDirection;

			GL.Uniform3(flagShader.GetUniformLocation("polePosition"), new Vector3(8, 3, 6));
			GL.Uniform3(flagShader.GetUniformLocation("windDirection"), demoLevel.WindDirection);

			float waveSpeed;
			float waveAmplitude;

			if (demoLevel.WindForceLevel == 0)
			{
				waveSpeed = 1f;
				waveAmplitude = 0.2f;
			}
			else if (demoLevel.WindForceLevel == 1)
			{
				waveSpeed = 2f;
				waveAmplitude = 0.3f;
			}
			else if (demoLevel.WindForceLevel == 2)
			{
				waveSpeed = 3f;
				waveAmplitude = 0.4f;
			}
			else
			{
				waveSpeed = 8f;
				waveAmplitude = 0.6f;
			}

			GL.Uniform1(flagShader.GetUniformLocation("waveSpeed"), waveSpeed);
			GL.Uniform1(flagShader.GetUniformLocation("waveAmplitude"), waveAmplitude);

			renderSettings.DiffuseTexture.BeginUse();
			vao.Draw();
			renderSettings.DiffuseTexture.EndUse();

			flagShader.End();
		}

		private void SetDefaultVertexUniforms(Shader shader, Matrix4 cam)
		{
			GL.Uniform1(shader.GetUniformLocation("time"), time);
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref cam);
			GL.Uniform3(shader.GetUniformLocation("cameraPosition"), camera.CalcPosition());
		}

		private void SetSunMoonUniforms(Shader shader)
		{
			GL.Uniform3(shader.GetUniformLocation("lightDirection"), demoLevel.SunMoon.GetLightDirection());
			GL.Uniform3(shader.GetUniformLocation("lightColor"), demoLevel.SunMoon.GetLightColor());
			GL.Uniform1(shader.GetUniformLocation("ambientFactor"), demoLevel.SunMoon.GetAmbientFactor());
		}

		private void RenderModel(Shader shader, Model model)
		{
			RenderSettings renderSettings = model.RenderSettings;

			Matrix3 rotation = Matrix3.CreateRotationY(renderSettings.Rotation);
			GL.UniformMatrix3(shader.GetUniformLocation("instanceRotation"), false, ref rotation);

			GL.Uniform3(shader.GetUniformLocation("instancePosition"), renderSettings.Position);
			GL.Uniform1(shader.GetUniformLocation("instanceScale"), renderSettings.Scale);

			if (renderSettings.UseTexture)
			{
				Texture texture = renderSettings.DiffuseTexture;

				GL.Uniform1(shader.GetUniformLocation("diffuseTexture"), texture.ID);

				texture.BeginUse();
				model.Vao.Draw();
				texture.EndUse();
			}
			else
			{
				GL.Uniform3(shader.GetUniformLocation("materialColor"), renderSettings.Color);

				model.Vao.Draw();
			}

		}
		
	}
}
