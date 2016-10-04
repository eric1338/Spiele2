using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.DemoObjects
{
	class Particle : PhysicalBody
	{

		private float birthTime;
		private float lifeTimeSpan;

		public float Alpha { get; set; } = 1;
		public bool IsRetired { get; set; } = false;

		public Vector3 EmitForce { get; set; }
		public bool HasEmitForceBeenUsed { get; set; } = false;

		public Particle(Vector3 position, float birthTime, float lifeTimeSpan) : base(position, 5f)
		{
			this.birthTime = birthTime;
			this.lifeTimeSpan = lifeTimeSpan;
		}

		public void Update(float time)
		{
			float timeActive = time - birthTime;

			Alpha = 1 - (timeActive / lifeTimeSpan);

			if (timeActive > lifeTimeSpan) IsRetired = true;
		}

	}
}
