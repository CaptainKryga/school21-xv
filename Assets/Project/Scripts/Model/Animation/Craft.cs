using Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace Animation
{
	public class Craft : Place
	{
		[SerializeField] private NavMeshAgent agent;
		[SerializeField] private GameTypes.TypeCraft typeCraft;

		public void StartCraft(ContentTask func)
		{
			//start animation
			if (typeCraft == GameTypes.TypeCraft.Use)
			{
				//чо-то включаем анимацию?
				anim.Play();
				var contentTask = func;
			}
			else if (typeCraft == GameTypes.TypeCraft.Drive)
			{
				//вождение
			}
		}
	}
}

