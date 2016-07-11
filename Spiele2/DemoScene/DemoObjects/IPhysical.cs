using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.DemoObjects
{
	interface IPhysical
	{

		Vector3 Position { get; set; }
		float Mass { get; set; }
		bool DoPhysics { get; set; }

		void AddForce(Vector3 force);
		void ApplyForces();
	}
}
