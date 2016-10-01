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
	class FigurinesVisual : MyVisual
	{

		private Shader defaultShader;
		private Shader specularTextureShader;


		public FigurinesVisual(MainVisual mainVisual, Models models, DemoLevel demoLevel) : base(mainVisual, models, demoLevel)
		{
			defaultShader = CreateShader(Resources.vertex, Resources.fragment);
			specularTextureShader = CreateShader(Resources.vertex, Resources.speculartexfragment);

			models.CreateFigurines(defaultShader);
			models.CreateSpecularFigurines(specularTextureShader);
		}


		public override void Render(Matrix4 camera)
		{
			defaultShader.Begin();

			SetDefaultVertexUniforms(defaultShader, camera);

			SetSunMoonUniforms(defaultShader);

			GL.Uniform3(defaultShader.GetUniformLocation("lightningBugPosition"), mainVisual.LightningBugPosition);

			GL.Uniform3(defaultShader.GetUniformLocation("playerPosition"), demoLevel.Player.Position);

			foreach (Model figurine in models.Figurines)
			{
				RenderModel(defaultShader, figurine);
			}

			defaultShader.End();


			specularTextureShader.Begin();

			SetDefaultVertexUniforms(specularTextureShader, camera);
			SetSunMoonUniforms(specularTextureShader);

			foreach (Model figurine in models.SpecularFigurines)
			{
				RenderModel(specularTextureShader, figurine, false, true);
			}

			specularTextureShader.End();
		}

	}
}
