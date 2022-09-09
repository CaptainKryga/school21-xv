using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImgLoadSave : MonoBehaviour
{
	[SerializeField] private Image img;
	[SerializeField] private TMPro.TMP_Text textInfo;
	
	public Image GetImg { get => img; }
	public TMPro.TMP_Text GetTextInfo { get => textInfo; }
}
