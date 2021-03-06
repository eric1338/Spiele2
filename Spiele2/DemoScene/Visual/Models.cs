﻿using Framework;
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
		public List<Model> SpecularFigurines = new List<Model>();
		public List<Model> EffectFigurines = new List<Model>();

		public Model PixelFigurine;
		public Model PixelFragmentFigurine;
		public Model BlurFigurine;
		public Model TransparencyFigurine;

		public Model DefaultBall;
		public Model CellShadingBall;
		public Model CellAndToonShadingBall;
		public Model Rabbit;
		public Model Flag;

		public Model Sun;
		public Model Moon;

		public List<Model> Tetrahedrons = new List<Model>();

		public List<Model> Ground = new List<Model>();
		public List<Model> DaySkybox = new List<Model>();
		public List<Model> NightSkybox = new List<Model>();

		public Model HelpBoard;
		public Model KeysBoard;

		public void CreateFigurines(Shader shader)
		{
			CreatePlainFigurines(shader);
			CreateShinyFigurines(shader);
		}

		private void CreatePlainFigurines(Shader shader)
		{
			RenderSettings r2d2 = new RenderSettings(Textures.Instance.R2D2Diffuse);
			r2d2.Position = GetFigurinePosition(4);
			r2d2.Rotation = GetFigurineRotation(4);

			RenderSettings statue = new RenderSettings(Textures.Instance.StatueDiffuse);
			statue.Position = GetFigurinePosition(7);
			statue.Rotation = GetFigurineRotation(7);

			RenderSettings nyra = new RenderSettings(Textures.Instance.NyraDiffuse);
			nyra.Position = GetFigurinePosition(10, 0.32f);
			nyra.Rotation = GetFigurineRotation(10);
			nyra.Scale = 0.15f;

			VAO r2d2Vao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.r2d2);
			Figurines.Add(new Model(r2d2Vao, r2d2));

			VAO statueVao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.statue);
			Figurines.Add(new Model(statueVao, statue));

			VAO nyraVao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.nyra);
			Figurines.Add(new Model(nyraVao, nyra));
		}

		public void CreateShinyFigurines(Shader shader)
		{
			RenderSettings r2d2 = new RenderSettings(Textures.Instance.R2D2Diffuse);
			r2d2.Position = GetFigurinePosition(5);
			r2d2.Rotation = GetFigurineRotation(5);
			r2d2.SpecularFactor = 1;

			RenderSettings statue = new RenderSettings(Textures.Instance.StatueDiffuse);
			statue.Position = GetFigurinePosition(8);
			statue.Rotation = GetFigurineRotation(8);
			statue.SpecularFactor = 1;

			RenderSettings nyra = new RenderSettings(Textures.Instance.NyraDiffuse);
			nyra.Position = GetFigurinePosition(11, 0.32f);
			nyra.Rotation = GetFigurineRotation(11);
			nyra.SpecularFactor = 1;
			nyra.Scale = 0.15f;

			VAO r2d2Vao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.r2d2);
			Figurines.Add(new Model(r2d2Vao, r2d2));

			VAO statueVao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.statue);
			Figurines.Add(new Model(statueVao, statue));

			VAO nyraVao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.nyra);
			Figurines.Add(new Model(nyraVao, nyra));
		}

		public void CreateSpecularFigurines(Shader shader)
		{
			RenderSettings r2d2 = new RenderSettings(Textures.Instance.R2D2Diffuse);
			r2d2.SpecularTexture = Textures.Instance.R2D2Specular;
			r2d2.Position = GetFigurinePosition(6);
			r2d2.Rotation = GetFigurineRotation(6);
			r2d2.SpecularFactor = 1;

			RenderSettings statue = new RenderSettings(Textures.Instance.StatueDiffuse);
			statue.SpecularTexture = Textures.Instance.StatueSpecular;
			statue.Position = GetFigurinePosition(9);
			statue.Rotation = GetFigurineRotation(9);
			statue.SpecularFactor = 1;

			RenderSettings nyra = new RenderSettings(Textures.Instance.NyraDiffuse);
			nyra.SpecularTexture = Textures.Instance.NyraSpecular;
			nyra.Position = GetFigurinePosition(12, 0.32f);
			nyra.Rotation = GetFigurineRotation(12);
			nyra.SpecularFactor = 1;
			nyra.Scale = 0.15f;

			VAO r2d2Vao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.r2d2);
			SpecularFigurines.Add(new Model(r2d2Vao, r2d2));

			VAO statueVao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.statue);
			SpecularFigurines.Add(new Model(statueVao, statue));

			VAO nyraVao = VaoFactory.Instance.CreateFromObjectData(shader, Resources.nyra);
			SpecularFigurines.Add(new Model(nyraVao, nyra));
		}


		private float radius = 15;
		private int numberOfFigurines = 13;

		private Vector3 GetFigurinePosition(int number, float y = 0)
		{
			float angle = 1.57f + numberOfFigurines * 0.08f - number * 0.16f;

			float x = (float) Math.Cos(angle) * radius + 4;
			float z = (float) -Math.Sin(angle) * radius - 4;

			return new Vector3(x, y, z);
		}

		private float GetFigurineRotation(int number)
		{
			return numberOfFigurines * -0.08f + number * 0.16f;
		}


		public void CreatePixelFigurines(Shader pixelFragmentShader, Shader pixelShader)
		{
			RenderSettings nyra1 = new RenderSettings(Textures.Instance.NyraDiffuse);
			nyra1.Position = new Vector3(-20, 0.32f, -10);
			nyra1.Rotation = -1.57f;
			nyra1.Scale = 0.15f;

			RenderSettings nyra2 = new RenderSettings(Textures.Instance.NyraDiffuse);
			nyra2.Position = new Vector3(-20, 0.32f, -13);
			nyra2.Rotation = -1.57f;
			nyra2.Scale = 0.15f;

			VAO nyra1Vao = VaoFactory.Instance.CreateFromObjectData(pixelFragmentShader, Resources.nyra);
			PixelFragmentFigurine = new Model(nyra1Vao, nyra1);

			VAO nyra2Vao = VaoFactory.Instance.CreateFromObjectData(pixelShader, Resources.nyra);
			PixelFigurine = new Model(nyra2Vao, nyra2);

			EffectFigurines.Add(PixelFragmentFigurine);
			EffectFigurines.Add(PixelFigurine);
		}

		public void CreateBlurFigurine(Shader blurShader)
		{
			RenderSettings nyra = new RenderSettings(Textures.Instance.NyraDiffuse);
			nyra.Position = new Vector3(-20, 0.32f, -7);
			nyra.Rotation = -1.57f;
			nyra.Scale = 0.15f;

			VAO nyraVao = VaoFactory.Instance.CreateFromObjectData(blurShader, Resources.nyra);
			BlurFigurine = new Model(nyraVao, nyra);

			EffectFigurines.Add(BlurFigurine);
		}

		public void CreateTransparencyFigurine(Shader transparencyShader)
		{
			RenderSettings casualman = new RenderSettings(Textures.Instance.CasualManDiffuse);
			casualman.Position = new Vector3(-20, 0, -4);
			casualman.Rotation = -1.57f;

			VAO casualmanVao = VaoFactory.Instance.CreateFromObjectData(transparencyShader, Resources.casualman);
			TransparencyFigurine = new Model(casualmanVao, casualman);

			EffectFigurines.Add(TransparencyFigurine);
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

		public void CreateTetrahedrons(Shader shader, int count)
		{
			for (int i = 0; i < count; i++)
			{
				VAO vao = VaoFactory.Instance.CreateTetrahedron(shader);

				Vector3 color = new Vector3(0.2f, 0.7f, 1.0f);
				RenderSettings renderSettings = RenderSettings.CreateColoredRenderSettings(color);

				Tetrahedrons.Add(new Model(vao, renderSettings));
			}
		}


		public void CreateGround(Shader shader)
		{
			Texture groundTexture = Textures.Instance.Ground;

			float l = 8;
			Vector3 a, b, c, d;

			for (int i = -2; i < 4; i++)
			{
				for (int j = -2; j < 4; j++)
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

			CreateBoards(shader);
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

		private void CreateBoards(Shader shader)
		{
			float y = 80;
			float helpBoardLength = 16;
			float keysBoardLength = 64;

			Vector3 a1 = new Vector3(helpBoardLength, y, helpBoardLength);
			Vector3 b1 = new Vector3(-helpBoardLength, y, helpBoardLength);
			Vector3 c1 = new Vector3(-helpBoardLength, y, -helpBoardLength);
			Vector3 d1 = new Vector3(helpBoardLength, y, -helpBoardLength);
			Vector3 a2 = new Vector3(keysBoardLength, y, keysBoardLength);
			Vector3 b2 = new Vector3(-keysBoardLength, y, keysBoardLength);
			Vector3 c2 = new Vector3(-keysBoardLength, y, -keysBoardLength);
			Vector3 d2 = new Vector3(keysBoardLength, y, -keysBoardLength);

			HelpBoard = Create2DModel(shader, a1, b1, c1, d1, Textures.Instance.HelpBoard);
			KeysBoard = Create2DModel(shader, a2, b2, c2, d2, Textures.Instance.KeysBoard);
		}

		private Model Create2DModel(Shader shader, Vector3 a, Vector3 b, Vector3 c, Vector3 d, Texture texture)
		{
			RenderSettings model = new RenderSettings(texture);

			VAO vao = VaoFactory.Instance.Create2DVao(shader, a, b, c, d, true);

			return new Model(vao, model);
		}

		public void CreateSunMoon(Shader shader)
		{
			Vector3 a = new Vector3(-10, -10, 0);
			Vector3 b = new Vector3(10, -10, 0);
			Vector3 c = new Vector3(10, 10, 0);
			Vector3 d = new Vector3(-10, 10, 0);

			Sun = Create2DModel(shader, a, b, c, d, Textures.Instance.Sun);
			Moon = Create2DModel(shader, a, b, c, d, Textures.Instance.Moon);
		}

	}
}
