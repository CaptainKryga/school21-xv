using UnityEngine;

public class ControllerView : MonoBehaviour
{
	[SerializeField] private ControllerGame game;
	[SerializeField] private ControllerModel model;

	[SerializeField] private PlayerInfo playerInfo;
	
	[SerializeField] private GameObject panelGlobalPanel;
	[SerializeField] private GameObject panelImportExport;
	[SerializeField] private GameObject panelChangeScene;
	[SerializeField] private GameObject panelAnimations;
	[SerializeField] private GameObject panelSaveLoad;
	[SerializeField] private GameObject panelVideo;

	private void Start()
	{
		panelGlobalPanel.SetActive(true);
	}

	public void ChangeVisibleGlobalPanel(bool playerIsPlay)
	{
		panelGlobalPanel.SetActive(!playerIsPlay);
	}

	public void UpdatePlayerTypeMove(string type)
	{
		playerInfo.UpdatePlayerTypeMove(type);
	}

	#region Global Button's

	public void OnClick_OpenImportExport()
	{
		DisableOpenPanels();
		panelImportExport.SetActive(true);
	}
	public void OnClick_OpenChangeScene()
	{
		DisableOpenPanels();
		panelChangeScene.SetActive(true);
	}
	public void OnClick_OpenAnimations()
	{
		DisableOpenPanels();
		panelAnimations.SetActive(true);
	}	
	public void OnClick_OpenSaveLoad()
	{
		DisableOpenPanels();
		panelSaveLoad.SetActive(true);
	}	
	public void OnClick_OpenVideo()
	{
		DisableOpenPanels();
		panelVideo.SetActive(true);
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

}
