using DemoScene.Utils;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.DemoObjects
{
	class TetrahedronSphere
	{

		public List<MovingTetrahedron> Tetrahedrons = new List<MovingTetrahedron>();

		public Vector3 Position;

		public int TetrahedronCount { get; set; }
		public float TetrahedronSize { get; set; }

		private float sphereSize;


		public TetrahedronSphere()
		{
			TetrahedronCount = 400;
			TetrahedronSize = 0.6f;

			Position = new Vector3(-12, 4, 12);

			sphereSize = 4f;

			InitTetrahedrons();
		}

		private void InitTetrahedrons()
		{
			for (int i = 0; i < TetrahedronCount; i++)
			{
				MovingTetrahedron tetrahedron = new MovingTetrahedron(this);

				tetrahedron.Position = GetRandomVectorInSphere();
				tetrahedron.CurrentDestiny = GetRandomVectorInSphere();
				tetrahedron.NextDestiny = GetRandomVectorInSphere();
				tetrahedron.PlayerDistance = 1000;

				Tetrahedrons.Add(tetrahedron);
			}
		}


		public Vector3 GetRandomVectorInSphere()
		{
			return Position + Util.GetRandomVector3() * sphereSize;
		}

		public void Tick(Vector3 playerPosition)
		{
			foreach (MovingTetrahedron tetrahedron in Tetrahedrons)
			{
				tetrahedron.CalculatePlayerDistance(playerPosition);
				tetrahedron.Move();
			}
		}

	}
}
