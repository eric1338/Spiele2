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

		public Texture DiffuseTexture { get; set; }
		public Texture SpecularTexture { get; set; }

		public Vector3 Position { get; set; }
		public float Scale { get; set; }
		public float Rotation { get; set; }

		public RenderSettings(Texture diffuseTexture)
		{
			Position = Vector3.Zero;
			Scale = 1;
			Rotation = 0;

			DiffuseTexture = diffuseTexture;
		}
	}
}
