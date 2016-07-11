using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScene.DemoObjects
{
	class Player : PhysicalBody
	{

		public Player() : base(new Vector3(0, 0, 0), 60)
		{

		}

	}
}
