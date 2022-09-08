using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowChangeScene : MonoBehaviour
{
	[SerializeField] private GameObject panel;
	[SerializeField] private GameObject panelCreate;
	[SerializeField] private GameObject panelChange;

	public void OnClick_OpenPanelChange()
	{
		panelChange.SetActive(true);
		panelCreate.SetActive(false);
	}

	public void OnClick_OpenPanelCreate()
	{
		panelChange.SetActive(false);
		panelCreate.SetActive(true);
	}
	
	
}
