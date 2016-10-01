using DemoScene.Utils;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.DemoObjects
{
	class ParticleSystem
	{

		private int maximumParticles;
		private Vector3 emitPosition;

		private float lastEmit = 0;
		private float releaseIntervall;

		private float defaultParticleLifeTimeSpan = 10;

		private float time = 0;

		public List<Particle> Particles { get; set; }

		public ParticleSystem()
		{
			Particles = new List<Particle>();

			maximumParticles = 100;
			emitPosition = new Vector3(2, 0, 2);
			releaseIntervall = 0.1f;
		}


		public void Update()
		{
			time += 0.01f;

			List<Particle> particlesCopy = new List<Particle>(Particles);

			foreach (Particle particle in particlesCopy) {
				particle.Update(time);

				if (particle.IsRetired) Particles.Remove(particle);
			}

			if (Particles.Count < maximumParticles)
			{
				while (lastEmit + releaseIntervall < time)
				{
					lastEmit += releaseIntervall;

					Particles.Add(CreateNewParticle(time));
				}
			}
		}

		private Particle CreateNewParticle(float time)
		{
			Particle particle = new Particle(emitPosition, time, defaultParticleLifeTimeSpan);

			Vector3 randomVector = Util.GetRandomVector3();
			float randomSpeed = Util.GetRandomFloat() * 0.4f + 0.8f;

			Vector3 influenceVector = new Vector3(0, 10, 2);

			Vector3 emitVector = influenceVector + randomVector;
			emitVector.Normalize();

			particle.EmitForce = emitVector * randomSpeed;

			return particle;
		}

	}
}
