using System;
using Project.Scripts.Utils;
using UnityEngine;

namespace Project.Scripts.Model
{
	public class Player : MonoBehaviour
	{
		//ссылка на элементы игрока в сцене
		[SerializeField] private Transform player;
		[SerializeField] private Transform cam;
		private Transform parentStart;
		[SerializeField] private CustomAnimator animator;
		private Rigidbody rigidbody;

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
		[SerializeField] private float speed;
		[SerializeField] public float limitSpeed;

		//скорость поворота
		[SerializeField] private float sensitive;


		private void Start()
		{
			parentStart = player.parent;
			meshRenderers = player.GetComponentsInChildren<SkinnedMeshRenderer>();
			rigidbody = player.GetComponent<Rigidbody>();

			state = GameTypes.PlayerMove.Spectator;
			UpdateState(state);
			ChangeVisiblePlayer(false);
		}

		private void Update()
		{
			if (!isPlay)
				return;

			if (state == GameTypes.PlayerMove.Spectator)
			{
				rigidbody.velocity = (cam.forward * move.y + cam.right * move.x) * speed / 2;
				DefaultUpdateCam();
			}
			else if (state == GameTypes.PlayerMove.HumanFirst)
			{
				if (rigidbody.velocity.magnitude < limitSpeed)
					rigidbody.velocity += (player.forward * move.y + cam.right * move.x) * speed * Time.deltaTime;
				rigidbody.velocity += Physics.gravity * Time.deltaTime;
				DefaultUpdateCam();
			}
			else if (state == GameTypes.PlayerMove.HumanThird)
			{
				if (rigidbody.velocity.magnitude < limitSpeed)
					rigidbody.velocity += (player.forward * move.y + cam.right * move.x) * speed * Time.deltaTime;
				rigidbody.velocity += Physics.gravity * Time.deltaTime;
				DefaultUpdateCam();
			}
			else if (state == GameTypes.PlayerMove.WorkerFirst)
			{
				
			}
			else if (state == GameTypes.PlayerMove.WorkerThird)
			{
				
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
				UpdatePlayerSettings(false, false, true, point1, new Vector2(-85, 85));
			else if (state == GameTypes.PlayerMove.HumanFirst)
				UpdatePlayerSettings(false, false, true, point1, new Vector2(-85, 85));
			else if (state == GameTypes.PlayerMove.HumanThird)
				UpdatePlayerSettings(true, false, true, point3, new Vector2(-20, 0));
			else if (state == GameTypes.PlayerMove.WorkerFirst)
				UpdatePlayerSettings(false, true, false, point1, Vector2.zero);
			else if (state == GameTypes.PlayerMove.WorkerThird)
				UpdatePlayerSettings(false, true, false, point3, Vector2.zero);
		}

		private void UpdatePlayerSettings(bool isVisible, bool isWorker, bool isPhysics,
			Transform camPoint, Vector2 restrictionRot)
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

			rigidbody.isKinematic = !isPhysics;
			rigidbody.useGravity = isPhysics;
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
			if (worker == null)
			{
				Worker temp = FindObjectOfType<Worker>();
				if (temp != null)
					worker = temp.GetTransformWorker();
			}
		}

		public bool IsPlay()
		{
			return isPlay;
		}
	}
}