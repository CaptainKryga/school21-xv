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
		private ContentTask nowTask;
		private GameTypes.Phase phase;
		private bool isPlay;

		private float delayGive = 5f, delayDrop = 5f, delayCraft = 10f, delay;

		private GameTypes.Task tempType;
		private Place tempPlaceA, tempPlaceB;
		private GameTypes.Item tempItem;
		private int tempIterations = 1;
		private string tempDescription;

		private List<ContentTask> cycles = new List<ContentTask>();

		public GameTypes.Task TempType { get => tempType; }
		public WindowAnimation WindowAnimation { get => wAnimation; }

		private void Update()
		{
			if (!isPlay)
				return;

			if (!nowTask)
			{
				if (actualTasks != null && actualTasks.Length > 0)
				{
					nowTask = actualTasks[0];
					nowTask.UpdateColor(true);
					wAnimation.SetTextWorker(nowTask.Description);
				}
				else
				{
					wAnimation.SetTextWorker("Worker is sleep!");
					worker.UpdateAnimation(0, 1);
					isPlay = false;
					return;
				}
			}

			if (nowTask.Type == GameTypes.Task.Transfer)
			{
				MethodTransfer();
			}
			else if (nowTask.Type == GameTypes.Task.Craft)
			{
				MethodCraft();
			}
			else if (nowTask.Type == GameTypes.Task.Cycle)
			{
				MethodCycle();
			}
		}

		private void MethodCycle()
		{
			if (!cycles.Contains(nowTask))
			{
				cycles.Add(nowTask);
				nowTask.NowIterations = nowTask.Iterations;
			}
			
			Debug.Log("updTask1");
			NextTask(nowTask);
		}
		
		private void MethodCraft()
		{
			//идём к первой точке
			if (phase == GameTypes.Phase.First)
			{
				worker.UpdateAnimation(1, 1);
				worker.UpdateSpeed(nowTask.Speed);
					
				if (worker.SetNextPosition(nowTask.PlaceA.Point ? nowTask.PlaceA.Point.position : 
					nowTask.PlaceA.transform.position))
				{
					phase = GameTypes.Phase.Second;
					delay = delayGive;
				}
					
				pointTarget.position = nowTask.PlaceA.Point ? nowTask.PlaceA.Point.position : 
					nowTask.PlaceA.transform.position;
			}
			//забираем объект из первого места
			else if (phase == GameTypes.Phase.Second)
			{
				worker.UpdateAnimation(0, 0);
				worker.UpdateSpeed(nowTask.Speed);

				delay -= Time.deltaTime * nowTask.Speed;
				if (delay <= 0)
				{
					phase = GameTypes.Phase.Third;
					Craft craft = (Craft)nowTask.PlaceA;
					if (craft)
					{
						craft.StartCraft(this, nowTask, worker);
						if (!nowTask)
							return;
						worker.UpdateVisibleItem(nowTask.Item, true);
					}
				}
			}
		}
		
		private void MethodTransfer()
		{
			//идём к первой точке
			if (phase == GameTypes.Phase.First)
			{
				worker.UpdateAnimation(1, 1);
				worker.UpdateSpeed(nowTask.Speed);
					
				if (worker.SetNextPosition(nowTask.PlaceA.transform.position))
				{
					phase = GameTypes.Phase.Second;
					delay = delayGive;
				}
					
				pointTarget.position = nowTask.PlaceA.transform.position;
			}
			//забираем объект из первого места
			else if (phase == GameTypes.Phase.Second)
			{
				worker.UpdateAnimation(0, 0);
				worker.UpdateSpeed(nowTask.Speed);

				delay -= Time.deltaTime * nowTask.Speed;
				if (delay <= 0)
				{
					phase = GameTypes.Phase.Third;
					Storage storage = (Storage)nowTask.PlaceA;
					if (storage)
					{
						storage.GetOneItem();
						worker.UpdateVisibleItem(nowTask.Item, true);
					}
				}
			}
			//несём объект ко второй точке
			else if (phase == GameTypes.Phase.Third)
			{
				worker.UpdateAnimation(1, 0);
				worker.UpdateSpeed(nowTask.Speed);

				if (worker.SetNextPosition(nowTask.PlaceB.transform.position))
				{
					phase = GameTypes.Phase.Fourth;
					delay = delayDrop;
				}
					
				pointTarget.position = nowTask.PlaceB.transform.position;
			}
			//складируем объект на точке
			else if (phase == GameTypes.Phase.Fourth)
			{
				worker.UpdateAnimation(0, 0);
				worker.UpdateSpeed(nowTask.Speed);

				delay -= Time.deltaTime * nowTask.Speed;
				if (delay <= 0)
				{
					phase = GameTypes.Phase.First;
					worker.UpdateAnimation(0, 1);
					worker.UpdateVisibleItem(nowTask.Item, false);
					Debug.Log("updTask2");
					NextTask(nowTask);
				}
			}
		}

		public void NextTask(ContentTask now)
		{
			//цикл
			if (cycles.Count > 0 && now == cycles[^1] && now.ParentTask != null)
			{
				nowTask.ParentTask.NowIterations--;
				nowTask.ParentTask.UpdateInfo();
				if (nowTask.ParentTask.NowIterations > 0)
				{
					nowTask.UpdateColor(false);
					nowTask = now.ParentTask;
					nowTask.UpdateColor(true);
					wAnimation.SetTextWorker(nowTask.Description);
					phase = GameTypes.Phase.First;
					Debug.Log("task1: " + nowTask.TextNameTask);
					return;
				}
			}
			
			//по порядку
			for (int x = 0; x < actualTasks.Length; x++)
			{
				if (actualTasks[x] == now && x + 1 < actualTasks.Length)
				{
					nowTask.UpdateColor(false);
					// nowTask = now.ParentTask;
					actualTasks[x + 1].UpdateColor(true);
					wAnimation.SetTextWorker(nowTask.Description);
					phase = GameTypes.Phase.First;
					Debug.Log("task2: " + actualTasks[x + 1].TextNameTask);
					nowTask = actualTasks[x + 1];
					return;
				}
			}
			
			//если таска была последней стопаем анимацию
			isPlay = false;
			nowTask.UpdateColor(false);
			worker.UpdateAnimation(0, 1);
			phase = GameTypes.Phase.First;
			Debug.Log("task3: " + null);

			nowTask = null;
		}

		public int UpdatePositionTasks(ContentTask task, bool isUp)
		{
			for (int x = 0; x < actualTasks.Length; x++)
			{
				if (actualTasks[x] == task)
				{
					if (isUp && x > 0)
					{
						//чек на ребёнка в цикле
						if (task.Type == GameTypes.Task.Cycle && ((task.ParentTask && 
							actualTasks[x - 1].ChildTask == task) || 
							(task.ChildTask && actualTasks[x - 1].ParentTask)))
							break;
						
						actualTasks[x] = actualTasks[x - 1];
						actualTasks[x - 1] = task;
					}
					if (!isUp && x < actualTasks.Length - 1)
					{
						//чек на родителя в цикле
						if (task.Type == GameTypes.Task.Cycle && ((task.ChildTask && 
							actualTasks[x + 1].ParentTask == task) || 
							(task.ParentTask && actualTasks[x + 1].ChildTask)))
							break;
						
						actualTasks[x] = actualTasks[x + 1];
						actualTasks[x + 1] = task;
					}
					break;
				}
			}
			
			wAnimation.UpdateContent(actualTasks);

			return 0;
		}

		public int DestroyTask(ContentTask task)
		{
			List<ContentTask> list = actualTasks.ToList();
			list.Remove(task);
			if (task.ParentTask)
			{
				list.Remove(task.ParentTask);
				Destroy(task.ParentTask.gameObject);
			}
			if (task.ChildTask)
			{
				list.Remove(task.ChildTask);
				Destroy(task.ChildTask.gameObject);
			}
			actualTasks = list.ToArray();
			Destroy(task.gameObject);
			return 0;
		}

		public void AddNewTask(string taskName, ContentTask contentTask, ContentTask subTaskEnd)
		{
			List<ContentTask> temp;
			if (actualTasks != null)
				temp = actualTasks.ToList();
			else
				temp = new List<ContentTask>();
			
			contentTask.InitTask(taskName, tempDescription, tempType, tempPlaceA, 
				tempPlaceB, tempItem, 1, tempIterations);
			contentTask.InitButtons(UpdatePositionTasks, DestroyTask);
			temp.Add(contentTask);

			if (contentTask.Type == GameTypes.Task.Cycle)
			{
				contentTask.InitWhile(tempIterations, subTaskEnd, null);
				subTaskEnd.InitButtons(UpdatePositionTasks, DestroyTask);
				subTaskEnd.InitWhile(tempIterations, null, contentTask);
				temp.Add(subTaskEnd);
			}

			actualTasks = temp.ToArray();
		}

		public Place[] GetAllPlaces()
		{
			return parentView.GetComponentsInChildren<Place>();
		}

		public Craft[] GetAllCrafts()
		{
			return parentView.GetComponentsInChildren<Craft>();
		}

		public Storage[] GetAllStorages()
		{
			return parentView.GetComponentsInChildren<Storage>();
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

		public void SetIterations(int iterations)
		{
			this.tempIterations = iterations;
		}

		public void SetDescription(string description)
		{
			this.tempDescription = description;
		}

		public ContentTask[] GetActualTasks()
		{
			return actualTasks;
		}
		public Dynamic[] GetAllDynamics()
		{
			return parentView.GetComponentsInChildren<Dynamic>();
		}

		public void SetActualTasks(ContentTask[] tasks)
		{
			RemoveActualTasks(actualTasks);
			actualTasks = tasks;
		}
		
		private void RemoveActualTasks(ContentTask[] tasks)
		{
			if (tasks == null)
				return;

			for (int i = 0; i < tasks.Length; i++)
			{
				Destroy(tasks[i].gameObject);
			}
		}
	}
}