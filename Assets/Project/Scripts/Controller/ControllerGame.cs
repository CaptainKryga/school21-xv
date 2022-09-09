using System;
using Project.Scripts.Utils;
using UnityEngine;

public class ControllerGame : MonoBehaviour
{
	[SerializeField] private ModelController model;

	//все нажатия исключительно для движения игрока
	public Action<Vector2> PlayerMove_Action;
	public Action<Vector2> PlayerAxis_Action;
	//все нажатия с клавиатуры
	public Action<KeyCode> Keyboard_Action;
	//все нажатия с мыши
	public Action<KeyCode> Mouse_Action;

	private void Start()
	{
		
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
			Keyboard_Action?.Invoke(KeyCode.Tab);
		if (Input.GetKeyDown(KeyCode.V))
			Keyboard_Action?.Invoke(KeyCode.V);
		
		if (Input.GetKeyDown(KeyCode.Mouse0))
			Mouse_Action?.Invoke(KeyCode.Mouse0);
		if (Input.GetKeyDown(KeyCode.Mouse1))
			Mouse_Action?.Invoke(KeyCode.Mouse1);
		
		
		PlayerMove_Action?.Invoke(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
		PlayerAxis_Action?.Invoke(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
	}

	public void UpdateState(GameTypes.Game state)
	{
		model.UpdateState(state);
	}
}
