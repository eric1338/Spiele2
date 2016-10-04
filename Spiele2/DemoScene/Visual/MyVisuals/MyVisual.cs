using DemoScene.DemoObjects;
using Framework;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.Visual.MyVisuals
{
	abstract class MyVisual
	{

		public bool DoRender { get; set; }

		protected MainVisual mainVisual;
		protected Models models;
		protected DemoLevel demoLevel;

		public MyVisual(MainVisual mainVisual, Models models, DemoLevel demoLevel)
		{
			this.mainVisual = mainVisual;
			this.models = models;
			this.demoLevel = demoLevel;

			DoRender = true;
		}

		public abstract void Render(Matrix4 camera);

		public void TryRender(Matrix4 camera)
		{
			if (!DoRender) return;

			Render(camera);
		}

		public void ToggleDoRender()
		{
			DoRender = !DoRender;
		}
		
		protected Shader CreateShader(byte[] vertexShader, byte[] fragmentShader)
		{
			string vertexShaderString = Encoding.UTF8.GetString(vertexShader);
			string fragmentShaderString = Encoding.UTF8.GetString(fragmentShader);

			return ShaderLoader.FromStrings(vertexShaderString, fragmentShaderString);
		}

		protected void SetDefaultVertexUniforms(Shader shader, Matrix4 cam)
		{
			GL.Uniform1(shader.GetUniformLocation("time"), mainVisual.Time);
			GL.UniformMatrix4(shader.GetUniformLocation("camera"), true, ref cam);
			GL.Uniform3(shader.GetUniformLocation("cameraPosition"), mainVisual.FirstPersonCamera.GetEyePosition());
		}

		protected void SetSunMoonUniforms(Shader shader)
		{
			GL.Uniform3(shader.GetUniformLocation("lightDirection"), demoLevel.SunMoon.GetLightDirection());
			GL.Uniform3(shader.GetUniformLocation("lightColor"), demoLevel.SunMoon.GetLightColor());
			GL.Uniform1(shader.GetUniformLocation("ambientFactor"), demoLevel.SunMoon.GetAmbientFactor());
		}

		protected void RenderModel(Shader shader, Model model, bool useSpecularTexture = false)
		{
			RenderSettings renderSettings = model.RenderSettings;

			Matrix3 rotation = Matrix3.CreateRotationY(renderSettings.Rotation);
			GL.UniformMatrix3(shader.GetUniformLocation("instanceRotation"), false, ref rotation);

			GL.Uniform3(shader.GetUniformLocation("instancePosition"), renderSettings.Position);
			GL.Uniform1(shader.GetUniformLocation("instanceScale"), renderSettings.Scale);

			if (renderSettings.UseTexture)
			{
				Texture diffuseTexture = renderSettings.DiffuseTexture;

				GL.Uniform1(shader.GetUniformLocation("specularFactor"), renderSettings.SpecularFactor);

				if (useSpecularTexture)
				{
					int diffuseTextureUniformLocation = shader.GetUniformLocation("diffuseTexture");
					int diffuseTextureID = (int)diffuseTexture.ID;

					UseTexture(diffuseTextureUniformLocation, ref diffuseTextureID, TextureUnit.Texture0);

					Texture specularTexture = renderSettings.SpecularTexture;

					int specularTextureUniformLocation = shader.GetUniformLocation("specularTexture");
					int specularTextureID = (int)specularTexture.ID;

					UseTexture(specularTextureUniformLocation, ref specularTextureID, TextureUnit.Texture1);

					model.Vao.Draw();

					//GL.Disable(EnableCap.Texture2D);
					//GL.BindTexture(TextureTarget.Texture2D, 1);
					//GL.BindTexture(TextureTarget.Texture2D, 0);
				}
				else
				{
					GL.ActiveTexture(TextureUnit.Texture0);

					GL.Uniform1(shader.GetUniformLocation("diffuseTexture"), diffuseTexture.ID);

					diffuseTexture.BeginUse();
					model.Vao.Draw();
					diffuseTexture.EndUse();
				}
			}
			else
			{
				GL.Uniform3(shader.GetUniformLocation("materialColor"), renderSettings.Color);

				model.Vao.Draw();
			}
		}

		private void UseTexture(int uniformLocation, ref int textureId, TextureUnit textureUnit)
		{
			GL.ActiveTexture(textureUnit);
			GL.BindTexture(TextureTarget.Texture2D, textureId);
			GL.Uniform1(uniformLocation, textureUnit - TextureUnit.Texture0);
		}

	}
}
