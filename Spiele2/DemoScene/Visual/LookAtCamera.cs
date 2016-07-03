using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.Visual
{
	class LookAtCamera
	{

		private Vector3 position;
		private Vector3 target;

		public bool FixY { get; set; }

		private float moveSpeedFactor = 0.01f;
		private float fixedYValue = 2f;


		public LookAtCamera()
		{
			position = new Vector3(0, fixedYValue, 10);
			target = new Vector3(0, fixedYValue, 0);
		}


		private Vector3 GetLookDirection()
		{
			return target - position;
		}

		private void Move(Vector3 direction)
		{
			Vector3 moveVector3 = FixY ? new Vector3(direction.X, fixedYValue, direction.Z) : direction;

			position += direction * moveSpeedFactor;
			target += direction * moveSpeedFactor;
		}

		public void MoveForwards()
		{
			Move(GetLookDirection());
		}

		public void MoveBackwards()
		{
			Move(-GetLookDirection());
		}

		public void MoveLeft()
		{
			Vector3 lookDirection = GetLookDirection();

			Move(new Vector3(lookDirection.Z, lookDirection.Y, -lookDirection.X));
		}

		public void MoveRight()
		{
			Vector3 lookDirection = GetLookDirection();

			Move(new Vector3(-lookDirection.Z, lookDirection.Y, lookDirection.X));
		}

		public Matrix4 GetMatrix()
		{
			Console.WriteLine(position);

			Matrix4 camera = Matrix4.LookAt(position, target, new Vector3(0, 1, 0));

			camera.Transpose();

			return camera;
		}


	}
}
