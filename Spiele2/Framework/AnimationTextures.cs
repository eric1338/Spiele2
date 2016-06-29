using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace Framework
{
	public class AnimationTextures : IAnimation
	{
		public AnimationTextures(float animationLength)
		{
			this.AnimationLength = animationLength;
		}

		public void AddFrame(Texture textureFrame)
		{
			textures.Add(textureFrame);
		}

		public float AnimationLength { get; set; }

		public IList<Texture> Textures
		{
			get
			{
				return textures;
			}
		}

		/// <summary>
		/// Calculates the frame id (the current frame of the animation) out of the given time
		/// </summary>
		/// <param name="time"></param>
		/// <returns>id of the current frame of the animation</returns>
		public uint CalcAnimationFrame(float time)
		{
			float normalizedDeltaTime = (time % AnimationLength) / AnimationLength;
			double idF = normalizedDeltaTime * (textures.Count - 1);
			uint id = (uint) Math.Max(0, Math.Round(idF));
			return id;
		}

		/// <summary>
		/// draws a GL quad, textured with an animation.
		/// </summary>
		/// <param name="rectangle">coordinates ofthe GL quad</param>
		/// <param name="totalSeconds">animation position in seconds</param>
		public void Draw(AABR rectangle, float totalSeconds)
		{
			var id = (int)CalcAnimationFrame(totalSeconds);
			textures[id].BeginUse();
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0); GL.Vertex2(rectangle.X, rectangle.Y);
			GL.TexCoord2(1, 0); GL.Vertex2(rectangle.MaxX, rectangle.Y);
			GL.TexCoord2(1, 1); GL.Vertex2(rectangle.MaxX, rectangle.MaxY);
			GL.TexCoord2(0, 1); GL.Vertex2(rectangle.X, rectangle.MaxY);
			GL.End();
			textures[id].EndUse();
		}

		private List<Texture> textures = new List<Texture>();
	}
}
