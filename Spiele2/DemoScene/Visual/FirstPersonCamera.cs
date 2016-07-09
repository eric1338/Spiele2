using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.Visual
{
	class FirstPersonCamera
	{

		private Vector3 position;
		private float upAngle = 0;
		private float sideAngle = 0;

		public bool FixY { get; set; } = true;

		private float moveSpeedFactor = 0.1f;
		private float fixedYValue = -3f;


		public FirstPersonCamera()
		{
			position = new Vector3(0, fixedYValue, 0);
		}


		private Vector3 GetLookDirection()
		{
			float x = (float) -Math.Cos(sideAngle + 1.57f);
			float y = (float) -Math.Sin(upAngle);
			float z = (float) -Math.Sin(sideAngle + 1.57f);

			return new Vector3(x, y, z);
		}

		private void Move(Vector3 direction)
		{
			Vector3 moveVector = FixY ? new Vector3(direction.X, 0, direction.Z) : direction;

			moveVector.Normalize();

			position -= moveVector * moveSpeedFactor;
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

		public void ChangeTarget(float horizontalDelta, float verticalDelta)
		{
			sideAngle += horizontalDelta;
			upAngle = MathHelper.Clamp(upAngle + verticalDelta, -1.57f, 1.57f);
		}

		public void ToggleFixY()
		{
			FixY = !FixY;

			if (FixY) position.Y = fixedYValue;
		}

		public Matrix4 GetMatrix()
		{
			Matrix4 downRotation = Matrix4.Transpose(Matrix4.CreateRotationY(sideAngle));
			Matrix4 sideRotation = Matrix4.Transpose(Matrix4.CreateRotationX(upAngle));
			Matrix4 translation = Matrix4.Transpose(Matrix4.CreateTranslation(position));
			Matrix4 perspective = Matrix4.Transpose(Matrix4.CreatePerspectiveFieldOfView(1.2f, 16f / 9f, 0.1f, 200));

			return perspective * sideRotation * downRotation * translation;
		}
	}
}
