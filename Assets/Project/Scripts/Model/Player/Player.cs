using System;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.Model
{
	public class Player : MonoBehaviour
	{
		//ссылка на элементы игрока в сцене
		[SerializeField] private Transform player;
		[SerializeField] private Camera cam;
		//можем ли мы двигаться?
		private bool isPlay;
		private GameTypes.PlayerCam playerCam;

		private Vector2 move;
		private float mouseX, mouseY;
		
		[Header("Settings")]
		[SerializeField] private float speed;
		[SerializeField] private float sensitive;
		[SerializeField] float rotateMinY = -85.0f;
		[SerializeField] float rotateMaxY = 85.0f;

		private void Start()
		{
			playerCam = GameTypes.PlayerCam.Spectator;
		}

		private void Update()
		{
			if (!isPlay)
				return;
			
			if (playerCam == GameTypes.PlayerCam.Spectator)
			{
				player.position += (player.forward * move.y + player.right * move.x) * speed * Time.deltaTime;
				UpdateCam();
				Debug.Log("player pos: " + player.position);
			} 
			else if (playerCam == GameTypes.PlayerCam.HumanFirst)
			{
				//enable physics
			}
			else if (playerCam == GameTypes.PlayerCam.HumanThird)
			{
				//enable physics
			}
			else if (playerCam == GameTypes.PlayerCam.WorkerFirst)
			{
				//disable physics
				//set parent worker
			}
			else if (playerCam == GameTypes.PlayerCam.WorkerThird)
			{
				//disable physics
				//set parent worker
			}
		}
		
		public void UpdateCam()
		{

			//ставим ограничение по осям чтобы не улетать как колобок
			mouseY = Mathf.Clamp(mouseY, rotateMinY, rotateMaxY);
			cam.transform.eulerAngles = new Vector3(-mouseY, player.transform.localEulerAngles.y, 0);
			player.localEulerAngles = new Vector3(0, mouseX, 0);
		}

		public bool ChangePlayState()
		{
			isPlay = !isPlay;
			return isPlay;
		}

		public void UpdateGlobalState(GameTypes.Game state)
		{
			
		}

		public void ReceivePlayerMoveActions(Vector2 vec)
		{
			move = vec;
		}
		public void ReceivePlayerAxisActions(Vector2 axis)
		{
			//добавляем угол для обзора по осям
			mouseX += axis.x * sensitive * Time.deltaTime;
			mouseY += axis.y * sensitive * Time.deltaTime;
		}
	}
	
}