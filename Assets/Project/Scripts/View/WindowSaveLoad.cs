using Project.Scripts.Utils;
using UnityEngine;

public class WindowSaveLoad : MonoBehaviour
{
	[SerializeField] private ModelSaveLoad modelSaveLoad;

	[SerializeField] private Transform parentContent;
	[SerializeField] private GameObject prafabImgContentButton;
	[SerializeField] private TMPro.TMP_InputField inputField;

	private ImgContentButton[] imgContentButtons;

	private int selectedScene = -1;
	
	private void Start()
	{
		string[] lastScanSaveFiles = modelSaveLoad.LastScanSaveFiles;
		imgContentButtons = new ImgContentButton[lastScanSaveFiles.Length];
		for (int i = 0; i < lastScanSaveFiles.Length; i++)
		{
			ImgContentButton imgContentButton = 
				Instantiate(prafabImgContentButton, parentContent).GetComponent<ImgContentButton>();
			imgContentButton.GetTextInfo.text = "> " + lastScanSaveFiles[i];
			imgContentButtons[i] = imgContentButton;
			
			int i1 = i;
			imgContentButtons[i].GetButton.onClick.AddListener(delegate { OnClick_SelectSave(i1); });
		}
	}

	public void OnClick_SaveGame()
	{
		modelSaveLoad.PreSaveScene(imgContentButtons[selectedScene].GetTextInfo.text);
	}

	public void OnClick_LoadGame()
	{
		modelSaveLoad.PreLoadScene(imgContentButtons[selectedScene].GetTextInfo.text);
	}

	public void OnClick_SelectSave(int scene)
	{
		if (selectedScene != -1)
			imgContentButtons[selectedScene].GetImg.color = Color.white;
		selectedScene = scene;
		imgContentButtons[scene].GetImg.color = Color.green;
	}

	public void OnClick_Rename()
	{
		imgContentButtons[selectedScene].sceneName = inputField.text;
		imgContentButtons[selectedScene].GetTextInfo.text = 
			(imgContentButtons[selectedScene].isSave ? "> " : "") + inputField.text;
	}
}
