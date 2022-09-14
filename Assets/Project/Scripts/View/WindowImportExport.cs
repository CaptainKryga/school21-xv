using Project.Scripts.Model.ImportExport;
using UnityEditor;
using UnityEngine;

public class WindowImportExport : MonoBehaviour
{
	[SerializeField] private ModelImportExport modelImportExport;

	[SerializeField] private Transform parentContent;
	[SerializeField] private GameObject prafabImgContentButton;
	[SerializeField] private TMPro.TMP_InputField inputFieldPath;
	[SerializeField] private TMPro.TMP_InputField inputFieldName;

	private ContentScene[] imgContentButtons;

	private int selectedFile = -1;

	private void Start()
	{
		UpdateContent();
	}

	public void OnClick_ExportScene()
	{
		// string path = EditorUtility.SaveFilePanel("ff", "", "default","import");
		// Debug.Log(path);
		// modelImportExport.PreExportScene(path);
		
		// if (selectedFile != -1)
			// modelImportExport.PreExportScene(imgContentButtons[selectedFile].GetTextInfo.text);
		// else if (inputField.text != "")
			// modelImportExport.PreExportScene(inputField.text);
		// else
			// Debug.LogError("ВВЕДИТЕ ИМЯ ДЛЯ EXPORT'a");
	}

	public void OnClick_ImportScene()
	{
		if (selectedFile == 0)
		{
			modelImportExport.UpdatePathToImportExportDirectory(true);
		} 
		else if (selectedFile < 9999)
		{
			modelImportExport.UpdatePathToImportExportDirectory(false, 
				imgContentButtons[selectedFile].GetTextInfo.text);
		}
		
		UpdateContent();
		// string path = EditorUtility.OpenFilePanel("ff", "", "import");
		// Debug.Log(path);
		// modelImportExport.PreImportScene(path);
	}

	public void OnClick_SelectExport(int scene)
	{
		if (selectedFile != -1)
			imgContentButtons[selectedFile].GetImg.color = Color.white;
		if (scene > 9999)
			scene -= 10000;
		selectedFile = scene;
		imgContentButtons[scene].GetImg.color = Color.green;
	}

	public void OnClick_RenameScene()
	{
		// if (selectedFile == -1)
		// {
			// Debug.LogError("ВЫБЕРИТЕ FILE");
			// return;
		// }

		// if (modelImportExport.Rename(imgContentButtons[selectedFile].GetTextInfo.text, inputFieldName.text))
		// {
			// imgContentButtons[selectedFile].GetTextInfo.text = inputFieldName.text;
		
			// UpdateContent();
		// }
	}

	public void UpdateContent()
	{
		for (int i = 0; i < parentContent.childCount; i++)
		{
			Destroy(parentContent.GetChild(i).gameObject);
		}
		
		selectedFile = -1;
		
		inputFieldPath.text = modelImportExport.PathToImportExportDirectory;
		UpdateListImportExport();
	}

	private void UpdateListImportExport()
	{
		string[] directories = modelImportExport.LastScanSaveDirectories;
		string[] files = modelImportExport.LastScanSaveFiles;
		imgContentButtons = new ContentScene[1 + directories.Length + files.Length];
		int i = 0;
		
		//cd ..
		if (modelImportExport.PathToImportExportDirectory != "/")
		{
			ContentScene content = Instantiate(prafabImgContentButton, parentContent).GetComponent<ContentScene>();
			content.GetTextInfo.text = "..";
			imgContentButtons[i] = content;
			var i1 = i;
			imgContentButtons[i].GetButton.onClick.AddListener(delegate { OnClick_SelectExport(i1); });
			i++;
		}
		
		//directories
		for (int x = 0; x < directories.Length; x++, i++)
		{
			Debug.Log("directories:" + directories[x]);
			
			ContentScene content = Instantiate(prafabImgContentButton, parentContent).GetComponent<ContentScene>();
			content.GetTextInfo.text = directories[x];
			imgContentButtons[i] = content;
			
			int i2 = i;
			imgContentButtons[i].GetButton.onClick.AddListener(delegate { OnClick_SelectExport(i2); });
		}
		
		//files + 10000
		for (int x = 0; x < files.Length; x++, i++)
		{
			Debug.Log("files:" + files[x]);
			ContentScene content = Instantiate(prafabImgContentButton, parentContent).GetComponent<ContentScene>();
			content.GetTextInfo.text = files[x];
			imgContentButtons[i] = content;
			
			int i2 = i + 10000;
			imgContentButtons[i].GetButton.onClick.AddListener(delegate { OnClick_SelectExport(i2); });
		}
	}
}