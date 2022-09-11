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
			
		}

		public void OnClick_StopSequence()
		{
			
		}

		public void OnClick_Save()
		{
			if (inputFieldTaskName.text != "" &&
				transferDropdownPlaceA.options[transferDropdownPlaceA.value].text != "None" &&
				transferDropdownPlaceB.options[transferDropdownPlaceB.value].text != "None" &&
				transferDropdownItem.options[transferDropdownItem.value].text != "None")
			{
				GameTypes.Task tempTask =
					GameTypes.GetTaskFromString(dropdownTaskType.options[dropdownTaskType.value].text);
				string place1 = (transferDropdownPlaceA.options[transferDropdownPlaceA.value].text).Split('#')[^1];
				string place2 = (transferDropdownPlaceB.options[transferDropdownPlaceB.value].text).Split('#')[^1];
				GameTypes.Item tempItem =
					GameTypes.GetItemFromString(transferDropdownItem.options[transferDropdownItem.value].text);
				ContentTask task = Instantiate(prefabContentTask, parentContent).GetComponent<ContentTask>();
				modelAnimation.AddNewTask(inputFieldTaskName.text, task, tempTask, Int32.Parse(place1),
					Int32.Parse(place2), tempItem);
			}
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
				inputFieldTaskName.text = "Transfer";
				
				Place[] places = modelAnimation.GetAllPlaces();
				List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();

				for (int x = 0; x < places.Length; x++)
				{
					optionDatas.Add(new TMP_Dropdown.OptionData(places[x].itemName + "#" + 
																places[x].type + "#" + 
																places[x].output + "#" + 
																places[x].gameObject.GetInstanceID()));
				}
				
				// SetupDropdownPlaces(transferDropdownPlaceB, modelAnimation.GetAllPlaces());
				transferDropdownPlaceB.options = new List<TMP_Dropdown.OptionData>()
					{ new TMP_Dropdown.OptionData("null") };
				// SetupDropdownItems(transferDropdownPlaceB, itemsStorage);
				transferDropdownItem.options = new List<TMP_Dropdown.OptionData>()
					{new TMP_Dropdown.OptionData("null")};
				
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

		public void OnDropdown_SetPlaceA()
		{
			string[] dropA = transferDropdownPlaceA.options[transferDropdownPlaceA.value].text.Split('#');
			Craft[] crafts = modelAnimation.GetCraft(GameTypes.GetItemFromString(dropA[^2]));
			List<TMP_Dropdown.OptionData> optionDataB = new List<TMP_Dropdown.OptionData>();
			List<GameTypes.Item> inputItems = new List<GameTypes.Item>();

			for (int x = 0; x < crafts.Length; x++)
			{
				optionDataB.Add(new TMP_Dropdown.OptionData(crafts[x].itemName + "#" + 
															crafts[x].type + "#" + crafts[x].gameObject.GetInstanceID()));
			}

			if (crafts.Length == 0)
			{
				optionDataB.Add(new TMP_Dropdown.OptionData("None"));
			}

			transferDropdownPlaceB.options = optionDataB;
			transferDropdownPlaceB.RefreshShownValue();
			transferDropdownPlaceB.onValueChanged?.Invoke(0);


			List<TMP_Dropdown.OptionData> optionDataItem = new List<TMP_Dropdown.OptionData>();
			optionDataItem.Add(new TMP_Dropdown.OptionData(GameTypes.GetItemFromString(dropA[^2]).ToString()));
			transferDropdownItem.options = optionDataItem;
			transferDropdownItem.RefreshShownValue();
			transferDropdownItem.onValueChanged?.Invoke(0);
		}

		public void OnDropdown_SetPlaceB()
		{

		}
	}
}