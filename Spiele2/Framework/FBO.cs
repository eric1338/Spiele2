using OpenTK.Graphics.OpenGL;
using System;

namespace Framework
{
	[Serializable]
	public class FBOException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FBOException"/> class.
		/// </summary>
		/// <param name="msg">The error msg.</param>
		public FBOException(string msg) : base(msg) { }
	}

	public class FBO : IDisposable
	{
		public FBO()
		{
			// Create an FBO object
			GL.GenFramebuffers(1, out m_FBOHandle);
		}

		public void BeginUse(Texture texture)
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, this.m_FBOHandle);
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texture.ID, 0);
			string status = GetStatusMessage();
			if (!string.IsNullOrEmpty(status))
			{
				EndUse();
				throw new FBOException(status);
			}
			GL.Viewport(0, 0, texture.Width, texture.Height);
		}

		public void EndUse()
		{
			GL.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
		}

		public void Dispose()
		{
			GL.DeleteFramebuffers(1, ref this.m_FBOHandle);
		}

		private uint m_FBOHandle = 0;

		private string GetStatusMessage()
		{
			switch (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer))
			{
				case FramebufferErrorCode.FramebufferComplete: return string.Empty;
				case FramebufferErrorCode.FramebufferIncompleteAttachment: return "One or more attachment points are not framebuffer attachment complete. This could mean there’s no texture attached or the format isn’t renderable. For color textures this means the base format must be RGB or RGBA and for depth textures it must be a DEPTH_COMPONENT format. Other causes of this error are that the width or height is zero or the z-offset is out of range in case of render to volume.";
				case FramebufferErrorCode.FramebufferIncompleteMissingAttachment: return "There are no attachments.";
				case FramebufferErrorCode.FramebufferIncompleteDimensionsExt: return "Attachments are of different size. All attachments must have the same width and height.";
				case FramebufferErrorCode.FramebufferIncompleteFormatsExt: return "The color attachments have different format. All color attachments must have the same format.";
				case FramebufferErrorCode.FramebufferIncompleteDrawBuffer: return "An attachment point referenced by GL.DrawBuffers() doesn’t have an attachment.";
				case FramebufferErrorCode.FramebufferIncompleteReadBuffer: return "The attachment point referenced by GL.ReadBuffers() doesn’t have an attachment.";
				case FramebufferErrorCode.FramebufferUnsupported: return "This particular FBO configuration is not supported by the implementation.";
				default: return "Status unknown. (yes, this is really bad.)";
			}
		}
	}
}
