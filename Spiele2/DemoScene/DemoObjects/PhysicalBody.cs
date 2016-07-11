﻿using System;
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

			DoPhysics = false;
			Bounce = false;
			BounceFactor = 1;
		}

		public void SetNewPosition(Vector3 newPosition)
		{
			if (newPosition.Y <= 0)
			{
				//Position = new Vector3(newPosition.X, 0, newPosition.Z);
				Position = new Vector3(Position.X, 0, Position.Z);

				if (Bounce)
				{
					velocity = new Vector3(velocity.X, -velocity.Y, velocity.Z) * BounceFactor;
				}
				else
				{
					velocity = Vector3.Zero;
					//DoPhysics = false;
				}
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

	}
}