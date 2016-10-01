using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoScene.DemoObjects;
using OpenTK;
using Framework;
using OpenTK.Graphics.OpenGL;

namespace DemoScene.Visual.MyVisuals
{
	class ParticleSystemVisual : MyVisual
	{

		private Shader smokeShader;

		private VAO particlesVAO;
		private Texture smokeTexture;

		public ParticleSystemVisual(MainVisual mainVisual, Models models, DemoLevel demoLevel) : base(mainVisual, models, demoLevel)
		{
			smokeShader = CreateShader(Resources.smokevertex, Resources.smokefragment);

			particlesVAO = new VAO();
			smokeTexture = Textures.Instance.Smoke;
		}


		public override void Render(Matrix4 camera)
		{
			List<Particle> particles = demoLevel.ParticleSystem.Particles;

			Vector3[] positions = new Vector3[particles.Count];
			float[] alphas = new float[particles.Count];

			int i = 0;

			foreach (Particle particle in particles)
			{
				positions[i] = particle.Position;
				alphas[i] = particle.Alpha;

				i++;
			}

			particlesVAO.SetAttribute(smokeShader.GetAttributeLocation("position"), positions, VertexAttribPointerType.Float, 3);
			particlesVAO.SetAttribute(smokeShader.GetAttributeLocation("fade"), alphas, VertexAttribPointerType.Float, 1);


			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
			GL.BlendEquation(BlendEquationMode.FuncAdd);
			GL.DepthMask(false);
			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.PointSprite);
			GL.Enable(EnableCap.VertexProgramPointSize);

			smokeShader.Begin();
			GL.UniformMatrix4(smokeShader.GetUniformLocation("camera"), true, ref camera);
			//GL.Uniform1(shader.GetUniformLocation("texParticle"), 0);
			smokeTexture.BeginUse();
			particlesVAO.DrawArrays(PrimitiveType.Points, particles.Count);
			smokeTexture.EndUse();
			smokeShader.End();

			GL.Disable(EnableCap.VertexProgramPointSize);
			GL.Disable(EnableCap.PointSprite);
			GL.Disable(EnableCap.Blend);
			GL.DepthMask(true);
		}
	}
}
