using DemoScene.DemoObjects;
using DemoScene.Visual.MyVisuals;
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

		private DemoLevel demoLevel;
		private Models models;

		public bool PassTime { get; set; }

		public float Time { get; set; } = 0;

		public Vector3 LightningBugPosition;


		public DefaultVisual defaultVisual { get; set; }
		public FigurinesVisual figurinesVisual { get; set; }
		public EffectFigurinesVisual effectFigurinesVisual { get; set; }
		public BallsVisual ballsVisual { get; set; }
		public FlagVisual flagVisual { get; set; }
		public TetrahedronSphereVisual tetrahedronSphereVisual { get; set; }
		public ParticleSystemVisual particleSystemVisual { get; set; }

		private List<MyVisual> visuals = new List<MyVisual>();


		public MainVisual(DemoLevel demoLevel)
		{
			models = new Models();
			this.demoLevel = demoLevel;

			FirstPersonCamera = new FirstPersonCamera(demoLevel.Player);

			defaultVisual = new DefaultVisual(this, models, demoLevel);
			figurinesVisual = new FigurinesVisual(this, models, demoLevel);
			effectFigurinesVisual = new EffectFigurinesVisual(this, models, demoLevel);
			ballsVisual = new BallsVisual(this, models, demoLevel);
			flagVisual = new FlagVisual(this, models, demoLevel);
			tetrahedronSphereVisual = new TetrahedronSphereVisual(this, models, demoLevel);
			particleSystemVisual = new ParticleSystemVisual(this, models, demoLevel);

			visuals.Add(defaultVisual);
			visuals.Add(figurinesVisual);
			visuals.Add(effectFigurinesVisual);
			visuals.Add(ballsVisual);
			visuals.Add(flagVisual);
			visuals.Add(tetrahedronSphereVisual);
			visuals.Add(particleSystemVisual);

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
			float rotationDelta = 0.02f;

			foreach (Model model in models.Figurines)
			{
				model.RenderSettings.Rotation += rotationDelta;
			}

			foreach (Model model in models.SpecularFigurines)
			{
				model.RenderSettings.Rotation += rotationDelta;
			}

			foreach (Model model in models.EffectFigurines)
			{
				model.RenderSettings.Rotation += rotationDelta;
			}
		}

		private Matrix4 GetCurrentCameraMatrix()
		{
			return FirstPersonCamera.GetMatrix();
		}

		public void Render()
		{
			if (PassTime) Time += 0.02f;

			if (demoLevel.SunMoon.IsDay()) LightningBugPosition = new Vector3(1000, 1000, 1000);
			else LightningBugPosition = demoLevel.LightningBug.Position;

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			Matrix4 cam = GetCurrentCameraMatrix();

			foreach (MyVisual visual in visuals) visual.TryRender(cam);
		}
		
	}
}
