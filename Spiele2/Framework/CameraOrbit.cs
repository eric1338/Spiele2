using OpenTK;

namespace Framework
{
	public class CameraOrbit
	{
		public CameraOrbit()
		{
			Aspect = 1;
			Distance = 1;
			FarClip = 1;
			FovY = 90;
			Heading = 0;
			NearClip = 0.1f;
			Target = Vector3.Zero;
			Tilt = 0;
		}

		public float Aspect { get; set; }
		public float Distance { get; set; }
		public float FarClip { get; set; }
		public float FovY { get; set; }
		public float Heading { get; set; }
		public float NearClip { get; set; }
		public Vector3 Target { get; set; }
		public float Tilt { get; set; }

		public Matrix4 CalcViewMatrix()
		{
			Distance = MathHelper.Clamp(Distance, NearClip, FarClip);
			var mtxDistance = Matrix4.Transpose(Matrix4.CreateTranslation(0, 0, -Distance));
			var mtxTilt = Matrix4.Transpose(Matrix4.CreateRotationX(Tilt));
			var mtxHeading = Matrix4.Transpose(Matrix4.CreateRotationY(Heading));
			var mtxTarget = Matrix4.Transpose(Matrix4.CreateTranslation(-Target));
			return mtxDistance * mtxTilt * mtxHeading * mtxTarget;
		}

		public Matrix4 CalcProjectionMatrix()
		{
			FovY = MathHelper.Clamp(FovY, 0.1f, 180);
			return Matrix4.Transpose(Matrix4.CreatePerspectiveFieldOfView(
				MathHelper.DegreesToRadians(FovY),
				Aspect, NearClip, FarClip));
		}

		public Matrix4 CalcMatrix()
		{
			return CalcProjectionMatrix() * CalcViewMatrix();
		}

		public Vector3 CalcPosition()
		{
			var view = CalcViewMatrix();
			view.Invert();
			return view.Column3.Xyz;
		}
	}
}
