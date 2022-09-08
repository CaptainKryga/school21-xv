using System;
using UnityEngine;

public class ControllerView : MonoBehaviour
{
	[SerializeField] private ControllerGame game;
	[SerializeField] private ControllerModel model;

	[SerializeField] private GameObject panelGlobalPanel;

	private void Start()
	{
		panelGlobalPanel.SetActive(true);
	}

	public void ChangeVisibleGlobalPanel(bool playerIsPlay)
	{
		panelGlobalPanel.SetActive(!playerIsPlay);
	}
}
