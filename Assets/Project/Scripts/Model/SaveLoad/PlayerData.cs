using System;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.Model
{
	[Serializable]
	public class PlayerData
	{
		//позиции тела игрока
		public Vector3 bodyPosition;
		public Quaternion bodyRotation;
		
		//позиции камеры игрока
		public Vector3 camPosition;
		public Quaternion camRotation;

		public GameTypes.PlayerMove state;
	}
}