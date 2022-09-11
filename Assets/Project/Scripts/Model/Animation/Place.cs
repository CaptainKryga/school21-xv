using System;
using Project.Scripts.Utils;

namespace Animation
{
	public class Place : Dynamic
	{
		public static int id;
		public GameTypes.Place type;
		public GameTypes.Item[] input;
		public GameTypes.Item output;
		public int localId;

		private void Start()
		{
			localId = id++;
		}
	} 
}

