using UnityEngine;

public class WindowSaveLoad : MonoBehaviour
{
	[SerializeField] private ModelSaveLoad modelSaveLoad;

	[SerializeField] private Transform parentContent;
	[SerializeField] private GameObject prafabImgContentButton;
	[SerializeField] private TMPro.TMP_InputField inputField;

	private ContentScene[] imgContentButtons;

	private int selectedScene = -1;

	private void Start()
	{
		UpdateContent();
	}

	public void OnClick_SaveGame()
	{
		if (selectedScene != -1)
			modelSaveLoad.PreSaveScene(imgContentButtons[selectedScene].GetTextInfo.text);
		else if (inputField.text != "")
			modelSaveLoad.PreSaveScene(inputField.text);
		else
			Debug.LogError("ВВЕДИТЕ ИМЯ ДЛЯ СОХРАНЕНИЯ");
	}

	public void OnClick_LoadGame()
	{
		if (selectedScene != -1)
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
		if (selectedScene == -1)
		{
			Debug.LogError("ВЫБЕРИТЕ СОХРАНЕНИЕ");
			return;
		}

		if (modelSaveLoad.Rename(imgContentButtons[selectedScene].GetTextInfo.text, inputField.text))
		{
			imgContentButtons[selectedScene].GetTextInfo.text = inputField.text;
		
			UpdateContent();
		}
	}

	public void UpdateContent()
	{
		for (int i = 0; i < parentContent.childCount; i++)
		{
			Destroy(parentContent.GetChild(i).gameObject);
		}
		
		selectedScene = -1;
		
		UpdateListSaveLoad();
	}

	private void UpdateListSaveLoad()
	{
		string[] lastScanSaveFiles = modelSaveLoad.LastScanSaveFiles;
		imgContentButtons = new ContentScene[lastScanSaveFiles.Length];
		for (int i = 0; i < lastScanSaveFiles.Length; i++)
		{
			ContentScene contentScene = 
				Instantiate(prafabImgContentButton, parentContent).GetComponent<ContentScene>();
			contentScene.GetTextInfo.text = lastScanSaveFiles[i];
			imgContentButtons[i] = contentScene;
			
			int i1 = i;
			imgContentButtons[i].GetButton.onClick.AddListener(delegate { OnClick_SelectSave(i1); });
		}
	}
}
