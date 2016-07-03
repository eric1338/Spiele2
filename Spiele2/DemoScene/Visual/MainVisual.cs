using DemoScene.DemoObjects;
using Framework;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using ShaderDebugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.Visual
{
	class MainVisual
	{

		public LookAtCamera Looki { get; set; }

		public CameraOrbit OrbitCamera { get { return camera; } }

		private List<Model> models = new List<Model>();

		private List<Tuple<VAO, Model>> geometries = new List<Tuple<VAO, Model>>();

		private CameraOrbit camera = new CameraOrbit();
		private ShaderFileDebugger shaderWatcher;
		private ShaderFileDebugger shaderWatcher2;

		private VAO groundVAO;
		private Texture groundTexture;

		public SunMoon SunMoon { get; set; }
		
		public MainVisual()
		{
			Looki = new LookAtCamera();

			SunMoon = new SunMoon();

			Model rabbit = Model.CreateModel(Resources.rabbit, Resources.rabbit_d);
			rabbit.Position = new Vector3(-3f, 0f, 0f);

			Model r2d2 = Model.CreateModel(Resources.r2d2, Resources.r2d2_d);
			r2d2.Position = new Vector3(-1f, 0f, 0f);

			Model statue = Model.CreateModel(Resources.statue, Resources.statue_d);
			statue.Position = new Vector3(1f, 0f, 0f);
			//statue.Scale = 0.5f;

			Model nyra = Model.CreateModel(Resources.nyra, Resources.nyra_d);
			nyra.Position = new Vector3(3f, 0.3f, 0.0f);
			nyra.Scale = 0.15f;

			models.Add(rabbit);
			models.Add(r2d2);
			models.Add(statue);
			models.Add(nyra);

			shaderWatcher = new ShaderFileDebugger("../../DemoScene/Resources/vertex.vert"
				, "../../DemoScene/Resources/fragment.frag"
				, Resources.vertex, Resources.fragment);

			shaderWatcher2 = new ShaderFileDebugger("../../DemoScene/Resources/flagvertex.vert"
				, "../../DemoScene/Resources/fragment.frag"
				, Resources.flagvertex, Resources.fragment);


			groundTexture = TextureLoader.FromBitmap(Resources.autumn);

			flagVao = FlagTest(shaderWatcher.Shader);
			flagTexture = models[3].DiffuseTexture;

			UpdateGeometries(shaderWatcher.Shader);

			camera.FarClip = 2000;
			camera.Distance = 5;
			camera.FovY = 50;

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
		}

		private void UpdateGeometries(Shader shader)
		{
			geometries.Clear();

			foreach (Model model in models)
			{
				VAO vao = CreateVAO(model.ObjectMesh, shader);

				geometries.Add(new Tuple<VAO, Model>(vao, model));
			}
		}

		float time = 0;

		public void Render()
		{
			time += 0.02f;

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			var shader = shaderWatcher.Shader;
			shader.Begin();

			Matrix4 cam = camera.CalcMatrix();

			Vector3 eye = new Vector3(0, 2, 10);
			Vector3 target = new Vector3(0, 2, 0);
			Vector3 up = new Vector3(0, 1, 0);
			//Matrix4 cam = Matrix4.LookAt(eye, target, up);

			//cam.Transpose();

			//Matrix4 cam = Looki.GetMatrix();

			GL.Uniform1(shader.GetUniformLocation("time"), time);

			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref cam);
			GL.Uniform3(shader.GetUniformLocation("cameraPosition"), camera.CalcPosition());

			//GL.Uniform1(shader.GetUniformLocation("texN"), texture_n.ID);
			//GL.Uniform1(shader.GetUniformLocation("texS"), texture_s.ID);

			GL.Uniform3(shader.GetUniformLocation("lightDirection"), SunMoon.GetLightDirection());
			GL.Uniform3(shader.GetUniformLocation("lightColor"), SunMoon.GetLightColor());

			if (shaderWatcher.CheckForShaderChange())
			{
				//update geometry when shader changes
				UpdateGeometries(shaderWatcher.Shader);
			}

			foreach (Tuple<VAO, Model> geometry in geometries)
			{
				Model model = geometry.Item2;

				Texture texture = model.DiffuseTexture;

				GL.Uniform1(shader.GetUniformLocation("texD"), texture.ID);

				GL.Uniform3(shader.GetUniformLocation("instancePosition"), model.Position);
				GL.Uniform1(shader.GetUniformLocation("instanceScale"), model.Scale);

				texture.BeginUse();
				geometry.Item1.Draw();
				texture.EndUse();
			}

			Texture test = models[0].DiffuseTexture;

			GL.Uniform1(shader.GetUniformLocation("texD"), test.ID);

			GL.Uniform3(shader.GetUniformLocation("instancePosition"), Vector3.Zero);
			GL.Uniform1(shader.GetUniformLocation("instanceScale"), 1f);
			test.BeginUse();
			//groundVAO.Draw();
			test.EndUse();

			//VAO myFlagVao = FlagTest(shaderWatcher.Shader);

			GL.Uniform1(shader.GetUniformLocation("texD"), groundTexture.ID);

			groundTexture.BeginUse();
			//groundVAO.Draw();
			groundTexture.EndUse();

			shader.End();

			Shader shader2 = shaderWatcher2.Shader;

			shader2.Begin();

			GL.Uniform1(shader2.GetUniformLocation("time"), time);
			GL.UniformMatrix4(shader2.GetUniformLocation("camera"), true, ref cam);
			GL.Uniform3(shader2.GetUniformLocation("cameraPosition"), camera.CalcPosition());

			GL.Uniform1(shader2.GetUniformLocation("texD"), test.ID);

			GL.Uniform3(shader2.GetUniformLocation("lightDirection"), SunMoon.GetLightDirection());
			GL.Uniform3(shader2.GetUniformLocation("lightColor"), SunMoon.GetLightColor());

			GL.Uniform3(shader2.GetUniformLocation("instancePosition"), Vector3.Zero);
			GL.Uniform1(shader2.GetUniformLocation("instanceScale"), 1f);

			test.BeginUse();
			flagVao.Draw();
			test.EndUse();

			shader2.End();
		}

		VAO flagVao;
		Texture flagTexture;


		private List<VAO> CreateGroundVaos(Shader shader)
		{
			List<VAO> vaos = new List<VAO>();

			Vector3 point1 = new Vector3(10, 0, 10);
			Vector3 point2 = new Vector3(-10, 0, -10);

			return vaos;
		}

		private VAO FlagTest(Shader shader)
		{
			List<Vector3> posis = new List<Vector3>();
			List<Vector3> normals = new List<Vector3>();

			Vector3 bottomLeft = Vector3.Zero;
			Vector3 topRight = Vector3.Zero;

			uint pointsPerRow = 200;
			uint pointRows = 200;

			for (int i = 0; i < pointRows; i++)
			{
				for (int j = 0; j < pointsPerRow; j++)
				{
					float z = (float) Math.Sin(time + i * 0.1f);

					z = 0;

					posis.Add(new Vector3(i * 0.05f, j * 0.025f, z * 0.2f));
					normals.Add(new Vector3(0, 0, 1));
				}
			}

			List<Vector2> uvs = CreateUVs(pointRows, pointsPerRow);
			List<uint> ids = CreateIDs(pointRows, pointsPerRow);

			return CreateMyVAO(shader, posis, normals, uvs, ids);
		}

		private VAO CreateSimpleVao(Shader shader, bool renderBack = true)
		{
			List<Vector3> positions = new List<Vector3>();

			Vector3 bottomLeft = Vector3.Zero;
			Vector3 topLeft = new Vector3(0, 1, 0);
			Vector3 bottomRight = new Vector3(1, 0, 0);
			Vector3 topRight = new Vector3(1, 1, 0);

			positions.Add(bottomLeft);
			positions.Add(topLeft);
			positions.Add(bottomRight);
			positions.Add(topRight);

			List<Vector3> normals = new List<Vector3>();

			// TODO: anders
			for (int i = 0; i < 3; i++) normals.Add(new Vector3(0, 0, 1));

			List<Vector2> uvs = CreateUVs(2, 2);
			List<uint> ids = CreateIDs(2, 2, renderBack);

			return CreateMyVAO(shader, positions, normals, uvs, ids);
		}

		private VAO CreateMyVAO(Shader shader, List<Vector3> positions, List<Vector3> normals, List<Vector2> uvs, List<uint> ids)
		{
			VAO vao = new VAO();

			vao.SetAttribute(shader.GetAttributeLocation("position"), positions.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetAttribute(shader.GetAttributeLocation("normal"), normals.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetAttribute(shader.GetAttributeLocation("uv"), uvs.ToArray(), VertexAttribPointerType.Float, 2);

			vao.SetID(ids.ToArray(), PrimitiveType.Triangles);

			return vao;
		}

		private List<Vector2> CreateUVs(uint pointRows, uint pointsPerRow)
		{
			List<Vector2> uvs = new List<Vector2>();

			for (int i = 0; i < pointRows; i++)
			{
				for (int j = 0; j < pointsPerRow; j++)
				{
					float u = i / (float)(pointRows - 1);
					float v = j / (float)(pointsPerRow - 1);

					uvs.Add(new Vector2(u, v));
				}
			}

			return uvs;
		}

		private List<uint> CreateIDs(uint pointRows, uint pointsPerRow, bool renderBack = true)
		{
			List<uint> ids = new List<uint>();

			for (uint i = 0; i < (pointRows - 1); i++)
			{
				for (uint j = 0; j < (pointsPerRow - 1); j++)
				{
					uint bottomLeft = i * pointRows + j;
					uint topLeft = bottomLeft + 1;
					uint bottomRight = ((i + 1) * pointRows) + j;
					uint topRight = bottomRight + 1;

					ids.Add(bottomLeft);
					ids.Add(bottomRight);
					ids.Add(topLeft);

					ids.Add(bottomRight);
					ids.Add(topRight);
					ids.Add(topLeft);

					if (renderBack)
					{
						ids.Add(topLeft);
						ids.Add(bottomRight);
						ids.Add(bottomLeft);

						ids.Add(topLeft);
						ids.Add(topRight);
						ids.Add(bottomRight);
					}
				}
			}

			return ids;
		}


		private VAO CreateVAO(Mesh mesh, Shader shader)
		{
			VAO vao = new VAO();

			vao.SetAttribute(shader.GetAttributeLocation("position"), mesh.positions.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetAttribute(shader.GetAttributeLocation("normal"), mesh.normals.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetAttribute(shader.GetAttributeLocation("uv"), mesh.uvs.ToArray(), VertexAttribPointerType.Float, 2);
			vao.SetID(mesh.ids.ToArray(), PrimitiveType.Triangles);

			return vao;
		}

	}
}
