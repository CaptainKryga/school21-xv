using System;
using Project.Scripts.Model.Animation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.View
{
	public class WindowAnimation : MonoBehaviour
	{
		[SerializeField] private ModelAnimation modelAnimation;
		
		[SerializeField] private Transform parentContent;
		[SerializeField] private ContentTask prefabContentTask;

		[Header("Sub Windows")] 
		[SerializeField] private GameObject panelRedactor;
		[SerializeField] private GameObject panelTransfer;
		[SerializeField] private GameObject panelCraft;
		[SerializeField] private GameObject panelCycle;

		[Header("Basic")] 
		[SerializeField] private TMP_InputField inputFieldTaskName;
		[SerializeField] private TMP_Dropdown dropdownTaskType;
		
		[Header("Transfer")] 
		[SerializeField] private TMP_Dropdown transferDropdownPointA;
		[SerializeField] private TMP_Dropdown transferDropdownPointB;
		[SerializeField] private TMP_Dropdown transferDropdownItem;
		[SerializeField] private Slider transferSliderSpeed;		
		
		[Header("Craft")] 
		[SerializeField] private TMP_Dropdown craftDropdownTable;
		[SerializeField] private TMP_Dropdown craftDropdownItem;
		[SerializeField] private Slider craftSliderSpeed;		
		
		[Header("Cycle")] 
		[SerializeField] private TMP_InputField cycleDropdownIterations;

		private void Start()
		{
			panelRedactor.SetActive(false);
			panelTransfer.SetActive(true);
			panelCraft.SetActive(false);
			panelCycle.SetActive(false);
		}

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
			panelRedactor.SetActive(true);
		}

		public void OnClick_StartSequence()
		{
			
		}

		public void OnClick_StopSequence()
		{
			
		}

		public void OnClick_Save()
		{
			ContentTask task = Instantiate(prefabContentTask, parentContent).GetComponent<ContentTask>();
			modelAnimation.AddNewTask(task);
		}

		public void OnClick_Cancel()
		{
			panelRedactor.SetActive(false);
		}

		public void OnDropdown_TypeTask()
		{
			if (dropdownTaskType.options[dropdownTaskType.value].text == "Transfer")
			{
				panelTransfer.SetActive(true);
				panelCraft.SetActive(false);
				panelCycle.SetActive(false);
			} else if (dropdownTaskType.options[dropdownTaskType.value].text == "Craft")
			{
				panelTransfer.SetActive(false);
				panelCraft.SetActive(true);
				panelCycle.SetActive(false);
			} else if (dropdownTaskType.options[dropdownTaskType.value].text == "Cycle")
			{
				panelTransfer.SetActive(false);
				panelCraft.SetActive(false);
				panelCycle.SetActive(true);
			}
		}
	}
}