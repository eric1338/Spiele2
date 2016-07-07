using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.Visual
{
	class Model
	{

		public VAO Vao { get; set; }
		public RenderSettings RenderSettings { get; set; }

		public Model(VAO vao, RenderSettings renderSettings)
		{
			Vao = vao;
			RenderSettings = renderSettings;
		}

	}
}
