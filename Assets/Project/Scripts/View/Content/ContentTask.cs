using System;
using Animation;
using Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

public class ContentTask : MonoBehaviour
{
	[SerializeField] private Button btnUp, btnDown;
	[SerializeField] private Slider slider;

	[SerializeField] private TMPro.TMP_Text textNameTask;
	private GameTypes.Task type;
	private Place placeA, placeB;
	private GameTypes.Item item;
	private float speed;

	public void InitTask(string taskName, Func<ContentTask, bool, int> func, GameTypes.Task type, Place placeA, Place placeB,
		GameTypes.Item item)
	{
		btnUp.onClick.AddListener(delegate { func(this, true); });
		btnDown.onClick.AddListener(delegate { func(this, false); });

		textNameTask.text = taskName;
		this.type = type;
		this.placeA = placeA;
		this.placeB = placeB;
		this.item = item;
	}

	public void OnSlider_ChangeSpeed()
	{
		speed = slider.value;
	}
}
