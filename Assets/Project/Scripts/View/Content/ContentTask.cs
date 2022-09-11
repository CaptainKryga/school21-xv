using System;
using UnityEngine;
using UnityEngine.UI;

public class ContentTask : MonoBehaviour
{
	[SerializeField] private TMPro.TMP_Text infoText;
	[SerializeField] private Button btnUp, btnDown;

	public void InitButtons(Func<ContentTask, bool, int> func)
	{
		btnUp.onClick.AddListener(delegate { func(this, true); });
		btnDown.onClick.AddListener(delegate { func(this, false); });
	}
}
