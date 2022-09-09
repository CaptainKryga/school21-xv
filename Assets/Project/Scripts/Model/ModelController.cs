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
	[SerializeField] private GameTypes.Game state;

	private SettingsScene nowOpenScene;
	
	
	public GameTypes.Game GetStateGame { get => state; }
	public SettingsScene NowOpenScene { get => nowOpenScene; set => nowOpenScene = value; }
	
	
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

	private void Start()
	{
		UpdatePlayerState(GameTypes.PlayerMove.Spectator);
	}

	private void ReceiveKeyboardActions(KeyCode key)
	{
		if (key == KeyCode.Tab)
		{
			view.ChangeVisibleGlobalPanel();
		}

		if (key == KeyCode.Alpha1)
		{
			player.UpdateState(GameTypes.PlayerMove.Spectator);
			view.UpdatePlayerTypeMove(GameTypes.PlayerMove.Spectator.ToString());
		}
		if (key == KeyCode.Alpha2)
		{
			player.UpdateState(GameTypes.PlayerMove.HumanFirst);
			view.UpdatePlayerTypeMove(GameTypes.PlayerMove.HumanFirst.ToString());
		}
		if (key == KeyCode.Alpha3)
		{
			player.UpdateState(GameTypes.PlayerMove.HumanThird);
			view.UpdatePlayerTypeMove(GameTypes.PlayerMove.HumanThird.ToString());
		}
		if (key == KeyCode.Alpha4)
		{
			player.UpdateState(GameTypes.PlayerMove.WorkerFirst);
			view.UpdatePlayerTypeMove(GameTypes.PlayerMove.WorkerFirst.ToString());
		}
		if (key == KeyCode.Alpha5)
		{
			player.UpdateState(GameTypes.PlayerMove.WorkerThird);
			view.UpdatePlayerTypeMove(GameTypes.PlayerMove.WorkerThird.ToString());
		}
	}

	public void UpdateGameState(GameTypes.Game state)
	{
		this.state = state;
	}

	public void UpdatePlayerState(GameTypes.PlayerMove state)
	{
		player.UpdateState(state);
		view.UpdatePlayerTypeMove(state.ToString());
	}
}
