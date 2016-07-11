using DemoScene.DemoObjects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.Logic
{
	class Physics
	{

		private DemoLevel demoLevel;

		public Physics(DemoLevel demoLevel)
		{
			this.demoLevel = demoLevel;
		}


		public void DoPhysics()
		{
			ApplyPhysics(demoLevel.Rabbit);
		}

		public void LetPlayerJump(Vector3 direction)
		{
			
		}

		public void LetRabbitJump(Vector3 direction)
		{
			demoLevel.Rabbit.AddForce(direction * 0.1f);
		}

		private void ApplyPhysics(IPhysical physical)
		{
			if (!physical.DoPhysics) return;

			AddEnvironmentalForces(physical);
			physical.ApplyForces();
		}

		private void AddEnvironmentalForces(IPhysical physical)
		{
			physical.AddForce(GetGravity());
			physical.AddForce(GetWindForce());
		}

		private Vector3 GetGravity()
		{
			int gravityLevel = demoLevel.GravityLevel;

			float yFactor;

			if (gravityLevel == 0) yFactor = 0;
			else if (gravityLevel == 1) yFactor = 0.2f;
			else yFactor = 1f;

			return new Vector3(0, -0.01f * yFactor, 0);
		}

		public Vector3 GetWindForce()
		{
			Vector3 windDirection = demoLevel.WindDirection;
			int windForceLevel = demoLevel.WindForceLevel;

			windDirection.Normalize();

			float forceFactor;

			if (windForceLevel == 0) forceFactor = 0.1f;
			else if (windForceLevel == 1) forceFactor = 0.3f;
			else forceFactor = 1f;

			return windDirection * forceFactor * 0.01f;
		}


	}
}
