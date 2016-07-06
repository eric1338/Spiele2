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

		private List<Model> models = new List<Model>();

		private float modelAngle = 0.0f;

		private List<Tuple<VAO, Model>> geometries = new List<Tuple<VAO, Model>>();

		private CameraOrbit camera = new CameraOrbit();
		private ShaderFileDebugger shaderWatcher;
		private ShaderFileDebugger flagShaderWatcher;

		private VAO groundVAO;
		private Texture groundTexture;

		VAO flagVao;
		Texture flagTexture;

		public SunMoon SunMoon { get; set; }
		
		public MainVisual()
		{
			Looki = new LookAtCamera();

			SunMoon = new SunMoon();

			Model rabbit = Model.CreateModel(Resources.rabbit, Resources.rabbit_d);
			rabbit.Position = new Vector3(-3f, 0f, 0f);

			Model r2d2 = Model.CreateModel(Resources.r2d2, Resources.r2d2_d);
			r2d2.Position = new Vector3(-1f, 0f, 0f);

			Model statue = Model.CreateModel(Resources.statue, Resources.statue_d);
			statue.Position = new Vector3(1f, 0f, 0f);
			//statue.Scale = 0.5f;

			Model nyra = Model.CreateModel(Resources.nyra, Resources.nyra_d);
			nyra.Position = new Vector3(3f, 0.3f, 0.0f);
			nyra.Scale = 0.15f;

			models.Add(rabbit);
			models.Add(r2d2);
			models.Add(statue);
			models.Add(nyra);

			shaderWatcher = new ShaderFileDebugger("../../DemoScene/Resources/vertex.vert"
				, "../../DemoScene/Resources/fragment.frag"
				, Resources.vertex, Resources.fragment);

			flagShaderWatcher = new ShaderFileDebugger("../../DemoScene/Resources/flagvertex.vert"
				, "../../DemoScene/Resources/fragment.frag"
				, Resources.flagvertex, Resources.fragment);


			groundTexture = TextureLoader.FromBitmap(Resources.autumn);

			flagVao = VaoFactory.Instance.CreateFlagVao(shaderWatcher.Shader);
			flagTexture = models[3].DiffuseTexture;

			UpdateGeometries(shaderWatcher.Shader);

			camera.FarClip = 2000;
			camera.Distance = 5;
			camera.FovY = 50;

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
		}

		private void UpdateGeometries(Shader shader)
		{
			geometries.Clear();

			foreach (Model model in models)
			{
				VAO vao = VaoFactory.Instance.CreateFromMesh(shader, model.ObjectMesh);

				geometries.Add(new Tuple<VAO, Model>(vao, model));
			}
		}

		float time = 0;

		public void RotateModels()
		{
			modelAngle += 0.02f;
		}

		public void Render()
		{
			time += 0.02f;

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			var shader = shaderWatcher.Shader;
			shader.Begin();

			Matrix4 cam = camera.CalcMatrix();

			Vector3 eye = new Vector3(0, 2, 10);
			Vector3 target = new Vector3(0, 2, 0);
			Vector3 up = new Vector3(0, 1, 0);
			//Matrix4 cam = Matrix4.LookAt(eye, target, up);

			//cam.Transpose();

			//Matrix4 cam = Looki.GetMatrix();

			GL.Uniform1(shader.GetUniformLocation("time"), time);

			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref cam);
			GL.Uniform3(shader.GetUniformLocation("cameraPosition"), camera.CalcPosition());

			//GL.Uniform1(shader.GetUniformLocation("texN"), texture_n.ID);
			//GL.Uniform1(shader.GetUniformLocation("texS"), texture_s.ID);

			GL.Uniform3(shader.GetUniformLocation("lightDirection"), SunMoon.GetLightDirection());
			GL.Uniform3(shader.GetUniformLocation("lightColor"), SunMoon.GetLightColor());

			if (shaderWatcher.CheckForShaderChange())
			{
				//update geometry when shader changes
				UpdateGeometries(shaderWatcher.Shader);
			}

			float angle = 0.4f;

			foreach (Tuple<VAO, Model> geometry in geometries)
			{
				Model model = geometry.Item2;

				Texture texture = model.DiffuseTexture;

				Matrix3 rotation = Matrix3.CreateRotationY(modelAngle + angle);
				GL.UniformMatrix3(shader.GetUniformLocation("instanceRotation"), false, ref rotation);

				angle += 0.3f;

				GL.Uniform1(shader.GetUniformLocation("texD"), texture.ID);

				GL.Uniform3(shader.GetUniformLocation("instancePosition"), model.Position);
				GL.Uniform1(shader.GetUniformLocation("instanceScale"), model.Scale);

				texture.BeginUse();
				geometry.Item1.Draw();
				texture.EndUse();
			}

			Texture test = models[0].DiffuseTexture;

			GL.Uniform1(shader.GetUniformLocation("texD"), test.ID);

			GL.Uniform3(shader.GetUniformLocation("instancePosition"), Vector3.Zero);
			GL.Uniform1(shader.GetUniformLocation("instanceScale"), 1f);
			test.BeginUse();
			//groundVAO.Draw();
			test.EndUse();

			//VAO myFlagVao = FlagTest(shaderWatcher.Shader);

			GL.Uniform1(shader.GetUniformLocation("texD"), groundTexture.ID);

			groundTexture.BeginUse();
			//groundVAO.Draw();
			groundTexture.EndUse();

			shader.End();

			RenderFlag(cam);
		}


		private void RenderFlag(Matrix4 cam)
		{
			Shader flagShader = flagShaderWatcher.Shader;

			flagShader.Begin();

			Texture flagTexture = models[1].DiffuseTexture;

			GL.Uniform1(flagShader.GetUniformLocation("time"), time);
			GL.UniformMatrix4(flagShader.GetUniformLocation("camera"), true, ref cam);
			GL.Uniform3(flagShader.GetUniformLocation("cameraPosition"), camera.CalcPosition());

			GL.Uniform1(flagShader.GetUniformLocation("texD"), flagTexture.ID);

			GL.Uniform3(flagShader.GetUniformLocation("lightDirection"), SunMoon.GetLightDirection());
			GL.Uniform3(flagShader.GetUniformLocation("lightColor"), SunMoon.GetLightColor());

			GL.Uniform3(flagShader.GetUniformLocation("instancePosition"), new Vector3(-1, -1, -1));
			GL.Uniform1(flagShader.GetUniformLocation("instanceScale"), 0.15f);

			GL.Uniform3(flagShader.GetUniformLocation("polePosition"), new Vector3(-5, 2, -4));
			GL.Uniform3(flagShader.GetUniformLocation("windDirection"), new Vector3(-1.0f, 0, -0.0f));
			GL.Uniform1(flagShader.GetUniformLocation("waveSpeed"), 5f);
			GL.Uniform1(flagShader.GetUniformLocation("waveAmplitude"), 0.6f);

			// 6-3 

			flagTexture.BeginUse();
			flagVao.Draw();
			flagTexture.EndUse();

			flagShader.End();
		}


		private List<VAO> CreateGroundVaos(Shader shader)
		{
			List<VAO> vaos = new List<VAO>();

			Vector3 point1 = new Vector3(10, 0, 10);
			Vector3 point2 = new Vector3(-10, 0, -10);

			return vaos;
		}
		
	}
}
