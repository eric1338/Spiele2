using OpenTK.Graphics.OpenGL;

namespace Framework
{
	public class SpriteSheet
	{
		public SpriteSheet(Texture tex, uint spritesPerLine
			, float spriteBoundingBoxWidth = 1.0f, float spriteBoundingBoxHeight = 1.0f)
		{
			this.tex = tex;
			this.tex.FilterTrilinear();
			this.spritesPerLine = spritesPerLine;
			this.spriteBoundingBoxWidth = spriteBoundingBoxWidth;
			this.spriteBoundingBoxHeight = spriteBoundingBoxHeight;
		}

		public AABR CalcSpriteTexCoords(uint spriteID)
		{
			return CalcSpriteTexCoords(spriteID, SpritesPerLine, SpriteBoundingBoxWidth, SpriteBoundingBoxHeight);
		}

		public static AABR CalcSpriteTexCoords(uint spriteID, uint spritesPerLine
			, float spriteBoundingBoxWidth = 1.0f, float spriteBoundingBoxHeight = 1.0f)
		{
			uint row = spriteID / spritesPerLine;
			uint col = spriteID % spritesPerLine;

			float centerX = (col + 0.5f) / spritesPerLine;
			float centerY = 1.0f - (row + 0.5f) / spritesPerLine;
			float height = spriteBoundingBoxHeight / spritesPerLine;
			float width = spriteBoundingBoxWidth / spritesPerLine;
			return new AABR(centerX - 0.5f * width, centerY - 0.5f * height, width, height);
		}

		public void BeginUse()
		{
			tex.BeginUse();
		}

		public void EndUse()
		{
			tex.EndUse();
		}

		public void Draw(uint spriteID, AABR rectangle)
		{
			AABR texCoords = CalcSpriteTexCoords(spriteID);
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(texCoords.X, texCoords.Y); GL.Vertex2(rectangle.X, rectangle.Y);
			GL.TexCoord2(texCoords.MaxX, texCoords.Y); GL.Vertex2(rectangle.MaxX, rectangle.Y);
			GL.TexCoord2(texCoords.MaxX, texCoords.MaxY); GL.Vertex2(rectangle.MaxX, rectangle.MaxY);
			GL.TexCoord2(texCoords.X, texCoords.MaxY); GL.Vertex2(rectangle.X, rectangle.MaxY);
			GL.End();
		}

		public float SpriteBoundingBoxWidth
		{
			get
			{
				return spriteBoundingBoxWidth;
			}
		}

		public float SpriteBoundingBoxHeight
		{
			get
			{
				return spriteBoundingBoxHeight;
			}
		}

		public uint SpritesPerLine
		{
			get
			{
				return spritesPerLine;
			}
		}

		public Texture Tex
		{
			get
			{
				return tex;
			}
		}

		private readonly float spriteBoundingBoxWidth;
		private readonly float spriteBoundingBoxHeight;
		private readonly uint spritesPerLine;
		private readonly Texture tex;
	}
}
