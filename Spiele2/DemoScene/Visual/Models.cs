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
		public Model DefaultBall;
		public Model CellShadingBall;
		public Model CellAndToonShadingBall;
		public Model Rabbit;
		public Model Flag;

		public List<Model> Ground = new List<Model>();
		public List<Model> DaySkybox = new List<Model>();
		public List<Model> NightSkybox = new List<Model>();

		public void CreateFigurines(Shader shader)
		{
			RenderSettings rabbit = new RenderSettings(Textures.Instance.RabbitDiffuse);
			rabbit.Position = GetFigurinePosition(1);
			rabbit.Rotation = GetFigurineRotation(1);

			RenderSettings r2d2 = new RenderSettings(Textures.Instance.R2D2Diffuse);
			r2d2.Position = GetFigurinePosition(3);
			r2d2.Rotation = GetFigurineRotation(3);

			RenderSettings statue = new RenderSettings(Textures.Instance.StatueDiffuse);
			statue.Position = GetFigurinePosition(5);
			statue.Rotation = GetFigurineRotation(5);

			RenderSettings nyra = new RenderSettings(Textures.Instance.NyraDiffuse);
			nyra.Position = GetFigurinePosition(7, 0.32f);
			nyra.Rotation = GetFigurineRotation(7);
			nyra.Scale = 0.15f;

			VAO rabbitVao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.rabbit);
			Figurines.Add(new Model(rabbitVao, rabbit));

			VAO r2d2Vao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.r2d2);
			Figurines.Add(new Model(r2d2Vao, r2d2));

			VAO statueVao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.statue);
			Figurines.Add(new Model(statueVao, statue));

			VAO nyraVao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.nyra);
			Figurines.Add(new Model(nyraVao, nyra));

			Test(shader);
		}

		public void Test(Shader shader)
		{
			RenderSettings rabbit = new RenderSettings(Textures.Instance.RabbitDiffuse);
			rabbit.Position = GetFigurinePosition(2);
			rabbit.Rotation = GetFigurineRotation(2);

			RenderSettings r2d2 = new RenderSettings(Textures.Instance.R2D2Diffuse);
			r2d2.Position = GetFigurinePosition(4);
			r2d2.Rotation = GetFigurineRotation(4);

			RenderSettings statue = new RenderSettings(Textures.Instance.StatueDiffuse);
			statue.Position = GetFigurinePosition(6);
			statue.Rotation = GetFigurineRotation(6);

			RenderSettings nyra = new RenderSettings(Textures.Instance.NyraDiffuse);
			nyra.Position = GetFigurinePosition(8, 0.32f);
			nyra.Rotation = GetFigurineRotation(8);
			nyra.Scale = 0.15f;

			VAO rabbitVao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.rabbit);
			Figurines.Add(new Model(rabbitVao, rabbit));

			VAO r2d2Vao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.r2d2);
			Figurines.Add(new Model(r2d2Vao, r2d2));

			VAO statueVao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.statue);
			Figurines.Add(new Model(statueVao, statue));

			VAO nyraVao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.nyra);
			Figurines.Add(new Model(nyraVao, nyra));
		}

		private float radius = 15;
		private int numberOfFigurines = 9;

		private Vector3 GetFigurinePosition(int number, float y = 0)
		{
			float angle = 1.57f + numberOfFigurines * 0.1f - number * 0.2f;

			float x = (float) Math.Cos(angle) * radius;
			float z = (float) -Math.Sin(angle) * radius;

			return new Vector3(x, y, z);
		}

		private float GetFigurineRotation(int number)
		{
			return numberOfFigurines * -0.1f + number * 0.2f;
		}

		public void CreateBalls(Shader defaultShader, Shader cellShader, Shader cellAndToonShader)
		{
			DefaultBall = CreateBall(defaultShader, new Vector3(0.12f, 0.9f, 0.9f));
			CellShadingBall = CreateBall(cellShader, new Vector3(0.12f, 0.7f, 1.0f));
			CellAndToonShadingBall = CreateBall(cellAndToonShader, new Vector3(0.12f, 0.5f, 1.0f));
		}

		private Model CreateBall(Shader shader, Vector3 color)
		{
			Mesh sphereMesh = Meshes.CreateSphere(1, 4);

			VAO vao = VaoFactory.Instance.CreateFromMesh(shader, sphereMesh);
			RenderSettings renderSettings = RenderSettings.CreateColoredRenderSettings(color);

			return new Model(vao, renderSettings);
		}

		public void CreateRabbit(Shader shader)
		{
			VAO vao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.rabbit);
			RenderSettings renderSettings = new RenderSettings(Textures.Instance.RabbitDiffuse);

			Rabbit = new Model(vao, renderSettings);
		}

		public void CreateFlag(Shader shader)
		{
			VAO vao = VaoFactory.Instance.CreateFlagVao(shader);
			RenderSettings renderSettings = new RenderSettings(Textures.Instance.Flag);

			Flag = new Model(vao, renderSettings);
		}

		public void CreateGround(Shader shader)
		{
			Texture groundTexture = Textures.Instance.Ground;

			float l = 10;
			Vector3 a, b, c, d;

			for (int i = -1; i < 3; i++)
			{
				for (int j = -1; j < 3; j++)
				{
					a = new Vector3((i - 1) * l, 0, (j - 1) * l);
					b = new Vector3(i * l, 0, (j - 1) * l);
					c = new Vector3(i * l, 0, j * l);
					d = new Vector3((i - 1) * l, 0, j * l);

					Ground.Add(Create2DModel(shader, a, b, c, d, groundTexture));
				}
			}
		}

		public void CreateSkyboxes(Shader shader)
		{
			DaySkybox = GetSkyboxModels(shader, true);
			NightSkybox = GetSkyboxModels(shader, false);
		}

		public List<Model> GetSkyboxModels(Shader shader, bool day)
		{
			List<Model> skyboxModels = new List<Model>();

			float l = 90;

			Vector3 a = new Vector3(-l, -l, -l);
			Vector3 b = new Vector3(l, -l, -l);
			Vector3 c = new Vector3(l, -l, l);
			Vector3 d = new Vector3(-l, -l, l);
			Vector3 e = new Vector3(-l, l, -l);
			Vector3 f = new Vector3(l, l, -l);
			Vector3 g = new Vector3(l, l, l);
			Vector3 h = new Vector3(-l, l, l);

			Vector3 y = new Vector3(0, 5, 0);

			Texture sideTexture = day ? Textures.Instance.DaySkySide : Textures.Instance.NightSkySide;
			Texture topTexture = day ? Textures.Instance.DaySkyTop : Textures.Instance.NightSkyTop;
			Texture bottomTexture = day ? Textures.Instance.DaySkyBottom : Textures.Instance.NightSkyBottom;

			skyboxModels.Add(Create2DModel(shader, a + y, b + y, c + y, d + y, bottomTexture));
			skyboxModels.Add(Create2DModel(shader, a, b, f, e, sideTexture));
			skyboxModels.Add(Create2DModel(shader, b, c, g, f, sideTexture));
			skyboxModels.Add(Create2DModel(shader, c, d, h, g, sideTexture));
			skyboxModels.Add(Create2DModel(shader, d, a, e, h, sideTexture));
			skyboxModels.Add(Create2DModel(shader, e - y, f - y, g - y, h - y, topTexture));

			return skyboxModels;
		}

		private Model Create2DModel(Shader shader, Vector3 a, Vector3 b, Vector3 c, Vector3 d, Texture texture)
		{
			RenderSettings model = new RenderSettings(texture);

			VAO vao = VaoFactory.Instance.Create2DVao(shader, a, b, c, d, true);

			return new Model(vao, model);
		}

	}
}
