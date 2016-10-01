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
			RenderSunMoon(camera);
			RenderSkybox(camera);
		}

		public void RenderRabbitAndGround(Matrix4 camera)
		{
			defaultShader.Begin();

			SetDefaultVertexUniforms(defaultShader, camera);

			SetSunMoonUniforms(defaultShader);

			GL.Uniform3(defaultShader.GetUniformLocation("lightningBugPosition"), mainVisual.LightningBugPosition);

			GL.Uniform3(defaultShader.GetUniformLocation("playerPosition"), demoLevel.Player.Position);

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

		private void RenderSunMoon(Matrix4 camera)
		{
			SunMoon sunMoon = demoLevel.SunMoon;

			skyboxShader.Begin();
			SetDefaultVertexUniforms(skyboxShader, camera);

			GL.Uniform1(skyboxShader.GetUniformLocation("brightness"), sunMoon.GetIntensity());

			Model model = sunMoon.IsDay() ? models.Sun : models.Moon;

			model.RenderSettings.Position = sunMoon.GetLightPosition();

			RenderModel(skyboxShader, model, true);

			skyboxShader.End();
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


	}
}
