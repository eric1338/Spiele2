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
	class TetrahedronSphereVisual : MyVisual
	{

		private Shader colorShader;

		public TetrahedronSphereVisual(MainVisual mainVisual, Models models, DemoLevel demoLevel) : base(mainVisual, models, demoLevel)
		{
			colorShader = CreateShader(Resources.vertex, Resources.colorfragment);

			models.CreateTetrahedrons(colorShader, demoLevel.TetrahedronSphere.TetrahedronCount);
		}


		public override void Render(Matrix4 camera)
		{
			TetrahedronSphere tetrahedronSphere = demoLevel.TetrahedronSphere;

			int tetrahedronCount = tetrahedronSphere.TetrahedronCount;
			List<MovingTetrahedron> tetrahedrons = tetrahedronSphere.Tetrahedrons;

			colorShader.Begin();

			SetDefaultVertexUniforms(colorShader, camera);
			SetSunMoonUniforms(colorShader);

			for (int i = 0; i < tetrahedronCount; i++)
			{
				MovingTetrahedron tetrahedron = tetrahedrons[i];

				Model tetrahedronModel = models.Tetrahedrons[i];

				RenderSettings renderSettings = tetrahedronModel.RenderSettings;
				renderSettings.Position = tetrahedron.Position;
				renderSettings.Scale = tetrahedron.GetScale();
				renderSettings.Color = tetrahedron.GetColor();

				RenderModel(colorShader, tetrahedronModel);
			}

			colorShader.End();
		}



	}
}
