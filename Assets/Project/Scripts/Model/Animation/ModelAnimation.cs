using System.Collections.Generic;
using System.Linq;
using Animation;
using Project.Scripts.Utils;
using Project.Scripts.View;
using UnityEngine;

namespace Project.Scripts.Model.Animation
{
	public class ModelAnimation : MonoBehaviour
	{
		[SerializeField] private WindowAnimation wAnimation;

		[SerializeField] private Transform parentView;
		
		private ContentTask[] actualTasks;
		private int id;

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

		public void AddNewTask(string taskName, ContentTask contentTask, GameTypes.Task task, int placeA, int placeB, 
			GameTypes.Item item)
		{
			List<ContentTask> temp;
			if (actualTasks != null)
				temp = actualTasks.ToList();
			else
				temp = new List<ContentTask>();
			
			temp.Add(contentTask);
			contentTask.InitTask(taskName, UpdatePositionTasks, task, GetPlaceFromInt(placeA), 
				GetPlaceFromInt(placeB), item);
			actualTasks = temp.ToArray();
		}

		public Place[] GetAllPlaces()
		{
			return parentView.GetComponentsInChildren<Place>();
		}

		public Craft[] GetCraft(GameTypes.Item type)
		{
			Craft[] crafts = parentView.GetComponentsInChildren<Craft>();
			List<Craft> list = new List<Craft>();
			for (int x = 0; x < crafts.Length; x++)
			{
				for (int y = 0; y < crafts[x].input.Length; y++)
				{
					if (crafts[x].input[y] == type)
					{
						list.Add(crafts[x]);
						break;
					}
				}
			}
			return list.ToArray();
		}

		private Place GetPlaceFromInt(int place)
		{
			Place[] places = GetAllPlaces();
			for (int x = 0; x < places.Length; x++)
			{
				if (places[x].GetInstanceID() == place)
					return places[x];
			}

			return null;
		}
	}
}