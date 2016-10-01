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

		public bool IsRunning { get; set; }

		public SunMoon SunMoon { get; set; }

		public Player Player { get; set; }
		public Rabbit Rabbit { get; set; }

		public Ball DefaultBall { get; set; }
		public Ball CellShadingBall { get; set; }
		public Ball CellAndToonShadingBall { get; set; }

		public TetrahedronSphere TetrahedronSphere { get; set; }
		public ParticleSystem ParticleSystem { get; set; }

		public Vector3 WindDirection { get; set; }
		public int WindForceLevel { get; set; }

		public int GravityLevel { get; set; }
		public float GravityForce = 9.81f;

		private float windRotationAngle = -1.57f;
		private float windRotationDelta = 0.02f;

		public DemoLevel()
		{
			SunMoon = new SunMoon();

			Player = new Player();
			Rabbit = new Rabbit();

			DefaultBall = new Ball();
			CellShadingBall = new Ball();
			CellAndToonShadingBall = new Ball();

			TetrahedronSphere = new TetrahedronSphere();
			ParticleSystem = new ParticleSystem();

			SetInitialValues();
		}

		public void SetInitialValues()
		{
			IsRunning = true;

			WindDirection = new Vector3(0, 0, -1);
			WindForceLevel = 0;
			GravityLevel = 2;

			Rabbit.Position = new Vector3(-3, 0, 3);
			Rabbit.ResetForces();

			DefaultBall.Position = new Vector3(9f, 3f, 9f);
			DefaultBall.ResetForces();

			CellShadingBall.Position = new Vector3(6f, 3.5f, 9f);
			CellShadingBall.ResetForces();

			CellAndToonShadingBall.Position = new Vector3(3f, 4f, 9f);
			CellAndToonShadingBall.ResetForces();
		}

		public void IncreaseWindForce()
		{
			if (WindForceLevel < 3) WindForceLevel++;
		}

		public void DecreaseWindForce()
		{
			if (WindForceLevel > 0) WindForceLevel--;
		}

		public void IncreaseGravity()
		{
			if (GravityLevel < 2) GravityLevel++;
		}

		public void DecreaseGravity()
		{
			if (GravityLevel > 0) GravityLevel--;
		}

		public void RotateWind()
		{
			windRotationAngle += windRotationDelta;

			float x = (float) Math.Cos(windRotationAngle);
			float z = (float) Math.Sin(windRotationAngle);

			WindDirection = new Vector3(x, 0, z);
		}

	}
}
