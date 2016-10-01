using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace DemoScene.DemoObjects
{
	class PhysicalBody : IPhysical
	{
		public Vector3 Position { get; set; }
		public float Mass { get; set; }
		public bool DoPhysics { get; set; }

		public bool Bounce { get; set; }
		public float BounceFactor { get; set; }

		public Vector3 velocity = Vector3.Zero;
		public Vector3 acceleration = Vector3.Zero;

		public PhysicalBody(Vector3 position, float mass)
		{
			Position = position;
			Mass = mass;

			DoPhysics = true;
			Bounce = false;
			BounceFactor = 0.9f;
		}

		public void SetNewPosition(Vector3 newPosition)
		{
			if (newPosition.Y <= 0)
			{
				Position = new Vector3(Position.X, 0, Position.Z);

				velocity = Bounce ? (new Vector3(velocity.X, -velocity.Y, velocity.Z) * BounceFactor) : Vector3.Zero;
			}
			else
			{
				Position = newPosition;
			}
		}

		public void AddForce(Vector3 force)
		{
			acceleration += (force / Mass);
		}

		public void ApplyForces()
		{
			velocity += acceleration;

			SetNewPosition(Position + velocity);

			acceleration = Vector3.Zero;
		}

		public void ResetForces()
		{
			velocity = Vector3.Zero;
			acceleration = Vector3.Zero;
		}

	}
}
