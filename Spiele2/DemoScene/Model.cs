using Framework;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene
{
	class Model
	{

		public Mesh ObjectMesh { get; set; }

		public Texture DiffuseTexture { get; set; }
		public Texture SpecularTexture { get; set; }

		public Vector3 Position { get; set; }

		public float Scale { get; set; }


		public Model()
		{

		}

		public static Model CreateModel(byte[] objectData, Bitmap diffuseBitmap)
		{
			Model model = new Model();

			model.ObjectMesh = Obj2Mesh.FromObj(objectData);
			model.DiffuseTexture = TextureLoader.FromBitmap(diffuseBitmap);

			return model;
		}

	}
}
