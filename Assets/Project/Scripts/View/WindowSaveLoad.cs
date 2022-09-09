using UnityEngine;

public class WindowSaveLoad : MonoBehaviour
{
	[SerializeField] private ModelSaveLoad modelSaveLoad;
	
	[SerializeField] private ImgLoadSave[] imgLoadSaves;
	[SerializeField] private TMPro.TMP_InputField inputField;

	private int selectedScene;
	
	private void Start()
	{
		SettingsScene[] settingsScenes = modelSaveLoad.GetSaveScenes;
		for (int i = 0; i < imgLoadSaves.Length; i++)
		{
			if (settingsScenes[i].isSave)
			{
				imgLoadSaves[i].GetTextInfo.text = (settingsScenes[i].isSave ? "> " : "") + settingsScenes[i].sceneName;
			}
			else
			{
				imgLoadSaves[i].GetTextInfo.text = "Clear";
			}
		}
	}

	public void OnClick_SaveGame()
	{
		modelSaveLoad.PreSaveScene(modelSaveLoad.GetSaveScenes[selectedScene].sceneName);
	}

	public void OnClick_LoadGame()
	{
		modelSaveLoad.PreLoadScene(modelSaveLoad.GetSaveScenes[selectedScene].sceneName);
	}

	public void OnClick_SelectSave(int scene)
	{
		imgLoadSaves[selectedScene].GetImg.color = Color.white;
		selectedScene = scene;
		imgLoadSaves[scene].GetImg.color = Color.green;
	}

	public void OnClick_Rename()
	{
		modelSaveLoad.GetSaveScenes[selectedScene].sceneName = inputField.text;
		imgLoadSaves[selectedScene].GetTextInfo.text = 
			(modelSaveLoad.GetSaveScenes[selectedScene].isSave ? "> " : "") + inputField.text;
	}
}
