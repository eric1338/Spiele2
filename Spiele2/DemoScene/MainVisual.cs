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

namespace DemoScene
{
	class MainVisual
	{

		public CameraOrbit OrbitCamera { get { return camera; } }

		private List<Model> models = new List<Model>();

		private List<Tuple<VAO, Model>> geometries = new List<Tuple<VAO, Model>>();

		private CameraOrbit camera = new CameraOrbit();
		private ShaderFileDebugger shaderWatcher;

		private VAO groundVAO;

		public SunMoon SunMoon { get; set; }
		
		public MainVisual()
		{
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


			Mesh groundMesh = new Mesh();
			groundMesh.Add(Obj2Mesh.FromObj(Resources.nyra));
			groundVAO = CreateVAO(groundMesh, shaderWatcher.Shader);

			Model groundModel = new Model();
			groundModel.ObjectMesh = groundMesh;

			groundModel.DiffuseTexture = models[0].DiffuseTexture;

			//models.Add(groundModel);

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

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			var shader = shaderWatcher.Shader;
			shader.Begin();

			Matrix4 cam = camera.CalcMatrix();
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref cam);
			GL.Uniform3(shader.GetUniformLocation("cameraPosition"), camera.CalcPosition());

			//GL.Uniform1(shader.GetUniformLocation("texN"), texture_n.ID);
			//GL.Uniform1(shader.GetUniformLocation("texS"), texture_s.ID);

			GL.Uniform3(shader.GetUniformLocation("lightDirection"), SunMoon.GetLightDirection());
			GL.Uniform3(shader.GetUniformLocation("lightColor"), SunMoon.GetLightColor());

			if (shaderWatcher.CheckForShaderChange())
			{
				Console.WriteLine("TEST");

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

			shader.End();
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
