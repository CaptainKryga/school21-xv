using UnityEngine;
using UnityEngine.UI;

public class WindowChangeScene : MonoBehaviour
{
	[SerializeField] private ControllerGame game;
	[SerializeField] private ControllerView view;
	
	[SerializeField] private GameObject panelCreate;
	[SerializeField] private GameObject panelChange;
	//color
	[SerializeField] private Texture2D texture2DGradient;
	[SerializeField] private Image imgTarget;
	[SerializeField] private Image imgSelectColorGradient;
	[SerializeField] private Image imgColorGradient;

	public GameObject PanelChange { get => panelChange; }
	public GameObject PanelCreate { get => panelCreate; }
	
	public void OnClick_OpenPanelChange()
	{
		panelChange.SetActive(true);
		panelCreate.SetActive(false);
		game.UpdateState(view.GetNowState());
	}

	public void OnClick_OpenPanelCreate()
	{
		panelChange.SetActive(false);
		panelCreate.SetActive(true);
		game.UpdateState(view.GetNowState());
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
}
