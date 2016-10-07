using DemoScene.DemoObjects;
using Framework;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.Visual.MyVisuals
{
	class DefaultVisual : MyVisual
	{

		private Shader defaultShader;
		private Shader skyboxShader;

		public DefaultVisual(MainVisual mainVisual, Models models, DemoLevel demoLevel) : base(mainVisual, models, demoLevel)
		{
			defaultShader = CreateShader(Resources.vertex, Resources.fragment);
			skyboxShader = CreateShader(Resources.vertex, Resources.simplefragment);

			models.CreateRabbit(defaultShader);
			models.CreateSkyboxes(skyboxShader);
			models.CreateGround(defaultShader);
			models.CreateSunMoon(skyboxShader);
		}

		public override void Render(Matrix4 camera)
		{
			RenderRabbitAndGround(camera);
			RenderSkybox(camera);
		}

		public void RenderRabbitAndGround(Matrix4 camera)
		{
			defaultShader.Begin();

			SetDefaultVertexUniforms(defaultShader, camera);

			SetSunMoonUniforms(defaultShader);

			GL.Uniform3(defaultShader.GetUniformLocation("lightningBugPosition"), mainVisual.LightningBugPosition);
			GL.Uniform3(defaultShader.GetUniformLocation("playerPosition"), demoLevel.Player.Position);

			RenderSettings rabbitRenderSettings = models.Rabbit.RenderSettings;
			Rabbit rabbit = demoLevel.Rabbit;

			rabbitRenderSettings.Position = rabbit.Position;
			rabbitRenderSettings.Rotation = rabbit.Rotation;

			RenderModel(defaultShader, models.Rabbit);

			GL.Uniform1(defaultShader.GetUniformLocation("ambientFactor"), demoLevel.SunMoon.GetAmbientFactor() * 1.5f);

			foreach (Model groundModel in models.Ground)
			{
				RenderModel(defaultShader, groundModel);
			}

			defaultShader.End();
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

			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.BlendEquation(BlendEquationMode.FuncAdd);
			GL.Enable(EnableCap.Blend);

			GL.Uniform1(skyboxShader.GetUniformLocation("brightness"), 1);

			Model boardModel = demoLevel.ShowKeys ? models.KeysBoard : models.HelpBoard;

			RenderModel(skyboxShader, boardModel);

			SunMoon sunMoon = demoLevel.SunMoon;

			Model sunMoonModel = sunMoon.IsDay() ? models.Sun : models.Moon;

			sunMoonModel.RenderSettings.Position = sunMoon.GetLightPosition();
			
			RenderModel(skyboxShader, sunMoonModel);
			
			GL.Disable(EnableCap.Blend);

			skyboxShader.End();
		}


	}
}
