using System;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.Model
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private CustomAnimator animator;
		

		//ссылка на элементы игрока в сцене
		[SerializeField] private Transform player;
		[SerializeField] private Transform cam;
		private Transform parentStart;
		
		//ссылка на рабочего
		private Transform worker;

		//точки положения камеры при смене типа движения
		[SerializeField] private Transform point1, point3;

		//для отключения и включения видимости персонажа
		private SkinnedMeshRenderer[] meshRenderers;


		//можем ли мы двигаться?
		private bool isPlay;

		//тип движения
		private GameTypes.PlayerMove state;

		//параметры движения и поворотов
		private Vector2 move;
		private float mouseX, mouseY;

		//ограничения по поворотам
		private float rotateMinY = -85.0f;
		private float rotateMaxY = 85.0f;

		[Header("Settings")]
		//скорость движения
		[SerializeField]
		private float speed;

		//скорость поворота
		[SerializeField] private float sensitive;


		private void Start()
		{
			parentStart = player.parent;
			meshRenderers = player.GetComponentsInChildren<SkinnedMeshRenderer>();

			state = GameTypes.PlayerMove.Spectator;
			ChangeVisiblePlayer(false);
		}

		private void Update()
		{
			if (!isPlay)
				return;

			if (state == GameTypes.PlayerMove.Spectator)
			{
				player.position += (cam.forward * move.y + cam.right * move.x) * speed * Time.deltaTime;
				DefaultUpdateCam();
			}
			else if (state == GameTypes.PlayerMove.HumanFirst)
			{
				player.position += (player.forward * move.y + player.right * move.x) * speed * Time.deltaTime;
				DefaultUpdateCam();
				//enable physics?
			}
			else if (state == GameTypes.PlayerMove.HumanThird)
			{
				player.position += (player.forward * move.y + player.right * move.x) * speed * Time.deltaTime;
				DefaultUpdateCam();
				//enable physics
			}
			else if (state == GameTypes.PlayerMove.WorkerFirst)
			{
				//disable physics
				//set parent worker
			}
			else if (state == GameTypes.PlayerMove.WorkerThird)
			{
				//disable physics
				//set parent worker
			}

		}

		public void DefaultUpdateCam()
		{
			//ставим ограничение по осям чтобы не улетать как колобок
			mouseY = Mathf.Clamp(mouseY, rotateMinY, rotateMaxY);
			cam.transform.eulerAngles = new Vector3(-mouseY, player.transform.localEulerAngles.y, 0);
			player.localEulerAngles = new Vector3(0, mouseX, 0);
		}

		public bool ChangeStateIsPlay()
		{
			isPlay = !isPlay;
			return isPlay;
		}

		public GameTypes.PlayerMove ChangeStatePlayerMove()
		{
			GameTypes.PlayerMove state = GameTypes.PlayerMove.Spectator;
			if (this.state == GameTypes.PlayerMove.Spectator)
				state = GameTypes.PlayerMove.HumanFirst;
			else if (this.state == GameTypes.PlayerMove.HumanFirst)
				state = GameTypes.PlayerMove.HumanThird;
			else if (this.state == GameTypes.PlayerMove.HumanThird)
				state = GameTypes.PlayerMove.WorkerFirst;
			else if (this.state == GameTypes.PlayerMove.WorkerFirst)
				state = GameTypes.PlayerMove.WorkerThird;
			else if (this.state == GameTypes.PlayerMove.WorkerThird)
				state = GameTypes.PlayerMove.Spectator;

			UpdateState(state);
			return state;
		}

		private void UpdateState(GameTypes.PlayerMove state)
		{
			this.state = state;
			if (state == GameTypes.PlayerMove.Spectator)
				UpdatePlayerSettings(false, false, point1, new Vector2(-85, 85));
			else if (state == GameTypes.PlayerMove.HumanFirst)
				UpdatePlayerSettings(false, false, point1, new Vector2(-85, 85));
			else if (state == GameTypes.PlayerMove.HumanThird)
				UpdatePlayerSettings(true, false, point3, new Vector2(-20, 0));
			else if (state == GameTypes.PlayerMove.WorkerFirst)
				UpdatePlayerSettings(false, true, point1, Vector2.zero);
			else if (state == GameTypes.PlayerMove.WorkerThird)
				UpdatePlayerSettings(false, true, point3, Vector2.zero);
		}

		private void UpdatePlayerSettings(bool isVisible, bool isWorker, Transform camPoint, Vector2 restrictionRot)
		{
			if (worker == null)
				GetWorker();
			
			if (isWorker && worker)
			{
				player.SetParent(worker.transform);
				player.localPosition = Vector3.zero;
				player.localRotation = Quaternion.identity;
			}
			else
			{
				player.SetParent(parentStart);
			}
			
			ChangeVisiblePlayer(isVisible);
			
			cam.position = camPoint.position;
			cam.rotation = camPoint.rotation;

			rotateMinY = restrictionRot.x;
			rotateMaxY = restrictionRot.y;
		}

		public void ReceivePlayerMoveActions(Vector2 vec)
		{
			move = vec;
			animator.SetAnimatorWalkSpeed(Mathf.InverseLerp(0, 1, Mathf.Abs(vec.x) + Mathf.Abs(vec.y)));
		}

		public void ReceivePlayerAxisActions(Vector2 axis)
		{
			//добавляем угол для обзора по осям
			mouseX += axis.x * sensitive * Time.deltaTime;
			mouseY += axis.y * sensitive * Time.deltaTime;
		}

		private void ChangeVisiblePlayer(bool isVisible)
		{
			foreach (var mesh in meshRenderers)
				mesh.enabled = isVisible;
		}

		private void GetWorker()
		{
			Debug.Log("GetWorker");
			if (worker == null)
			{
				Debug.Log("worker == null");
				Worker temp = FindObjectOfType<Worker>();
				Debug.Log(temp);
				if (temp != null)
					worker = temp.GetTransformWorker();
				Debug.Log(worker);
			}
		}
	}
}