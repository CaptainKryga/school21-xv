using Project.Scripts.Model.ImportExport;
using UnityEngine;

public class WindowImportExport : MonoBehaviour
{
	[SerializeField] private ModelImportExport modelImportExport;

	[SerializeField] private Transform parentContent;
	[SerializeField] private GameObject prafabImgContentButton;
	[SerializeField] private TMPro.TMP_InputField inputField;
	[SerializeField] private TMPro.TMP_Text textPath;

	private ImgContentButton[] imgContentButtons;

	private int selectedFile = -1;

	private void Start()
	{
		UpdateContent();

		textPath.text = "path to files: " +  modelImportExport.PathToImportExportDirectory;
	}

	public void OnClick_ExportScene()
	{
		if (selectedFile != -1)
			modelImportExport.PreExportScene(imgContentButtons[selectedFile].GetTextInfo.text);
		else if (inputField.text != "")
			modelImportExport.PreExportScene(inputField.text);
		else
			Debug.LogError("ВВЕДИТЕ ИМЯ ДЛЯ EXPORT'a");
	}

	public void OnClick_ImportScene()
	{
		if (selectedFile != -1)
			modelImportExport.PreImportScene(imgContentButtons[selectedFile].GetTextInfo.text);
	}

	public void OnClick_SelectExport(int scene)
	{
		if (selectedFile != -1)
			imgContentButtons[selectedFile].GetImg.color = Color.white;
		selectedFile = scene;
		imgContentButtons[scene].GetImg.color = Color.green;
	}

	public void OnClick_RenameScene()
	{
		if (selectedFile == -1)
		{
			Debug.LogError("ВЫБЕРИТЕ FILE");
			return;
		}

		if (modelImportExport.Rename(imgContentButtons[selectedFile].GetTextInfo.text, inputField.text))
		{
			imgContentButtons[selectedFile].GetTextInfo.text = inputField.text;
		
			UpdateContent();
		}
	}

	public void UpdateContent()
	{
		for (int i = 0; i < parentContent.childCount; i++)
		{
			Destroy(parentContent.GetChild(i).gameObject);
		}
		
		selectedFile = -1;
		
		UpdateListImportExport();
	}

	private void UpdateListImportExport()
	{
		string[] lastScanSaveFiles = modelImportExport.LastScanSaveFiles;
		imgContentButtons = new ImgContentButton[lastScanSaveFiles.Length];
		for (int i = 0; i < lastScanSaveFiles.Length; i++)
		{
			ImgContentButton imgContentButton = 
				Instantiate(prafabImgContentButton, parentContent).GetComponent<ImgContentButton>();
			imgContentButton.GetTextInfo.text = lastScanSaveFiles[i];
			imgContentButtons[i] = imgContentButton;
			
			int i1 = i;
			imgContentButtons[i].GetButton.onClick.AddListener(delegate { OnClick_SelectExport(i1); });
		}
	}
}