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
		private Vector3 moonLightColor = new Vector3(0.4f, 0.1f, 0.8f);

		private float myAngle = 0.2f;

		private float xFactor = 80f;
		private float yFactor = 40f;
		private float zValue = 80f;

		public Vector3 GetLightPosition()
		{
			return IsDay() ? GetSunPosition() : GetMoonPosition();
		}

		public Vector3 GetLightDirection()
		{
			Vector3 lightDirection = Vector3.Zero - GetLightPosition();

			lightDirection.Normalize();

			return lightDirection;
		}

		public Vector3 GetLightColor()
		{
			Vector3 lightColor = IsDay() ? sunLightColor : moonLightColor;

			return lightColor * GetIntensity() * 0.5f;
		}

		public float GetIntensity()
		{
			Vector3 sunPosition = GetSunPosition();
			Vector3 straightSunPosition = new Vector3(sunPosition.X, sunPosition.Y, 0);

			straightSunPosition.Normalize();

			float intensity = Math.Abs(straightSunPosition.Y);

			if (!IsDay()) intensity *= 0.4f;

			return intensity;
		}

		public float GetAmbientFactor()
		{
			return IsDay() ? 0.8f : 0.4f;
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
