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

		public MainVisual()
		{
			shaderWatcher = new ShaderFileDebugger("../../DemoScene/Resources/vertex.vert"
				, "../../DemoScene/Resources/fragment.frag"
				, Resources.vertex, Resources.fragment);
			geometry = CreateMesh(shaderWatcher.Shader);

			camera.FarClip = 200;
			camera.Distance = 5;
			camera.FovY = 30;

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
		}

		public void Render()
		{
			if (shaderWatcher.CheckForShaderChange())
			{
				//update geometry when shader changes
				geometry = CreateMesh(shaderWatcher.Shader);
			}
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			
			
			var shader = shaderWatcher.Shader;
			shader.Begin();
			GL.Uniform3(shader.GetUniformLocation("light1Direction"), new Vector3(-1, -1, -1).Normalized());
			GL.Uniform4(shader.GetUniformLocation("light1Color"), new Color4(1f, 1f, 1f, 1f));
			GL.Uniform3(shader.GetUniformLocation("light2Position"), new Vector3(-1, -1, 1));
			GL.Uniform4(shader.GetUniformLocation("light2Color"), new Color4(1f, .1f, .1f, 1f));
			GL.Uniform3(shader.GetUniformLocation("light3Position"), new Vector3(-2, 2, 2));
			GL.Uniform3(shader.GetUniformLocation("light3Direction"), new Vector3(1, -1, -1).Normalized());
			GL.Uniform1(shader.GetUniformLocation("light3Angle"), MathHelper.DegreesToRadians(10f));
			GL.Uniform4(shader.GetUniformLocation("light3Color"), new Color4(0, 0, 1f, 1f));
			GL.Uniform4(shader.GetUniformLocation("ambientLightColor"), new Color4(.1f, .1f, .1f, 1f));
			GL.Uniform4(shader.GetUniformLocation("materialColor"), new Color4(.7f, .9f, .7f, 1f));
			Matrix4 cam = camera.CalcMatrix();
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref cam);
			GL.Uniform3(shader.GetUniformLocation("cameraPosition"), camera.CalcPosition());
			geometry.Draw();
			shader.End();

			//GL.Color3(1f, 1f, 1f);
			//GL.Begin(PrimitiveType.Quads);
			//GL.Vertex3(10, -1, 10);
			//GL.Vertex3(10, -1, -10);
			//GL.Vertex3(-10, -1, -10);
			//GL.Vertex3(-10, -1, 10);
			//GL.End();
		}

		private CameraOrbit camera = new CameraOrbit();
		private ShaderFileDebugger shaderWatcher;
		private VAO geometry;

		private static VAO CreateMesh(Shader shader)
		{
			Mesh mesh = new Mesh();
			//mesh.Add(Meshes.CreateSphere(.9f, 4));
			mesh.Add(Obj2Mesh.FromObj(Resources.suzanne));
			var vao = new VAO();
			vao.SetAttribute(shader.GetAttributeLocation("position"), mesh.positions.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetAttribute(shader.GetAttributeLocation("normal"), mesh.normals.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetID(mesh.ids.ToArray(), PrimitiveType.Triangles);
			return vao;
		}

	}
}
