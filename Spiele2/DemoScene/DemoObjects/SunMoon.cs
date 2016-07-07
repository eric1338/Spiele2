using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.DemoObjects
{
	class SunMoon
	{

		private Vector3 sunLightColor = new Vector3(1.0f, 0.9f, 0.8f);
		private Vector3 moonLightColor = new Vector3(0.4f, 0.0f, 1.0f);

		private float myAngle = 0.2f;

		private float intensity = 0.4f;

		private float xFactor = 20f;
		private float yFactor = 18f;
		private float zValue = 10f;

		public Vector3 GetLightDirection()
		{
			Vector3 starPosition = IsDay() ? GetSunPosition() : GetMoonPosition();

			return Vector3.Zero - starPosition;
		}

		public Vector3 GetLightColor()
		{
			Vector3 lightColor = IsDay() ? sunLightColor : moonLightColor;

			Vector3 sunPos = GetSunPosition();
			sunPos.Normalize();
			float test = Math.Abs(sunPos.Y);

			return lightColor * test * intensity;
		}

		public void IncreaseAngle()
		{
			myAngle += 0.02f;
		}

		public bool IsDay()
		{
			return GetSunPosition().Y > GetMoonPosition().Y;
		}

		private Vector3 GetStarPosition(float angle)
		{
			float x = (float) Math.Cos(angle) * xFactor;
			float y = (float) Math.Sin(angle) * yFactor;

			return new Vector3(x, y, zValue);
		}

		private Vector3 GetSunPosition()
		{
			return GetStarPosition(myAngle);
		}

		private Vector3 GetMoonPosition()
		{
			return GetStarPosition(myAngle + 3.1415f);
		}

	}
}
