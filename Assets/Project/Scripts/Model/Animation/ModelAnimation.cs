using System.Collections.Generic;
using System.Linq;
using Project.Scripts.View;
using UnityEngine;

namespace Project.Scripts.Model.Animation
{
	public class ModelAnimation : MonoBehaviour
	{
		[SerializeField] private WindowAnimation wAnimation;
		
		private ContentTask[] actualTasks;

		private int UpdatePositionTasks(ContentTask task, bool isUp)
		{
			for (int x = 0; x < actualTasks.Length; x++)
			{
				if (actualTasks[x] == task)
				{
					ContentTask swap = actualTasks[x];
					if (isUp && x > 0)
					{
						actualTasks[x] = actualTasks[x - 1];
						actualTasks[x - 1] = swap;
					}
					if (!isUp && x < actualTasks.Length - 1)
					{
						actualTasks[x] = actualTasks[x + 1];
						actualTasks[x + 1] = swap;
					}
					break;
				}
			}
			
			wAnimation.UpdateContent(actualTasks);

			return 0;
		}

		public void AddNewTask(ContentTask task)
		{
			List<ContentTask> temp;
			if (actualTasks != null)
				temp = actualTasks.ToList();
			else
				temp = new List<ContentTask>();
			
			temp.Add(task);
			task.InitButtons(UpdatePositionTasks);
			actualTasks = temp.ToArray();
		}
	}
}