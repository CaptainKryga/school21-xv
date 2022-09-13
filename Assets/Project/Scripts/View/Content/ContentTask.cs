using System;
using System.Collections.Generic;
using Animation;
using Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

public class ContentTask : MonoBehaviour
{
	[SerializeField] private Image background;
	[SerializeField] private Button btnUp, btnDown, btnDestroy;
	[SerializeField] private Slider slider;

	[SerializeField] private TMPro.TMP_Text textNameTask;
	private GameTypes.Task type;
	private string description;
	private Place placeA, placeB;
	private GameTypes.Item item;
	private float speed;
	[SerializeField] private Color[] colorTask;
	
	//cycle
	private int iterations, nowIterations;
	private ContentTask parentTask;
	private ContentTask childTask;
	private List<ContentTask> container = new List<ContentTask>();

	public GameTypes.Task Type { get => type; }
	public Place PlaceA { get => placeA; }
	public Place PlaceB { get => placeB; }
	public GameTypes.Item Item { get => item; }
	public float Speed { get => speed; }
	public string Description { get => description; }
	public string TextNameTask { get => textNameTask.text; }
	
	public int Iterations { get => iterations; }
	public int NowIterations { get => nowIterations; set => nowIterations = value; }
	public ContentTask ParentTask { get => parentTask; }
	public ContentTask ChildTask { get => childTask; }
	public List<ContentTask> Container { get => container; }


	public void InitTask(string taskName, string description, GameTypes.Task type, 
		Place placeA, Place placeB, GameTypes.Item item, float speed, int iterations)
	{

		Debug.Log(taskName);
		Debug.Log(type);
		Debug.Log(placeA);
		Debug.Log(placeB);
		Debug.Log(item);
		
		textNameTask.text = taskName;
		this.type = type;
		this.description = description;
		this.placeA = placeA;
		this.placeB = placeB;
		this.item = item;
		this.speed = speed;
		this.iterations = iterations;
		
		UpdateColorTask();
		DisableSlider();
	}

	public void InitButtons(Func<ContentTask, bool, int> func, Func<ContentTask, int> func2)
	{
		btnUp.onClick.AddListener(delegate { func(this, true); });
		btnDown.onClick.AddListener(delegate { func(this, false); });
		btnDestroy.onClick.AddListener(delegate { func2(this); });
	}

	public void InitWhile(int iterations, ContentTask subTaskEnd, ContentTask parentTask)
	{
		this.iterations = iterations;
		this.parentTask = parentTask;
		this.childTask = subTaskEnd;

		if (parentTask != null)
		{
			textNameTask.text = "end while task";
			type = GameTypes.Task.Cycle;
		}
		else
		{
			textNameTask.text = "while task  0/" + iterations;
		}

		UpdateColorTask();
		DisableSlider();
	}

	public void OnSlider_ChangeSpeed()
	{
		speed = slider.value;
	}

	private void UpdateColorTask()
	{
		if (type == GameTypes.Task.Transfer)
			background.color = colorTask[0];
		else if (type == GameTypes.Task.Craft)
			background.color = colorTask[1];
		else if (type == GameTypes.Task.Cycle)
			background.color = colorTask[2];
	}

	public void UpdateColor(bool isUse)
	{
		Color c = background.color;
		if (isUse)
			background.color = new Color(c.r, c.g, c.b, 1);
		else
			background.color = new Color(c.r, c.g, c.b, colorTask[0].a);
	}
	
	private void DisableSlider()
	{
		if (type == GameTypes.Task.Cycle)
			slider.gameObject.SetActive(false);
	}

	public void UpdateInfo()
	{
		textNameTask.text = "while " + (iterations - nowIterations) + "/" + iterations;
	}
}
