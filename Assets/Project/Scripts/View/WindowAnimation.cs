using System;
using System.Collections.Generic;
using Animation;
using Project.Scripts.Model.Animation;
using Project.Scripts.Utils;
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
		[SerializeField] private TMP_Dropdown transferDropdownPlaceA;
		[SerializeField] private TMP_Dropdown transferDropdownPlaceB;
		[SerializeField] private TMP_Dropdown transferDropdownItem;
		[SerializeField] private Slider transferSliderSpeed;		
		
		[Header("Craft")] 
		[SerializeField] private TMP_Dropdown craftDropdownPlace;
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
			modelAnimation.ChangePlayStatus(true);
		}

		public void OnClick_StopSequence()
		{
			modelAnimation.ChangePlayStatus(false);
		}

		public void OnClick_Save()
		{
			if (inputFieldTaskName.text != "" &&
				transferDropdownPlaceA.options[transferDropdownPlaceA.value].text != "None" &&
				transferDropdownPlaceB.options[transferDropdownPlaceB.value].text != "None" &&
				transferDropdownItem.options[transferDropdownItem.value].text != "None")
			{
				// GameTypes.Task tempTask =
				// 	GameTypes.GetTaskFromString(dropdownTaskType.options[dropdownTaskType.value].text);
				// string place1 = (transferDropdownPlaceA.options[transferDropdownPlaceA.value].text).Split('#')[^1];
				// string place2 = (transferDropdownPlaceB.options[transferDropdownPlaceB.value].text).Split('#')[^1];
				// GameTypes.Item tempItem =
				// 	GameTypes.GetItemFromString(transferDropdownItem.options[transferDropdownItem.value].text);
				ContentTask task = Instantiate(prefabContentTask, parentContent).GetComponent<ContentTask>();
				modelAnimation.AddNewTask(inputFieldTaskName.text, task);
			}
		}

		public void OnClick_Cancel()
		{
			panelRedactor.SetActive(false);
		}

		private Place[] listA;
		public void OnDropdown_TypeTask()
		{
			if (dropdownTaskType.options[dropdownTaskType.value].text == "Transfer")
			{
				panelTransfer.SetActive(true);
				panelCraft.SetActive(false);
				panelCycle.SetActive(false);
				inputFieldTaskName.text = "Transfer";
				
				Place[] places = modelAnimation.GetAllPlaces();
				List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
				List<Place> list = new List<Place>();

				for (int x = 0; x < places.Length; x++)
				{
					optionDatas.Add(new TMP_Dropdown.OptionData(places[x].itemName + "#" + 
																places[x].type + "#" + 
																places[x].output + "#" + 
																places[x].gameObject.GetInstanceID()));
					list.Add(places[x]);
				}
				
				// SetupDropdownPlaces(transferDropdownPlaceB, modelAnimation.GetAllPlaces());
				transferDropdownPlaceB.options = new List<TMP_Dropdown.OptionData>()
					{ new TMP_Dropdown.OptionData("null") };
				// SetupDropdownItems(transferDropdownPlaceB, itemsStorage);
				transferDropdownItem.options = new List<TMP_Dropdown.OptionData>()
					{new TMP_Dropdown.OptionData("null")};

				listA = list.ToArray();
				modelAnimation.SetTypeTask(GameTypes.Task.Transfer);
				modelAnimation.SetPlaceA(listA[transferDropdownPlaceA.value]);

				transferDropdownPlaceA.options = optionDatas;
				transferDropdownPlaceA.RefreshShownValue();
				transferDropdownPlaceA.onValueChanged?.Invoke(0);
				
			} else if (dropdownTaskType.options[dropdownTaskType.value].text == "Craft")
			{
				panelTransfer.SetActive(false);
				panelCraft.SetActive(true);
				panelCycle.SetActive(false);
				inputFieldTaskName.text = "Craft";
			} else if (dropdownTaskType.options[dropdownTaskType.value].text == "Cycle")
			{
				panelTransfer.SetActive(false);
				panelCraft.SetActive(false);
				panelCycle.SetActive(true);
				inputFieldTaskName.text = "Cycle";
			}
		}

		private Craft[] listB;
		public void OnDropdown_SetPlaceA()
		{
			Craft[] crafts = modelAnimation.GetCraft(listA[0].output);
			List<TMP_Dropdown.OptionData> optionDataB = new List<TMP_Dropdown.OptionData>();
			List<Craft> list = new List<Craft>();
			
			for (int x = 0; x < crafts.Length; x++)
			{
				optionDataB.Add(new TMP_Dropdown.OptionData(crafts[x].itemName + "#" + 
															crafts[x].type + "#" + crafts[x].gameObject.GetInstanceID()));
				list.Add(crafts[x]);
			}
			
			if (crafts.Length == 0)
			{
				optionDataB.Add(new TMP_Dropdown.OptionData("None"));
			}
			
			modelAnimation.SetPlaceA(listA[transferDropdownPlaceA.value]);
			
			listB = list.ToArray();
			modelAnimation.SetPlaceB(listB.Length >= 1 ? listB[transferDropdownPlaceB.value] : null);
			modelAnimation.SetItem(listA[transferDropdownPlaceA.value].output);

			transferDropdownPlaceB.options = optionDataB;
			transferDropdownPlaceB.RefreshShownValue();
			transferDropdownPlaceB.onValueChanged?.Invoke(0);
			
			List<TMP_Dropdown.OptionData> optionDataItem = new List<TMP_Dropdown.OptionData>();
			optionDataItem.Add(new TMP_Dropdown.OptionData(listA[transferDropdownPlaceA.value].output.ToString()));
			transferDropdownItem.options = optionDataItem;
			transferDropdownItem.RefreshShownValue();
			transferDropdownItem.onValueChanged?.Invoke(0);
		}

		public void OnDropdown_SetPlaceB()
		{
			modelAnimation.SetPlaceB(listB.Length >= 1 ? listB[transferDropdownPlaceB.value] : null);
			modelAnimation.SetItem(listA[transferDropdownPlaceA.value].output);
		}
	}
}