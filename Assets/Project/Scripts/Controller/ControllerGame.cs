using System;
using Project.Scripts.Utils;
using UnityEngine;

public class ControllerGame : MonoBehaviour
{
	public static ControllerGame Singelton { get; private set; }

	//все нажатия исключительно для движения игрока
	public Action<Vector2> PlayerMove_Action;
	public Action<Vector2> PlayerAxis_Action;
	//все нажатия с клавиатуры
	public Action<KeyCode> Keyboard_Action;
	
	//глобальный стейт текущей стадии игры
	public GameTypes.Game stateGame;
	
	private void Awake()
	{
		Singelton = this;
		stateGame = GameTypes.Game.Pause;
	}
	
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
			Keyboard_Action?.Invoke(KeyCode.Tab);
		if (Input.GetKeyDown(KeyCode.V))
			Keyboard_Action?.Invoke(KeyCode.V);
		
		
		PlayerMove_Action?.Invoke(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
		PlayerAxis_Action?.Invoke(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
	}
}
