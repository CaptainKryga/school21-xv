using System;
using Project.Scripts.Model;
using UnityEngine;

public class ControllerModel : MonoBehaviour
{
	[SerializeField] private ControllerGame game;
	[SerializeField] private ControllerView view;

	[SerializeField] private Player player;
	[SerializeField] private Worker worker;

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
			view.ChangeVisibleGlobalPanel(player.ChangeStateIsPlay());
		}
		if (key == KeyCode.V)
		{
			view.UpdatePlayerTypeMove(player.ChangeStatePlayerMove() + "");
		}
	}
}
