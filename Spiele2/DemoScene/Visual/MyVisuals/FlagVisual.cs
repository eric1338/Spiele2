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
	class FlagVisual : MyVisual
	{

		private Shader flagShader;


		public FlagVisual(MainVisual mainVisual, Models models, DemoLevel demoLevel) : base(mainVisual, models, demoLevel)
		{
			flagShader = CreateShader(Resources.flagvertex, Resources.fragment);

			models.CreateFlag(flagShader);
		}


		public override void Render(Matrix4 camera)
		{
			flagShader.Begin();

			VAO vao = models.Flag.Vao;
			RenderSettings renderSettings = models.Flag.RenderSettings;
			Texture flagTexture = models.Flag.RenderSettings.DiffuseTexture;

			SetDefaultVertexUniforms(flagShader, camera);
			SetSunMoonUniforms(flagShader);

			GL.Uniform1(flagShader.GetUniformLocation("diffuseTexture"), renderSettings.DiffuseTexture.ID);

			GL.Uniform3(flagShader.GetUniformLocation("lightningBugPosition"), mainVisual.LightningBugPosition);

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


	}
}
