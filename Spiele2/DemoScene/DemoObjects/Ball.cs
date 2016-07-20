using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace DemoScene.DemoObjects
{
	class Ball : PhysicalBody
	{

		public Ball() : base(Vector3.Zero, 0.4f)
		{
			DoPhysics = true;
			Bounce = true;
			BounceFactor = 1f;
		}
	}
}
