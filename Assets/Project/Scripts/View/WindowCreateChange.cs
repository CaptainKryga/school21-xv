using Project.Scripts.Model;
using Project.Scripts.Model.CreateChange;
using UnityEngine;
using UnityEngine.UI;

public class WindowCreateChange : MonoBehaviour
{
	[SerializeField] private ControllerGame game;
	[SerializeField] private ControllerView view;
	[SerializeField] private ModelCreate modelCreate;
	[SerializeField] private ModelChange modelChange;
	[SerializeField] private ModelSaveLoad modelSaveLoad;
	[SerializeField] private DataBase dataBase;
	
	//Create
	[SerializeField] private GameObject panelCreate;
	[SerializeField] private GameObject panelChange;
	//color
	[SerializeField] private Texture2D texture2DGradient;
	[SerializeField] private Image imgTarget;
	[SerializeField] private Image imgSelectColorGradient;
	[SerializeField] private Image imgColorGradient;
	
	//prefabs
	[SerializeField] private Transform parentContent;
	[SerializeField] private GameObject imgContentButton;

	public GameObject PanelChange { get => panelChange; }
	public GameObject PanelCreate { get => panelCreate; }

	private ImgContentButton[] saveButtons;
	private int selectedId = -1;
	
	
	//Change
	[SerializeField] private TMPro.TMP_InputField inputFieldScene;
	[SerializeField] private TMPro.TMP_InputField inputFieldObject;

	private void Start()
	{
		saveButtons = new ImgContentButton[dataBase.defaultPrefabs.Length];
		for (int i = 0; i < dataBase.defaultPrefabs.Length; i++)
		{
			GameObject newItem = Instantiate(imgContentButton, parentContent);
			saveButtons[i] = newItem.GetComponent<ImgContentButton>();
			saveButtons[i].GetTextInfo.text = dataBase.defaultPrefabs[i].GetComponent<Item>().itemName;
			var i1 = i;
			saveButtons[i].GetButton.onClick.AddListener(delegate { OnClick_SelectItem(i1); });
		}
		
		inputFieldScene.text = modelChange.GetSceneName();
	}

	public void OnClick_OpenPanelChange()
	{
		panelChange.SetActive(true);
		panelCreate.SetActive(false);
		game.UpdateState(view.GetNowState());
		
		//update sceneName
		inputFieldScene.text = modelChange.GetSceneName();
	}

	public void OnClick_OpenPanelCreate()
	{
		panelChange.SetActive(false);
		panelCreate.SetActive(true);
		game.UpdateState(view.GetNowState());
	}

	public void OnClick_SelectItem(int id)
	{
		if (selectedId != -1)
			saveButtons[selectedId].GetImg.color = Color.white;
		selectedId = id;
		modelCreate.SelectedId = id;
		saveButtons[selectedId].GetImg.color = Color.green;
	}

	public void OnClick_RenameScene()
	{
		modelSaveLoad.Rename(modelChange.GetSceneName(), inputFieldScene.text);
	}



	#region Change

	public void SetItemName(string itemName)
	{
		inputFieldObject.text = itemName;
	}

	public void OnClick_RenameItem()
	{
		modelChange.RenameNowSelectedItemName(inputFieldObject.text);
	}

	public void OnClick_DeleteItem()
	{
		modelChange.DeleteNowSelectedItem();
	}
	
	//тут магия
	public void Onclick_GetColorGradient()
	{
		Vector3 posGra = imgColorGradient.rectTransform.position;
		Vector3 posKra = new Vector3(posGra.x - imgColorGradient.rectTransform.sizeDelta.x / 2,
			posGra.y - imgColorGradient.rectTransform.sizeDelta.y / 2, 0);
		
		int x = (100 + (int)Input.mousePosition.x - (int)posGra.x) * 5;
		int y = (100 + (int)Input.mousePosition.y - (int)posGra.y) * 5;
		
		imgSelectColorGradient.color = texture2DGradient.GetPixel(x, y);
	}

	public void OnClick_ResetColor()
	{
		imgSelectColorGradient.color = Color.white;
	}
	
	public void OnClick_SetColor()
	{
		modelChange.SetColorNowSelectedItem(imgSelectColorGradient.color);
	}
	
	#endregion
}
