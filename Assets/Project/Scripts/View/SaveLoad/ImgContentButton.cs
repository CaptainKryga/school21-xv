using UnityEngine;
using UnityEngine.UI;

public class ImgContentButton : MonoBehaviour
{
	[SerializeField] private Image img;
	[SerializeField] private TMPro.TMP_Text textInfo;
	[SerializeField] private Button button;
	
	public Image GetImg { get => img; }
	public TMPro.TMP_Text GetTextInfo { get => textInfo; }
	public Button GetButton { get => button; }
}
