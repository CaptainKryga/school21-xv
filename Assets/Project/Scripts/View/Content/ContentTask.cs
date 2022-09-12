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
	
	public GameTypes.Task Type { get => type; }
	public Place PlaceA { get => placeA; }
	public Place PlaceB { get => placeB; }
	public GameTypes.Item Item { get => item; }
	public float Speed { get => speed; }

	public void InitTask(string taskName, Func<ContentTask, bool, int> func, GameTypes.Task type, Place placeA, Place placeB,
		GameTypes.Item item, float speed)
	{
		btnUp.onClick.AddListener(delegate { func(this, true); });
		btnDown.onClick.AddListener(delegate { func(this, false); });

		Debug.Log(taskName);
		Debug.Log(type);
		Debug.Log(placeA);
		Debug.Log(placeB);
		Debug.Log(item);
		
		textNameTask.text = taskName;
		this.type = type;
		this.placeA = placeA;
		this.placeB = placeB;
		this.item = item;
		this.speed = speed;
	}

	public void OnSlider_ChangeSpeed()
	{
		speed = slider.value;
	}
}
