using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace DemoScene.DemoObjects
{
	class Rabbit : PhysicalBody
	{

		public float Rotation = 0f;

		public Rabbit() : base(Vector3.Zero, 1.3f)
		{
			DoPhysics = true;
			Bounce = false;
		}

		public Vector3 GetJumpDirection()
		{
			float x = (float) Math.Cos(Rotation + 1.57f);
			float z = (float) Math.Sin(Rotation + 1.57f);

			Vector3 jumpDirection = new Vector3(x, 1.5f, z);

			jumpDirection.Normalize();

			return jumpDirection;
		}

	}
}
