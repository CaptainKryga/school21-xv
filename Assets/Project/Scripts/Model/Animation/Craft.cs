using System.Collections;
using System.Linq;
using Project.Scripts.Model;
using Project.Scripts.Model.Animation;
using Project.Scripts.Utils;
using UnityEngine;

namespace Animation
{
	public class Craft : Place
	{
		[SerializeField] private GameObject car;
		[SerializeField] private GameObject[] pallets;
		[SerializeField] private GameTypes.TypeCraft typeCraft;
		
		public GameTypes.TypeCraft TypeCraft { get => typeCraft; }

		public void StartCraft(ModelAnimation model, ContentTask task, Worker worker)
		{
			//start animation
			if (typeCraft == GameTypes.TypeCraft.Use)
			{
				//чо-то включаем анимацию?
				anim.Play();

				model.NextTask(task);
			}
			else if (typeCraft == GameTypes.TypeCraft.Drive)
			{
				//вождение
				Storage[] storages = FindObjectsOfType<Storage>();
				Craft[] crafts = FindObjectsOfType<Craft>();
				Craft nearCraft = null;
				float distance = 10000000;
				for (int x = 0; x < crafts.Length; x++)
				{
					if (crafts[x].typeCraft == GameTypes.TypeCraft.Sub)
					{
						float temp = Vector3.Distance(worker.GetTransform.position, crafts[x].transform.position);
						if (temp < distance)
						{
							nearCraft = crafts[x];
							distance = temp;
						}
					}
				}
				
				if (crafts.Length == 0 || storages.Length == 0 || nearCraft == null)
				{
					Debug.LogError("НЕТ ХРАНИЛИЩ ИЛИ КРАФТЕРОВ OR SUBS");
					Debug.Log("updTask4");
					model.NextTask(task);
					return;
				}
				
				StartCoroutine(Drive(nearCraft, storages.ToArray(), model, task, worker));
			}
		}

		IEnumerator Drive(Craft craft, Storage[] storages, ModelAnimation model, ContentTask task, Worker worker)
		{
			GameTypes.Phase phase = GameTypes.Phase.First;
			int x = 0;
			car.transform.SetParent(worker.GetTransform);
			car.transform.localPosition = Vector3.zero;
			car.transform.localRotation = Quaternion.identity;

			while (true)
			{
				if (phase == GameTypes.Phase.First)
				{
					worker.UpdateAnimation(1, 1);
					worker.UpdateSpeed(task.Speed);
					
					if (worker.SetNextPosition(storages[x].Point ? storages[x].Point.position : 
						storages[x].transform.position, 2))
					{
						phase = GameTypes.Phase.Second;
						pallets[storages[x].output == GameTypes.Item.BoxSmall ? 0 : 1].SetActive(true);
						storages[x].gameObject.SetActive(false);
					}
				}
				else if (phase == GameTypes.Phase.Second)
				{
					worker.UpdateAnimation(1, 1);
					worker.UpdateSpeed(task.Speed);
					
					if (worker.SetNextPosition(craft.Point ? craft.Point.position : craft.transform.position, 2))
					{
						phase = GameTypes.Phase.Third;
					}
				}
				else if (phase == GameTypes.Phase.Third)
				{
					worker.UpdateAnimation(1, 1);
					worker.UpdateSpeed(task.Speed);
					
					if (worker.SetNextPosition(storages[x].Point ? storages[x].Point.position : 
						storages[x].transform.position, 2))
					{
						pallets[storages[x].output == GameTypes.Item.BoxSmall ? 0 : 1].SetActive(false);
						storages[x].gameObject.SetActive(true);
						yield return new WaitForSeconds(1);
						x++;
						if (x >= storages.Length)
						{
							phase = GameTypes.Phase.Fourth;
						}
						else
						{
							phase = GameTypes.Phase.First;
						}
					}
				}
				else if (phase == GameTypes.Phase.Fourth)
				{
					worker.UpdateAnimation(1, 1);
					worker.UpdateSpeed(task.Speed);
					
					if (worker.SetNextPosition(transform.position, 2))
					{
						if (x >= storages.Length)
						{
							break;
						}
						else
						{
							phase = GameTypes.Phase.First;
						}
					}
				}
				yield return new WaitForEndOfFrame();
				// Debug.Log("phase: " + phase);
			}
			
			car.transform.SetParent(transform);
			car.transform.localPosition = Vector3.zero;
			car.transform.localRotation = Quaternion.identity;
			
			Debug.Log("updTask5");
			model.NextTask(task);
			yield break;
		}
	}
}

