using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.Utils
{
	class Util
	{

		private static Random random = new Random((int) DateTime.Now.Ticks);


		public static float GetRandomFloat()
		{
			return (float) random.NextDouble();
		}

		public static float GetRandomCoordinate()
		{
			return GetRandomFloat() * 2 - 1;
		}

		public static Vector3 GetRandomVector3()
		{
			Vector3 vector = new Vector3(GetRandomCoordinate(), GetRandomCoordinate(), GetRandomCoordinate());

			vector.Normalize();

			return vector;
		}

	}
}
