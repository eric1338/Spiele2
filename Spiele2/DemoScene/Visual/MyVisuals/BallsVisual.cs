using DemoScene.DemoObjects;
using Framework;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.Visual.MyVisuals
{
	class BallsVisual : MyVisual
	{

		private Shader colorShader;
		private Shader cellShader;
		private Shader cellAndToonShader;


		public BallsVisual(MainVisual mainVisual, Models models, DemoLevel demoLevel) : base(mainVisual, models, demoLevel)
		{
			colorShader = CreateShader(Resources.vertex, Resources.colorfragment);
			cellShader = CreateShader(Resources.vertex, Resources.cellfragment);
			cellAndToonShader = CreateShader(Resources.vertex, Resources.celltoonfragment);

			models.CreateBalls(colorShader, cellShader, cellAndToonShader);
		}

		public override void Render(Matrix4 camera)
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


	}
}
