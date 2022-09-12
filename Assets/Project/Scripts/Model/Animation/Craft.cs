using Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace Animation
{
	public class Craft : Place
	{
		[SerializeField] private NavMeshAgent agent;
		[SerializeField] private GameTypes.Craft type;

		public void StartCreate(ContentTask func)
		{
			//start animation
			if (type == GameTypes.Craft.Use)
			{
				//чо-то включаем анимацию?
			}
			else if (type == GameTypes.Craft.Drive)
			{
				//вождение
			}
		}
	}
}

