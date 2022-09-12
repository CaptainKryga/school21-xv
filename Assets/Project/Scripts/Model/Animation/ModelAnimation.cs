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
		[SerializeField] private Worker worker;

		[SerializeField] private Transform pointTarget;
		
		private ContentTask[] actualTasks;
		private int id;
		private GameTypes.Phase phase;
		private bool isPlay;

		private float delayGive = 5f, delayDrop = 5f, delayCraft = 10f, delay;

		private GameTypes.Task tempType;
		private Place tempPlaceA, tempPlaceB;
		private GameTypes.Item tempItem;

		private void Update()
		{
			if (!isPlay)
				return;

			if (id >= actualTasks.Length)
				id = 0;

			if (actualTasks[id].Type == GameTypes.Task.Transfer)
			{
				//идём к первой точке
				if (phase == GameTypes.Phase.First)
				{
					worker.UpdateAnimation(1, 1);
					if (worker.SetNextPosition(actualTasks[id].PlaceA.transform.position))
					{
						phase = GameTypes.Phase.Second;
						delay = delayGive;
					}
					
					pointTarget.position = actualTasks[id].PlaceA.transform.position;
				}
				//забираем объект из первого места
				else if (phase == GameTypes.Phase.Second)
				{
					worker.UpdateAnimation(0, 0);

					delay -= Time.deltaTime * actualTasks[id].Speed;
					if (delay <= 0)
						phase = GameTypes.Phase.Third;
				}
				//несём объект ко второй точке
				else if (phase == GameTypes.Phase.Third)
				{
					worker.UpdateAnimation(1, 0);
					if (worker.SetNextPosition(actualTasks[id].PlaceB.transform.position))
					{
						phase = GameTypes.Phase.Fourth;
						delay = delayDrop;
					}
					
					pointTarget.position = actualTasks[id].PlaceB.transform.position;
				}
				//складируем объект на точке
				else if (phase == GameTypes.Phase.Fourth)
				{
					worker.UpdateAnimation(0, 0);

					delay -= Time.deltaTime * actualTasks[id].Speed;
					if (delay <= 0)
					{
						phase = GameTypes.Phase.First;
						id++;
						worker.UpdateAnimation(0, 1);
					}
				}
				
				Debug.Log("phase: " + phase + "[" + actualTasks[id].Speed + "]");
			}
			else if (actualTasks[id].Type == GameTypes.Task.Craft)
			{
				
			}
			else if (actualTasks[id].Type == GameTypes.Task.Cycle)
			{
				
			}

		}

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

		public void AddNewTask(string taskName, ContentTask contentTask)
		{
			List<ContentTask> temp;
			if (actualTasks != null)
				temp = actualTasks.ToList();
			else
				temp = new List<ContentTask>();
			
			temp.Add(contentTask);
			contentTask.InitTask(taskName, UpdatePositionTasks, tempType, tempPlaceA, tempPlaceB, tempItem);
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
				Debug.Log("places[x].GetInstanceID() == place " + places[x].GetInstanceID() + "==" + place);
				if (places[x].GetInstanceID() == place)
				{
					return places[x];
				}
			}

			return null;
		}

		public void ChangePlayStatus(bool isPlay)
		{
			this.isPlay = isPlay;
		}

		public void SetTypeTask(GameTypes.Task type)
		{
			this.tempType = type;
		}

		public void SetPlaceA(Place placeA)
		{
			this.tempPlaceA = placeA;
		}

		public void SetPlaceB(Place placeB)
		{
			this.tempPlaceB = placeB;
		}

		public void SetItem(GameTypes.Item item)
		{
			this.tempItem = item;
		}
	}
}