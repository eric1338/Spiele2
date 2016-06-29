using OpenTK;
using System.Linq;

namespace Framework
{
	public static class AABRextensions
	{
		public static bool PushXRangeInside(this AABR rectangleA, AABR rectangleB)
		{
			if (rectangleA.SizeX > rectangleB.SizeX) return false;
			if (rectangleA.X < rectangleB.X)
			{
				rectangleA.X = rectangleB.X;
			}
			if (rectangleA.MaxX > rectangleB.MaxX)
			{
				rectangleA.MaxX = rectangleB.MaxX;
			}
			return true;
		}

		public static bool PushYRangeInside(this AABR rectangleA, AABR rectangleB)
		{
			if (rectangleA.SizeY > rectangleB.SizeY) return false;
			if (rectangleA.Y < rectangleB.Y)
			{
				rectangleA.Y = rectangleB.Y;
			}
			if (rectangleA.MaxY > rectangleB.MaxY)
			{
				rectangleA.MaxY = rectangleB.MaxY;
			}
			return true;
		}

		/// <summary>
		/// Calculates the AABR in the overlap
		/// Returns null if no intersection
		/// </summary>
		/// <param name="rectangleB"></param>
		/// <returns>AABR in the overlap</returns>
		public static AABR Overlap(this AABR rectangleA, AABR rectangleB)
		{
			AABR overlap = null;

			if (rectangleA.Intersects(rectangleB))
			{
				overlap = new AABR(0.0f, 0.0f, 0.0f, 0.0f);

				overlap.X = (rectangleA.X < rectangleB.X) ? rectangleB.X : rectangleA.X;
				overlap.Y = (rectangleA.Y < rectangleB.Y) ? rectangleB.Y : rectangleA.Y;

				overlap.SizeX = (rectangleA.MaxX < rectangleB.MaxX) ? rectangleA.MaxX - overlap.X : rectangleB.MaxX - overlap.X;
				overlap.SizeY = (rectangleA.MaxY < rectangleB.MaxY) ? rectangleA.MaxY - overlap.Y : rectangleB.MaxY - overlap.Y;
			}

			return overlap;
		}

		public static void TransformCenter(this AABR rectangle, Matrix3 M)
		{
			var newPos = M.Transform(rectangle.CenterX, rectangle.CenterY);
			rectangle.CenterX = newPos.X;
			rectangle.CenterY = newPos.Y;
		}

		/// <summary>
		/// If an intersection with the frame occurs do the minimal translation to undo the overlap
		/// </summary>
		/// <param name="rectangleB">The AABR to check for intersect</param>
		public static void UndoOverlap(this AABR rectangleA, AABR rectangleB)
		{
			if (rectangleA.Intersects(rectangleB))
			{
				Vector2[] directions = new Vector2[]
				{
					new Vector2(rectangleB.MaxX - rectangleA.X, 0),
					new Vector2(rectangleB.X - rectangleA.MaxX, 0),
					new Vector2(0, rectangleB.MaxY - rectangleA.Y),
					new Vector2(0, rectangleB.Y - rectangleA.MaxY)
				};

				Vector2 minimum = directions.Aggregate((curMin, x) => (curMin == null || (x.Length) < curMin.Length) ? x : curMin);

				rectangleA.X += minimum.X;
				rectangleA.Y += minimum.Y;
			}
		}
	}
}
