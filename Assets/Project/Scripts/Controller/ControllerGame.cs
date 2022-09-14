using System;
using Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerGame : MonoBehaviour
{
	[SerializeField] private ModelController model;

	//все нажатия исключительно для движения игрока
	public Action<Vector2> PlayerMove_Action;
	public Action<Vector2> PlayerAxis_Action;
	public Action<float> MouseScroll_Action;
	//все нажатия с клавиатуры
	public Action<KeyCode> Keyboard_Action;
	//все нажатия с мыши
	public Action<KeyCode> Mouse_Action;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
			Keyboard_Action?.Invoke(KeyCode.Tab);
		
		if (model.GetStateGame == GameTypes.Game.Play)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
				Keyboard_Action?.Invoke(KeyCode.Alpha1);
			if (Input.GetKeyDown(KeyCode.Alpha2))
				Keyboard_Action?.Invoke(KeyCode.Alpha2);
			if (Input.GetKeyDown(KeyCode.Alpha3))
				Keyboard_Action?.Invoke(KeyCode.Alpha3);
			if (Input.GetKeyDown(KeyCode.Alpha4))
				Keyboard_Action?.Invoke(KeyCode.Alpha4);
			if (Input.GetKeyDown(KeyCode.Alpha5))
				Keyboard_Action?.Invoke(KeyCode.Alpha5);
		}
		
		if (Input.GetKeyDown(KeyCode.Mouse0))
			Mouse_Action?.Invoke(KeyCode.Mouse0);
		if (Input.GetKeyDown(KeyCode.Mouse1))
			Mouse_Action?.Invoke(KeyCode.Mouse1);

		if (model.GetStateGame == GameTypes.Game.Create ||
			model.GetStateGame == GameTypes.Game.Change)
		{
			if (Input.GetKeyDown(KeyCode.LeftShift))
				Keyboard_Action?.Invoke(KeyCode.LeftShift);
			MouseScroll_Action?.Invoke(Input.GetAxis("Mouse ScrollWheel"));
		}
		
		
		PlayerMove_Action?.Invoke(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
		PlayerAxis_Action?.Invoke(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));


		if (Input.GetKeyDown(KeyCode.Escape))
			SceneManager.LoadScene(0);
	}

	public void UpdateState(GameTypes.Game state)
	{
		model.UpdateGameState(state);
	}
}
