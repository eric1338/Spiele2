using Framework;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.Visual
{
	class RenderSettings
	{

		public bool UseTexture { get; set; }

		public Texture DiffuseTexture { get; set; }
		public Texture SpecularTexture { get; set; }

		public Vector3 Color { get; set; }

		public Vector3 Position { get; set; }
		public float Scale { get; set; }
		public float Rotation { get; set; }

		public RenderSettings(Texture diffuseTexture)
		{
			Position = Vector3.Zero;
			Scale = 1;
			Rotation = 0;

			UseTexture = true;
			DiffuseTexture = diffuseTexture;
		}

		public static RenderSettings CreateColoredRenderSettings(Vector3 color)
		{
			RenderSettings renderSettings = new RenderSettings(null);

			renderSettings.UseTexture = false;
			renderSettings.Color = color;

			return renderSettings;
		}
	}
}
