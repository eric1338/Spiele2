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
		private Shader sunMoonShader;
		private Shader skyboxShader;

		public DefaultVisual(MainVisual mainVisual, Models models, DemoLevel demoLevel) : base(mainVisual, models, demoLevel)
		{
			defaultShader = CreateShader(Resources.vertex, Resources.fragment);
			sunMoonShader = CreateShader(Resources.vertex, Resources.sunmoonfragment);
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

			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
			GL.BlendEquation(BlendEquationMode.FuncAdd);
			//GL.DepthMask(false);
			GL.Enable(EnableCap.Blend);
			//GL.Enable(EnableCap.PointSprite);
			//GL.Enable(EnableCap.VertexProgramPointSize);


			sunMoonShader.Begin();
			SetDefaultVertexUniforms(sunMoonShader, camera);

			GL.Uniform1(sunMoonShader.GetUniformLocation("brightness"), sunMoon.GetIntensity());

			Model model = sunMoon.IsDay() ? models.Sun : models.Moon;

			model.RenderSettings.Position = sunMoon.GetLightPosition();

			RenderModel(sunMoonShader, model);

			sunMoonShader.End();

			//GL.Disable(EnableCap.VertexProgramPointSize);
			//GL.Disable(EnableCap.PointSprite);
			GL.Disable(EnableCap.Blend);
			//GL.DepthMask(true);
		}

		/*
		
					private void RenderSunMoon(Matrix4 camera)
		{
			SunMoon sunMoon = demoLevel.SunMoon;

			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.Zero);
			GL.BlendEquation(BlendEquationMode.FuncAdd);
			//GL.DepthMask(false);
			GL.Enable(EnableCap.Blend);
			//GL.Enable(EnableCap.PointSprite);
			//GL.Enable(EnableCap.VertexProgramPointSize);

			sunMoonShader.Begin();
			SetDefaultVertexUniforms(sunMoonShader, camera);

			GL.Uniform1(sunMoonShader.GetUniformLocation("brightness"), sunMoon.GetIntensity());


			Model model = sunMoon.IsDay() ? models.Sun : models.Moon;

			model.RenderSettings.Position = sunMoon.GetLightPosition();

			Texture texture = model.RenderSettings.DiffuseTexture;

			GL.Uniform1(sunMoonShader.GetUniformLocation("sunMoonTexture"), texture.ID);

			RenderSettings renderSettings = model.RenderSettings;

			Matrix3 rotation = Matrix3.CreateRotationY(renderSettings.Rotation);
			GL.UniformMatrix3(sunMoonShader.GetUniformLocation("instanceRotation"), false, ref rotation);

			GL.Uniform3(sunMoonShader.GetUniformLocation("instancePosition"), renderSettings.Position);
			GL.Uniform1(sunMoonShader.GetUniformLocation("instanceScale"), renderSettings.Scale);

			texture.BeginUse();

			model.Vao.Draw();

			//model.Vao.DrawArrays(PrimitiveType.Points, 1);

			texture.EndUse();


			//RenderModel(sunMoonShader, model, true);

			sunMoonShader.End();

			//GL.Disable(EnableCap.VertexProgramPointSize);
			//GL.Disable(EnableCap.PointSprite);
			GL.Disable(EnableCap.Blend);
			//GL.DepthMask(true);
		}


		*/


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
