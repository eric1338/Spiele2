using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.Visual
{
	class Textures
	{

		public static Textures Instance = new Textures();

		private bool _texturesLoaded = false;

		public Texture RabbitDiffuse { get; set; }
		public Texture R2D2Diffuse { get; set; }
		public Texture R2D2Specular { get; set; }
		public Texture StatueDiffuse { get; set; }
		public Texture NyraDiffuse { get; set; }

		public Texture Ground { get; set; }
		public Texture Flag { get; set; }

		public Texture DaySkySide { get; set; }
		public Texture DaySkyTop { get; set; }
		public Texture DaySkyBottom { get; set; }
		public Texture NightSkySide { get; set; }
		public Texture NightSkyTop { get; set; }
		public Texture NightSkyBottom { get; set; }


		public void LoadTextures()
		{
			if (_texturesLoaded) return;

			RabbitDiffuse = TextureLoader.FromBitmap(Resources.rabbit_d);
			R2D2Diffuse = TextureLoader.FromBitmap(Resources.r2d2_d);
			R2D2Specular = TextureLoader.FromBitmap(Resources.r2d2_s);
			StatueDiffuse = TextureLoader.FromBitmap(Resources.statue_d);
			NyraDiffuse = TextureLoader.FromBitmap(Resources.nyra_d);

			Flag = TextureLoader.FromBitmap(Resources.flag);
			Ground = TextureLoader.FromBitmap(Resources.autumn);

			DaySkySide = TextureLoader.FromBitmap(Resources.daysky_side);
			DaySkyTop = TextureLoader.FromBitmap(Resources.daysky_top);
			DaySkyBottom = TextureLoader.FromBitmap(Resources.daysky_bottom);
			NightSkySide = TextureLoader.FromBitmap(Resources.nightsky_side);
			NightSkyTop = TextureLoader.FromBitmap(Resources.nightsky_top);
			NightSkyBottom = TextureLoader.FromBitmap(Resources.nightsky_bottom);

			_texturesLoaded = true;
		}

	}
}
