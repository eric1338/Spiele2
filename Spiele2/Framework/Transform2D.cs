using OpenTK;

namespace Framework
{
	public static class Transform2D
	{
		public static Matrix3 RotateAroundOrigin(float angle)
		{
			//Matrix3 does the rotation for row vectors 
			//-> for column vectors we need the transpose = the inverse for orthonormal
			return Matrix3.CreateRotationZ(-angle);
		}

		public static Matrix3 RotateAround(float pivotX, float pivotY, float angle)
		{
			var Cinv = Transform2D.Translate(-pivotX, -pivotY);
			var R = Transform2D.RotateAroundOrigin(angle);
			var C = Transform2D.Translate(pivotX, pivotY);
			return C * R * Cinv;
		}

		public static Matrix3 ScaleAroundOrigin(float scaleX, float scaleY)
		{
			return new Matrix3(scaleX, 0, 0,
								  0, scaleY, 0,
								  0, 0, 1);
		}

		public static Matrix3 ScaleAround(float pivotX, float pivotY, float scaleX, float scaleY)
		{
			var Cinv = Transform2D.Translate(-pivotX, -pivotY);
			var S = Transform2D.ScaleAroundOrigin(scaleX, scaleY);
			var C = Transform2D.Translate(pivotX, pivotY);
			return C * S * Cinv;
		}

		public static Matrix3 Translate(float tx, float ty)
		{
			return new Matrix3( 1, 0, tx,
								0, 1, ty,
								0, 0, 1);
		}

		public static Vector2 Transform(this Matrix3 M, float x, float y)
		{
			var pos = new Vector3(x, y, 1);
			return new Vector2(Vector3.Dot(pos, M.Row0), Vector3.Dot(pos, M.Row1));
		}
		public static Vector2 Transform(this Matrix3 M, Vector2 pos)
		{
			var pos3 = new Vector3(pos.X, pos.Y, 1);
			return new Vector2(Vector3.Dot(pos3, M.Row0), Vector3.Dot(pos3, M.Row1));
		}
	}
}
