using UnityEngine;

namespace Project.Scripts.Model
{
	[CreateAssetMenu(fileName = "DataBase", menuName = "ScriptableObjects/DataBase", order = 1)]
	public class DataBase : ScriptableObject
	{
		public GameObject[] standardPrefabs;
	}
}