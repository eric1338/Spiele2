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

		public VAO CreateFromMesh(Shader shader, Mesh mesh)
		{
			return CreateMyVAO(shader, mesh.positions, mesh.normals, mesh.uvs, mesh.ids);
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

		public VAO CreateFlagVao(Shader shader)
		{
			List<Vector3> posis = new List<Vector3>();
			List<Vector3> normals = new List<Vector3>();

			Vector3 bottomLeft = Vector3.Zero;
			Vector3 topRight = Vector3.Zero;

			uint pointsPerRow = 1000;
			uint pointRows = 1000;

			for (int i = 0; i < pointRows; i++)
			{
				for (int j = 0; j < pointsPerRow; j++)
				{
					float z = 0;

					posis.Add(new Vector3(i * 0.05f, j * 0.025f, z * 0.2f));
					normals.Add(new Vector3(0, 0, 1));
				}
			}

			List<Vector2> uvs = CreateUVs(pointRows, pointsPerRow);
			List<uint> ids = CreateIDs(pointRows, pointsPerRow);

			return CreateMyVAO(shader, posis, normals, uvs, ids);
		}

		private VAO CreateMyVAO(Shader shader, List<Vector3> positions, List<Vector3> normals, List<Vector2> uvs, List<uint> ids)
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
