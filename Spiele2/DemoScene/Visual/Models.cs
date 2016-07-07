using Framework;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.Visual
{
	class Models
	{

		public List<Model> Figurines = new List<Model>();
		public List<Model> Ground = new List<Model>();
		public List<Model> DaySkybox = new List<Model>();
		public List<Model> NightSkybox = new List<Model>();
		public Model Flag;

		public void CreateFigurines(Shader shader)
		{
			RenderSettings rabbit = new RenderSettings(Textures.Instance.RabbitDiffuse);
			rabbit.Position = new Vector3(-3f, 0f, 0f);
			rabbit.Rotation = 0.3f;

			RenderSettings r2d2 = new RenderSettings(Textures.Instance.R2D2Diffuse);
			r2d2.Position = new Vector3(-1f, 0f, 0f);
			r2d2.Rotation = 0.1f;

			RenderSettings statue = new RenderSettings(Textures.Instance.StatueDiffuse);
			statue.Position = new Vector3(1f, 0f, 0f);
			statue.Rotation = -0.1f;

			RenderSettings nyra = new RenderSettings(Textures.Instance.NyraDiffuse);
			nyra.Position = new Vector3(3f, 0.3f, 0.0f);
			nyra.Scale = 0.15f;
			nyra.Rotation = -0.3f;

			VAO rabbitVao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.rabbit);
			Figurines.Add(new Model(rabbitVao, rabbit));

			VAO r2d2Vao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.r2d2);
			Figurines.Add(new Model(r2d2Vao, r2d2));

			VAO statueVao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.statue);
			Figurines.Add(new Model(statueVao, statue));

			VAO nyraVao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.nyra);
			Figurines.Add(new Model(nyraVao, nyra));
		}

		public void CreateFlag(Shader shader)
		{
			VAO vao = VaoFactory.Instance.CreateFlagVao(shader);
			RenderSettings renderSettings = new RenderSettings(Textures.Instance.Flag);

			Flag = new Model(vao, renderSettings);
		}

		public void CreateSkyboxes(Shader shader)
		{
			DaySkybox = GetSkyboxModels(shader, true);
			NightSkybox = GetSkyboxModels(shader, false);
		}

		public List<Model> GetSkyboxModels(Shader shader, bool day)
		{
			List<Model> skyboxModels = new List<Model>();

			float l = 50;

			Vector3 a = new Vector3(-l, -l, -l);
			Vector3 b = new Vector3(l, -l, -l);
			Vector3 c = new Vector3(l, -l, l);
			Vector3 d = new Vector3(-l, -l, l);
			Vector3 e = new Vector3(-l, l, -l);
			Vector3 f = new Vector3(l, l, -l);
			Vector3 g = new Vector3(l, l, l);
			Vector3 h = new Vector3(-l, l, l);

			Texture sideTexture = day ? Textures.Instance.DaySkySide : Textures.Instance.NightSkySide;
			Texture topTexture = day ? Textures.Instance.DaySkyTop : Textures.Instance.NightSkyTop;
			Texture bottomTexture = day ? Textures.Instance.DaySkyBottom : Textures.Instance.NightSkyBottom;

			skyboxModels.Add(Create2DModel(shader, a, b, c, d, bottomTexture));
			skyboxModels.Add(Create2DModel(shader, a, b, f, e, sideTexture));
			skyboxModels.Add(Create2DModel(shader, b, c, g, f, sideTexture));
			skyboxModels.Add(Create2DModel(shader, c, d, h, g, sideTexture));
			skyboxModels.Add(Create2DModel(shader, d, a, e, h, sideTexture));
			skyboxModels.Add(Create2DModel(shader, e, f, g, h, topTexture));

			return skyboxModels;
		}

		public void CreateGround(Shader shader)
		{
			Texture groundTexture = Textures.Instance.Ground;

			float l = 10;
			Vector3 a, b, c, d;

			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					a = new Vector3((i - 1) * l, 0, (j - 1) * l);
					b = new Vector3(i * l, 0, (j - 1) * l);
					c = new Vector3(i * l, 0, j * l);
					d = new Vector3((i - 1) * l, 0, j * l);

					Ground.Add(Create2DModel(shader, a, b, c, d, groundTexture));
				}
			}
		}

		private Model Create2DModel(Shader shader, Vector3 a, Vector3 b, Vector3 c, Vector3 d, Texture texture)
		{
			RenderSettings model = new RenderSettings(texture);

			VAO vao = VaoFactory.Instance.Create2DVao(shader, a, b, c, d, true);

			return new Model(vao, model);
		}

	}
}
