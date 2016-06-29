using Framework;
using System.IO;
using System.Text;

namespace ShaderDebugging
{
	public class ShaderFileDebugger
	{
		public ShaderFileDebugger(string vertexFile, string fragmentFile, byte[] vertexShader = null, byte [] fragmentShader = null)
		{
			if (File.Exists(vertexFile) && File.Exists(fragmentFile))
			{
				shaderWatcherVertex = new FileWatcher(vertexFile);
				shaderWatcherFragment = new FileWatcher(fragmentFile);
				CheckForShaderChange();
				while(null != LastException)
				{
					form.Hide();
					FormShaderExceptionFacade.ShowModal(LastException);
					CheckForShaderChange();
				}
			}
			else
			{
				var sVertex = Encoding.UTF8.GetString(vertexShader);
				var sFragment = Encoding.UTF8.GetString(fragmentShader);
				shader = ShaderLoader.FromStrings(sVertex, sFragment);
			}
		}

		public bool CheckForShaderChange()
		{
			//test if we even have file -> no files nothing to be done
			if (null == shaderWatcherVertex || null == shaderWatcherFragment) return false;
			//test if any file is dirty
			if (!shaderWatcherVertex.Dirty && !shaderWatcherFragment.Dirty) return false;
			try
			{
				shader = ShaderLoader.FromFiles(shaderWatcherVertex.FullPath, shaderWatcherFragment.FullPath);
				shaderWatcherVertex.Dirty = false;
				shaderWatcherFragment.Dirty = false;
				form.Clear();
				return true;
			}
			catch (IOException e)
			{
				LastException = new ShaderException("ERROR", e.Message, string.Empty, string.Empty);
				form.Show(LastException);
			}
			catch (ShaderException e)
			{
				LastException = e;
				form.Show(e);
			}
			return false;
		}

		public Shader Shader { get { return shader; } }
		public ShaderException LastException { get; private set; }

		private Shader shader;
		private readonly FileWatcher shaderWatcherVertex = null;
		private readonly FileWatcher shaderWatcherFragment = null;
		private readonly FormShaderExceptionFacade form = new FormShaderExceptionFacade();
	}
}
