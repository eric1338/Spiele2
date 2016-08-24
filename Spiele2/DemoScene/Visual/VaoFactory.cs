using DemoScene.Utils;
using Framework;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.Visual
{
	class VaoFactory
	{

		public static VaoFactory Instance { get; private set; } = new VaoFactory();

		public VAO CreateFromObjectData(Shader shader, byte[] objectData)
		{
			Mesh mesh = Obj2Mesh.FromObj(objectData);

			return CreateFromMesh(shader, mesh);
		}

		public VAO CreateFromMesh(Shader shader, Mesh mesh)
		{
			return CreateMyVao(shader, mesh.positions, mesh.normals, mesh.uvs, mesh.ids);
		}

		public VAO CreateCuboid(Shader shader, Vector3 a, Vector3 g)
		{
			Vector3 b = new Vector3(a.X, a.Y, g.Z);
			Vector3 c = new Vector3(g.X, a.Y, g.Z);
			Vector3 d = new Vector3(g.X, a.Y, a.Z);

			Vector3 e = new Vector3(a.X, g.Y, a.Z);
			Vector3 f = new Vector3(g.X, g.Y, a.Z);
			Vector3 h = new Vector3(a.X, g.Y, g.Z);

			List<Vector3> positions = new List<Vector3>();
			positions.Add(a);
			positions.Add(b);
			positions.Add(c);
			positions.Add(d);
			positions.Add(e);
			positions.Add(f);
			positions.Add(g);
			positions.Add(h);

			List<Vector3> normals = new List<Vector3>();

			List<Vector2> uvs = new List<Vector2>();

			for (int i = 0; i < 8; i++) uvs.Add(new Vector2(0, 0));

			List<uint> ids = new List<uint>();

			return CreateMyVao(shader, positions, normals, uvs, ids);
		}

		public VAO Create2DVao(Shader shader, Vector3 a, Vector3 b, Vector3 c, Vector3 d, bool renderBack = true)
		{
			List<Vector3> positions = new List<Vector3>();

			positions.Add(a);
			positions.Add(b);
			positions.Add(d);
			positions.Add(c);

			List<Vector3> normals = new List<Vector3>();

			Vector3 normal = -Vector3.Cross(b - a, c - b);

			for (int i = 0; i < 3; i++) normals.Add(normal);

			List<Vector2> uvs = new List<Vector2>();
			uvs.Add(new Vector2(0, 0));
			uvs.Add(new Vector2(1, 0));
			uvs.Add(new Vector2(0, 1));
			uvs.Add(new Vector2(1, 1));

			List<uint> ids = CreateIDs(2, 2, renderBack);

			return CreateMyVao(shader, positions, normals, uvs, ids);
		}

		public VAO CreateFlagVao(Shader shader)
		{
			List<Vector3> positions = new List<Vector3>();
			List<Vector3> normals = new List<Vector3>();

			Vector3 bottomLeft = Vector3.Zero;
			Vector3 topRight = Vector3.Zero;

			uint pointsPerRow = 200;
			uint pointRows = 200;

			for (int i = 0; i < pointRows; i++)
			{
				for (int j = 0; j < pointsPerRow; j++)
				{
					positions.Add(new Vector3(i * 0.25f, j * 0.125f, 0));
					normals.Add(new Vector3(0, 0, 1));
				}
			}

			List<Vector2> uvs = CreateUVs(pointRows, pointsPerRow);
			List<uint> ids = CreateIDs(pointRows, pointsPerRow);

			return CreateMyVao(shader, positions, normals, uvs, ids);
		}

		public VAO CreateTetrahedron(Shader shader)
		{
			List<Vector3> positions = new List<Vector3>();

			float halfSize = 0.5f;
			float x = halfSize * 0.85f;

			positions.Add(new Vector3(-halfSize, -x, -x));
			positions.Add(new Vector3(halfSize, -x, -x));
			positions.Add(new Vector3(0, -x, x));
			positions.Add(new Vector3(0, x, -0.3f * halfSize));

			List<Vector3> normals = new List<Vector3>();
			List<Vector2> uvs = new List<Vector2>();
			for (int i = 0; i < 4; i++)
			{
				normals.Add(new Vector3(0, 1, 0));
				uvs.Add(new Vector2(0, 0));
			}

			List<uint> ids = new List<uint>();

			ids.Add(0);
			ids.Add(1);
			ids.Add(2);
			ids.Add(2);
			ids.Add(1);
			ids.Add(0);

			ids.Add(0);
			ids.Add(1);
			ids.Add(3);
			ids.Add(3);
			ids.Add(1);
			ids.Add(0);

			ids.Add(0);
			ids.Add(2);
			ids.Add(3);
			ids.Add(3);
			ids.Add(2);
			ids.Add(0);

			ids.Add(1);
			ids.Add(2);
			ids.Add(3);
			ids.Add(3);
			ids.Add(2);
			ids.Add(1);

			return CreateMyVao(shader, positions, normals, uvs, ids);
		}

		private VAO CreateMyVao(Shader shader, List<Vector3> positions, List<Vector3> normals, List<Vector2> uvs, List<uint> ids)
		{
			VAO vao = new VAO();

			int positionLocation = shader.GetAttributeLocation(ShaderAttributes.VertexPosition);
			int normalLocation = shader.GetAttributeLocation(ShaderAttributes.VertexNormal);
			int uvLocation = shader.GetAttributeLocation(ShaderAttributes.VertexUV);

			vao.SetAttribute(positionLocation, positions.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetAttribute(normalLocation, normals.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetAttribute(uvLocation, uvs.ToArray(), VertexAttribPointerType.Float, 2);

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


	}
}
