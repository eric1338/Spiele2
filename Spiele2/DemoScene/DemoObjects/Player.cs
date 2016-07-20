using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.DemoObjects
{
	class Player : PhysicalBody
	{

		public bool IsFlying { get; set; }

		private float moveSpeedFactor = 0.14f;


		public Player() : base(Vector3.Zero, 60)
		{
			DoPhysics = true;
			Bounce = false;

			IsFlying = false;
		}

		public void Move(Vector3 direction)
		{
			if (direction == Vector3.Zero) return;

			Vector3 moveVector = IsFlying ? direction : new Vector3(direction.X, 0, direction.Z);

			moveVector.Normalize();

			Position += moveVector * moveSpeedFactor;
		}

		public void ToggleFlying()
		{
			IsFlying = !IsFlying;

			if (!IsFlying) Position = new Vector3(Position.X, 0, Position.Z);

			DoPhysics = !IsFlying;
		}

	}
}
