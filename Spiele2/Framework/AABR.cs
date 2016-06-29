namespace Framework
{
	/// <summary>
	/// Represents an 2D axis aligned bounding box - naming it rectangle would have been too simple ;)
	/// </summary>
	public class AABR
	{
		/// <summary>
		/// creates an AABR
		/// </summary>
		/// <param name="x">left side x coordinate</param>
		/// <param name="y">bottom side y coordinate</param>
		/// <param name="sizeX">width</param>
		/// <param name="sizeY">height</param>
		public AABR(float x, float y, float sizeX, float sizeY)
		{
			this.X = x;
			this.Y = y;
			this.SizeX = sizeX;
			this.SizeY = sizeY;
		}

		public AABR(AABR rectangle)
		{
			this.X = rectangle.X;
			this.Y = rectangle.Y;
			this.SizeX = rectangle.SizeX;
			this.SizeY = rectangle.SizeY;
		}

		public float SizeX { get; set; }

		public float SizeY { get; set; }

		public float X { get; set; }

		public float Y { get; set; }

		public float CenterX { get { return X + 0.5f * SizeX; } set { X = value - 0.5f * SizeX; } }

		public float CenterY { get { return Y + 0.5f * SizeY; } set { Y = value - 0.5f * SizeY; } }

		public static AABR CreateFromCenterSize(float centerX, float centerY, float sizeX, float sizeY)
		{
			var rectangle = new AABR(0, 0, sizeX, sizeY);
			rectangle.CenterX = centerX;
			rectangle.CenterY = centerY;
			return rectangle;
		}

		public bool Intersects(AABR rectangle)
		{
			if (null == rectangle) return false;
			bool noXintersect = (MaxX < rectangle.X) || (X > rectangle.MaxX);
			bool noYintersect = (MaxY < rectangle.Y) || (Y > rectangle.MaxY);
			return !(noXintersect || noYintersect);
		}

		public bool Inside(AABR rectangle)
		{
			if (X < rectangle.X) return false;
			if (MaxX > rectangle.MaxX) return false;
			if (Y < rectangle.Y) return false;
			if (MaxY > rectangle.MaxY) return false;
			return true;
		}

		public float MaxX { get { return X + SizeX; } set { X = value - SizeX; } }

		public float MaxY { get { return Y + SizeY; } set { Y = value - SizeY; } }

		public override string ToString()
		{
			return '(' + X.ToString() + ';' + Y.ToString() + ';' + SizeX.ToString() + ';' + SizeY.ToString() + ')';
		}

	}
}
