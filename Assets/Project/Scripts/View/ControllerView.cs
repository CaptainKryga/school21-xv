using System;
using Project.Scripts.Utils;
using UnityEngine;

public class ControllerView : MonoBehaviour
{
	[SerializeField] private ControllerGame game;
	[SerializeField] private ModelController model;

	[SerializeField] private PlayerInfo playerInfo;
	[SerializeField] private WindowChangeScene wChangeScene;
	
	[SerializeField] private GameObject panelGlobalPanel;
	[SerializeField] private GameObject panelImportExport;
	[SerializeField] private GameObject panelChangeScene;
	[SerializeField] private GameObject panelAnimations;
	[SerializeField] private GameObject panelSaveLoad;
	[SerializeField] private GameObject panelVideo;

	private void Awake()
	{
		panelGlobalPanel.SetActive(true);
		panelImportExport.SetActive(true);
		panelChangeScene.SetActive(true);
		wChangeScene.PanelChange.SetActive(true);
		wChangeScene.PanelCreate.SetActive(true);
		panelAnimations.SetActive(true);
		panelSaveLoad.SetActive(true);
		panelVideo.SetActive(true);
	}

	private void Start()
	{
		panelGlobalPanel.SetActive(false);
		panelImportExport.SetActive(false);
		panelChangeScene.SetActive(false);
		wChangeScene.PanelChange.SetActive(false);
		wChangeScene.PanelCreate.SetActive(false);
		panelAnimations.SetActive(false);
		panelSaveLoad.SetActive(false);
		panelVideo.SetActive(false);
		game.UpdateState(GetNowState());
	}

	public void ChangeVisibleGlobalPanel()
	{
		panelGlobalPanel.SetActive(!panelGlobalPanel.activeSelf);
		game.UpdateState(GetNowState());
	}

	public void UpdatePlayerTypeMove(string type)
	{
		playerInfo.UpdatePlayerTypeMove(type);
		game.UpdateState(GetNowState());
	}

	#region Global Button's

	public void OnClick_OpenImportExport()
	{
		DisableOpenPanels();
		panelImportExport.SetActive(true);
		game.UpdateState(GetNowState());
	}
	public void OnClick_OpenChangeScene()
	{
		DisableOpenPanels();
		panelChangeScene.SetActive(true);
		game.UpdateState(GetNowState());
	}
	public void OnClick_OpenAnimations()
	{
		DisableOpenPanels();
		panelAnimations.SetActive(true);
		game.UpdateState(GetNowState());
	}	
	public void OnClick_OpenSaveLoad()
	{
		DisableOpenPanels();
		panelSaveLoad.SetActive(true);
		game.UpdateState(GetNowState());
	}	
	public void OnClick_OpenVideo()
	{
		DisableOpenPanels();
		panelVideo.SetActive(true);
		game.UpdateState(GetNowState());
	}
	private void DisableOpenPanels()
	{
		panelImportExport.SetActive(false);
		panelChangeScene.SetActive(false);
		panelAnimations.SetActive(false);
		panelSaveLoad.SetActive(false);
		panelVideo.SetActive(false);
	}
	
	#endregion

	private GameTypes.Game GetNowState()
	{
		if (!panelGlobalPanel.activeSelf)
			return GameTypes.Game.Play;
		else if (panelChangeScene)
		{
			Debug.Log(wChangeScene);
			Debug.Log(wChangeScene.PanelChange);
			Debug.Log(wChangeScene.PanelChange.activeSelf);
			if (wChangeScene.PanelChange.activeSelf)
				return GameTypes.Game.Change;
			else if (wChangeScene.PanelCreate.activeSelf)
				return GameTypes.Game.Create;
		}
		else if (panelImportExport.activeSelf)
			return GameTypes.Game.SceneImportExport;
		else if (panelAnimations.activeSelf)
			return GameTypes.Game.Animations;
		else if (panelSaveLoad.activeSelf)
			return GameTypes.Game.GameSaveLoad;
		else if (panelVideo.activeSelf)
			return GameTypes.Game.Video;

		return GameTypes.Game.Null;
	}
}
