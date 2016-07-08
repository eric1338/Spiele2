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

			//List<Vector2> uvs = CreateUVs(2, 2);

			List<uint> ids = CreateIDs(2, 2, renderBack);

			return CreateMyVao(shader, positions, normals, uvs, ids);
		}

		public VAO CreateFlagVao(Shader shader)
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
					posis.Add(new Vector3(i * 0.25f, j * 0.125f, 0));
					normals.Add(new Vector3(0, 0, 1));
				}
			}

			List<Vector2> uvs = CreateUVs(pointRows, pointsPerRow);
			List<uint> ids = CreateIDs(pointRows, pointsPerRow);

			return CreateMyVao(shader, posis, normals, uvs, ids);
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
