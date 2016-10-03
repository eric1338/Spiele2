using DemoScene.Utils;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.DemoObjects
{
	class LightningBug
	{

		public Vector3 FirstDestiny { get; set; }
		public Vector3 SecondDestiny { get; set; }

		public Vector3 Position { get; set; }

		private bool moveToFirstDestiny;

		private Vector3 firstMoveVector;
		private Vector3 secondMoveVector;

		private int maxTickCount = 100;
		private int tickCount;

		public LightningBug(Vector3 firstDestiny, Vector3 secondDestiny) {
			FirstDestiny = firstDestiny;
			SecondDestiny = secondDestiny;

			SetInitialValues();
		}

		public void SetInitialValues()
		{
			Position = SecondDestiny;
			moveToFirstDestiny = true;

			tickCount = 0;

			firstMoveVector = CreateMoveVector();
			secondMoveVector = CreateMoveVector();
		}

		private Vector3 GetCurrentDestiny()
		{
			return moveToFirstDestiny ? FirstDestiny : SecondDestiny;
		}

		private Vector3 GetVectorToDestiny()
		{
			Vector3 vectorToDestiny = GetCurrentDestiny() - Position;

			vectorToDestiny.Normalize();

			return vectorToDestiny;
		}

		private bool IsCloseEnoughToDestiny()
		{
			return (GetCurrentDestiny() - Position).Length < 3f;
		}

		public void Tick()
		{
			tickCount++;

			if (IsCloseEnoughToDestiny()) moveToFirstDestiny = !moveToFirstDestiny;

			if (tickCount > maxTickCount)
			{
				tickCount = 0;
				firstMoveVector = secondMoveVector;
				secondMoveVector = CreateMoveVector();
			}

			Position += GetFinalMoveVector() * 0.05f;
		}

		private Vector3 GetFinalMoveVector()
		{
			Vector3 combinedMoveVector = (maxTickCount - tickCount) * firstMoveVector + tickCount * secondMoveVector;

			combinedMoveVector.Normalize();

			return combinedMoveVector;
		}

		private Vector3 CreateMoveVector()
		{
			Vector3 randomVector = Util.GetRandomVector3();

			Vector3 moveVector = randomVector + GetVectorToDestiny() * 4;

			moveVector.Normalize();

			return moveVector;
		}

	}
}
