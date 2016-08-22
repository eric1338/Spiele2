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

		public bool DoNonPlayerPhysics { get; set; }

		private DemoLevel demoLevel;

		public Physics(DemoLevel demoLevel)
		{
			this.demoLevel = demoLevel;

			DoNonPlayerPhysics = true;
		}

		public void DoPhysics()
		{
			ApplyPhysics(demoLevel.Player);

			if (!DoNonPlayerPhysics) return;

			DoRabbitHabit();

			ApplyPhysics(demoLevel.Rabbit);
			ApplyPhysics(demoLevel.DefaultBall);
			ApplyPhysics(demoLevel.CellShadingBall);
			ApplyPhysics(demoLevel.CellAndToonShadingBall);
		}

		public void LetPlayerJump(Vector3 direction)
		{
			demoLevel.Player.AddForce(direction * 14f);
		}

		public void LetRabbitJump(Vector3 direction)
		{
			if (!demoLevel.Rabbit.DoPhysics) return;

			demoLevel.Rabbit.AddForce(direction * 0.13f);
		}

		private void ApplyPhysics(IPhysical physical)
		{
			if (!physical.DoPhysics) return;

			AddEnvironmentalForces(physical);
			physical.ApplyForces();
		}

		private void AddEnvironmentalForces(IPhysical physical)
		{
			physical.AddForce(GetGravity() * physical.Mass);
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

			if (windForceLevel == 0) forceFactor = 0.0f;
			else if (windForceLevel == 1) forceFactor = 0.1f;
			else if (windForceLevel == 2) forceFactor = 0.3f;
			else forceFactor = 1f;

			return windDirection * forceFactor * 0.001f;
		}

		int count = -120;

		float rand1 = 0;

		private void DoRabbitHabit()
		{
			Rabbit rabbit = demoLevel.Rabbit;

			count++;

			if (count < 0) return;

			if (count % 400 == 0)
			{
				rand1 = (float)(new Random()).NextDouble() * 0.02f;
			}
			if (count % 400 < 100)
			{
				rabbit.Rotation += rand1;
			}
			if (count % 400 == 100)
			{
				LetRabbitJump(rabbit.GetJumpDirection());
			}
		}


	}
}
