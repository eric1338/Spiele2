using DemoScene.Utils;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.DemoObjects
{
	class DemoLevel
	{

		public SunMoon SunMoon { get; set; }

		public Vector3 WindDirection { get; set; }
		public int WindForceLevel { get; set; }

		public float GravityForce = 9.81f;

		private float windRotationAngle = -1.57f;
		private float windRotationDelta = 0.02f;

		public DemoLevel()
		{
			SunMoon = new SunMoon();

			WindDirection = new Vector3(0, 0, -1);
			WindForceLevel = 0;
		}

		public void IncreaseWindForce()
		{
			if (WindForceLevel < 2) WindForceLevel++;
		}

		public void DecreaseWindForce()
		{
			if (WindForceLevel > 0) WindForceLevel--;
		}

		public void RotateWind()
		{
			windRotationAngle += windRotationDelta;

			float x = (float)Math.Cos(windRotationAngle);
			float z = (float)Math.Sin(windRotationAngle);

			WindDirection = new Vector3(x, 0, z);
		}

	}
}
