using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.DemoObjects
{
	class MovingTetrahedron
	{

		private TetrahedronSphere sphere;

		public Vector3 Position { get; set; }
		public Vector3 CurrentDestiny { get; set; }
		public Vector3 NextDestiny { get; set; }

		public float PlayerDistance { get; set; }


		public MovingTetrahedron(TetrahedronSphere sphere)
		{
			this.sphere = sphere;
		}

		public void CalculatePlayerDistance(Vector3 playerPosition)
		{
			PlayerDistance = (Position - playerPosition).Length;
		}

		public void Move()
		{
			if (IsCloseEnoughToDestiny())
			{
				CurrentDestiny = NextDestiny;
				NextDestiny = sphere.GetRandomVectorInSphere();
			}

			Vector3 vectorToDestiny = GetVectorToDestiny();
			vectorToDestiny.Normalize();

			Position += vectorToDestiny * GetDistanceFactor() * 0.15f;
		}

		private bool IsCloseEnoughToDestiny()
		{
			return GetVectorToDestiny().Length < 0.05f;
		}

		private Vector3 GetVectorToDestiny()
		{
			return CurrentDestiny - Position;
		}

		private float GetDistanceFactor()
		{
			return MathHelper.Clamp(1 - ((PlayerDistance - 4) / 16), 0.1f, 1);
		}

		public float GetScale()
		{
			return (GetDistanceFactor() + 0.1f) * 1.0f;
		}

		public Vector3 GetColor()
		{
			return new Vector3(GetDistanceFactor(), (1 - GetDistanceFactor()) / 2, 1 - GetDistanceFactor()) * 4;
		}

	}
}
