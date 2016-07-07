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

		public LookAtCamera Looki { get; set; }

		public CameraOrbit OrbitCamera { get { return camera; } }

		private CameraOrbit camera = new CameraOrbit();

		private Shader defaultShader;
		private Shader flagShader;
		private Shader skyboxShader;

		public SunMoon SunMoon { get; set; }

		private Models models;

		private float time = 0;

		public MainVisual()
		{
			Looki = new LookAtCamera();
			SunMoon = new SunMoon();
			
			camera.FarClip = 2000;
			camera.Distance = 5;
			camera.FovY = 50;

			defaultShader = CreateShader(Resources.vertex, Resources.fragment);
			flagShader = CreateShader(Resources.flagvertex, Resources.fragment);
			skyboxShader = CreateShader(Resources.vertex, Resources.simplefragment);

			models = new Models();

			models.CreateFigurines(defaultShader);
			models.CreateFlag(flagShader);
			models.CreateSkyboxes(defaultShader);
			models.CreateGround(defaultShader);

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

		public void Render()
		{
			time += 0.02f;

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			Matrix4 cam = camera.CalcMatrix();

			Vector3 eye = new Vector3(0, 2, 10);
			Vector3 target = new Vector3(0, 2, 0);
			Vector3 up = new Vector3(0, 1, 0);
			//Matrix4 cam = Matrix4.LookAt(eye, target, up);

			//cam.Transpose();

			RenderDefault(cam);
			RenderFlag(cam);
		}

		private void RenderDefault(Matrix4 camera)
		{
			defaultShader.Begin();

			GL.Uniform1(defaultShader.GetUniformLocation("time"), time);

			GL.UniformMatrix4(defaultShader.GetUniformLocation("camera"), true, ref camera);
			GL.Uniform3(defaultShader.GetUniformLocation("cameraPosition"), this.camera.CalcPosition());

			GL.Uniform3(defaultShader.GetUniformLocation("lightDirection"), SunMoon.GetLightDirection());
			GL.Uniform3(defaultShader.GetUniformLocation("lightColor"), SunMoon.GetLightColor());

			foreach (Model figurine in models.Figurines)
			{
				RenderModel(defaultShader, figurine);
			}

			List<Model> skybox = SunMoon.IsDay() ? models.DaySkybox : models.NightSkybox;

			foreach (Model skyboxModel in skybox)
			{
				RenderModel(defaultShader, skyboxModel);
			}

			foreach (Model groundModel in models.Ground)
			{
				RenderModel(defaultShader, groundModel);
			}

			defaultShader.End();
		}

		private void RenderFlag(Matrix4 camera)
		{
			flagShader.Begin();

			VAO vao = models.Flag.Vao;
			RenderSettings renderSettings = models.Flag.RenderSettings;
			Texture flagTexture = models.Flag.RenderSettings.DiffuseTexture;

			GL.Uniform1(flagShader.GetUniformLocation("time"), time);
			GL.UniformMatrix4(flagShader.GetUniformLocation("camera"), true, ref camera);
			GL.Uniform3(flagShader.GetUniformLocation("cameraPosition"), this.camera.CalcPosition());

			GL.Uniform1(flagShader.GetUniformLocation("diffuseTexture"), renderSettings.DiffuseTexture.ID);

			GL.Uniform3(flagShader.GetUniformLocation("lightDirection"), SunMoon.GetLightDirection());
			GL.Uniform3(flagShader.GetUniformLocation("lightColor"), SunMoon.GetLightColor());

			GL.Uniform3(flagShader.GetUniformLocation("instancePosition"), new Vector3(4, 2, 3));
			GL.Uniform1(flagShader.GetUniformLocation("instanceScale"), 0.15f);

			Vector3 wind = new Vector3(1.0f * (float) Math.Sin(0), 0, -1.0f * (float) Math.Cos(0));

			GL.Uniform3(flagShader.GetUniformLocation("polePosition"), new Vector3(4, 2, 3));
			GL.Uniform3(flagShader.GetUniformLocation("windDirection"), wind);
			GL.Uniform1(flagShader.GetUniformLocation("waveSpeed"), 5f);
			GL.Uniform1(flagShader.GetUniformLocation("waveAmplitude"), 0.4f);

			// 5 0.4

			renderSettings.DiffuseTexture.BeginUse();
			vao.Draw();
			renderSettings.DiffuseTexture.EndUse();

			flagShader.End();
		}

		private void RenderModel(Shader shader, Model model)
		{
			RenderSettings renderSettings = model.RenderSettings;

			Texture texture = renderSettings.DiffuseTexture;

			Matrix3 rotation = Matrix3.CreateRotationY(renderSettings.Rotation);
			GL.UniformMatrix3(shader.GetUniformLocation("instanceRotation"), false, ref rotation);

			GL.Uniform1(shader.GetUniformLocation("diffuseTexture"), texture.ID);

			GL.Uniform3(shader.GetUniformLocation("instancePosition"), renderSettings.Position);
			GL.Uniform1(shader.GetUniformLocation("instanceScale"), renderSettings.Scale);

			texture.BeginUse();
			model.Vao.Draw();
			texture.EndUse();
		}
		
	}
}
