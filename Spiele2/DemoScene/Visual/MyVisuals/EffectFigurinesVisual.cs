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
	class EffectFigurinesVisual : MyVisual
	{

		private Shader pixelFragmentShader;
		private Shader pixelShader;
		private Shader blurShader;
		private Shader transparencyShader;


		public EffectFigurinesVisual(MainVisual mainVisual, Models models, DemoLevel demoLevel) : base(mainVisual, models, demoLevel)
		{
			pixelShader = CreateShader(Resources.pixelvertex, Resources.pixelfragment);
			pixelFragmentShader = CreateShader(Resources.vertex, Resources.pixelfragment);
			blurShader = CreateShader(Resources.vertex, Resources.blurfragment);
			transparencyShader = CreateShader(Resources.vertex, Resources.transparencyfragment);

			models.CreatePixelFigurines(pixelFragmentShader, pixelShader);
			models.CreateBlurFigurine(blurShader);
			models.CreateTransparencyFigurine(transparencyShader);
		}


		public override void Render(Matrix4 camera)
		{
			pixelFragmentShader.Begin();
			SetDefaultVertexUniforms(pixelFragmentShader, camera);
			SetSunMoonUniforms(pixelFragmentShader);

			RenderModel(pixelFragmentShader, models.PixelFragmentFigurine);

			pixelFragmentShader.End();

			pixelShader.Begin();
			SetDefaultVertexUniforms(pixelShader, camera);
			SetSunMoonUniforms(pixelShader);

			RenderModel(pixelShader, models.PixelFigurine);

			pixelShader.End();

			blurShader.Begin();
			SetDefaultVertexUniforms(blurShader, camera);
			SetSunMoonUniforms(blurShader);

			RenderModel(blurShader, models.BlurFigurine);

			pixelShader.End();

			transparencyShader.Begin();
			SetDefaultVertexUniforms(transparencyShader, camera);
			SetSunMoonUniforms(transparencyShader);

			GL.Uniform3(transparencyShader.GetUniformLocation("viewDirection"), mainVisual.FirstPersonCamera.GetForwardsVector());
			GL.Uniform3(transparencyShader.GetUniformLocation("playerPosition"), demoLevel.Player.Position);

			RenderModel(transparencyShader, models.TransparencyFigurine);

			pixelShader.End();
		}


	}
}
