using DemoScene.DemoObjects;
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

		private Player player;
		
		private float upAngle = 0;
		private float sideAngle = 0;
		
		private float playerHeight = 3f;

		public FirstPersonCamera(Player player)
		{
			this.player = player;
		}

		public Vector3 GetEyePosition()
		{
			return player.Position + new Vector3(0, playerHeight, 0);
		}

		private bool FixY()
		{
			return !player.IsFlying;
		}

		private Vector3 GetLookDirection()
		{
			float x = (float) -Math.Cos(sideAngle + 1.57f);
			float y = (float) -Math.Sin(upAngle);
			float z = (float) -Math.Sin(sideAngle + 1.57f);

			return new Vector3(x, y, z);
		}

		public Vector3 GetForwardsVector()
		{
			return GetLookDirection();
		}

		public Vector3 GetBackwardsVector()
		{
			return -GetLookDirection();
		}

		public Vector3 GetLeftVector()
		{
			Vector3 lookDirection = GetLookDirection();

			return new Vector3(lookDirection.Z, lookDirection.Y, -lookDirection.X);
		}

		public Vector3 GetRightVector()
		{
			Vector3 lookDirection = GetLookDirection();

			return new Vector3(-lookDirection.Z, lookDirection.Y, lookDirection.X);
		}

		public void ChangeTarget(float horizontalDelta, float verticalDelta)
		{
			sideAngle += horizontalDelta;
			upAngle = MathHelper.Clamp(upAngle + verticalDelta, -1.57f, 1.57f);
		}

		public Matrix4 GetMatrix()
		{
			Matrix4 downRotation = Matrix4.Transpose(Matrix4.CreateRotationY(sideAngle));
			Matrix4 sideRotation = Matrix4.Transpose(Matrix4.CreateRotationX(upAngle));
			Matrix4 translation = Matrix4.Transpose(Matrix4.CreateTranslation(-GetEyePosition()));
			Matrix4 perspective = Matrix4.Transpose(Matrix4.CreatePerspectiveFieldOfView(1.2f, 16f / 9f, 0.1f, 200));

			return perspective * sideRotation * downRotation * translation;
		}
	}
}
