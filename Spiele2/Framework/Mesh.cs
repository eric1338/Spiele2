using OpenTK;
using System.Collections.Generic;

namespace Framework
{
	public class Mesh
	{
		public List<Vector3> positions = new List<Vector3>();
		public List<Vector3> normals = new List<Vector3>();
		public List<Vector2> uvs = new List<Vector2>();
		public List<uint> ids = new List<uint>();
	}
}
