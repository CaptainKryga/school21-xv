using System;
using UnityEngine;

namespace Project.Scripts.Model
{
	[Serializable]
	public class ItemData
	{
		//стандартное имя префаба который находиться в директории
		public string defaultName;
		public string itemName;
		public Vector3 position;
		public Quaternion rotation;
		public Color color;

		public ItemData(string defaultName, string itemName, Vector3 position, 
			Quaternion rotation, Color color)
		{
			this.defaultName = defaultName;
			this.itemName = itemName;
			this.position = position;
			this.rotation = rotation;
			this.color = color;
		}
	}
}