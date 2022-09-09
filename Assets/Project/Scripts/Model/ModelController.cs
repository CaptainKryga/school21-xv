using System;
using Project.Scripts.Model;
using Project.Scripts.Utils;
using UnityEngine;

public class ModelController : MonoBehaviour
{
	[SerializeField] private ControllerGame game;
	[SerializeField] private ControllerView view;

	[SerializeField] private Player player;
	[SerializeField] private Worker worker;
		
	//глобальный стейт текущей стадии игры
	[SerializeField] private GameTypes.Game stateGame;
	
	public GameTypes.Game GetStateGame { get => stateGame; }

	private void OnEnable()
	{
		game.Keyboard_Action += ReceiveKeyboardActions;

		game.PlayerMove_Action += player.ReceivePlayerMoveActions;
		game.PlayerAxis_Action += player.ReceivePlayerAxisActions;
	}

	private void OnDisable()
	{
		game.Keyboard_Action -= ReceiveKeyboardActions;
		
		game.PlayerMove_Action -= player.ReceivePlayerMoveActions;
		game.PlayerAxis_Action -= player.ReceivePlayerAxisActions;
	}

	private void ReceiveKeyboardActions(KeyCode key)
	{
		if (key == KeyCode.Tab)
		{
			view.ChangeVisibleGlobalPanel();
		}
		if (key == KeyCode.V)
		{
			if (player.IsPlay())
				view.UpdatePlayerTypeMove(player.ChangeStatePlayerMove() + "");
		}
	}

	public void UpdateState(GameTypes.Game state)
	{
		stateGame = state;

		if (state == GameTypes.Game.Play)
		{
			player.ChangeStateIsPlay(true);
		}
		
		Debug.Log("state: " + state);
	}
}
