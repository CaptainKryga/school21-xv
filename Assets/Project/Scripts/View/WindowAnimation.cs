using System.Collections.Generic;
using Project.Scripts.Model.Animation;
using UnityEngine;

namespace Project.Scripts.View
{
	public class WindowAnimation : MonoBehaviour
	{
		[SerializeField] private ModelAnimation modelAnimation;
		
		[SerializeField] private Transform parentContent;
		[SerializeField] private ContentTask prefabContentTask;

		public void UpdateContent(ContentTask[] actualTasks)
		{
			for (int x = 0; x < actualTasks.Length; x++)
			{
				actualTasks[x].transform.SetParent(parentContent.parent);
			}		
			for (int x = 0; x < actualTasks.Length; x++)
			{
				actualTasks[x].transform.SetParent(parentContent);
			}
		}
		
		public void OnClick_OpenRedactor()
		{
			ContentTask task = Instantiate(prefabContentTask, parentContent).GetComponent<ContentTask>();
			modelAnimation.AddNewTask(task);
		}

		public void OnClick_StartSequence()
		{
			
		}

		public void OnClick_StopSequence()
		{
			
		}
	}
}