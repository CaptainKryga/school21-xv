using System;
using Project.Scripts.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.Scripts.Model
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private ModelController model;
		
		//ссылка на элементы игрока в сцене
		[SerializeField] private Transform body;
		[SerializeField] private Transform cam;
		private Transform parentStart;
		[SerializeField] private CustomAnimator animator;
		private Rigidbody rigidbody;

		private Vector3 playerBodyStartPosition, playerCamStartPosition;
		private Quaternion playerBodyStartRotation, playerCamStartRotation;

		//ссылка на рабочего
		private Transform worker;

		//точки положения камеры при смене типа движения
		[SerializeField] private Transform point1, point3;

		//для отключения и включения видимости персонажа
		private SkinnedMeshRenderer[] meshRenderers;

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
		
		public Transform GetBodyTransform { get => body; }
		public Transform GetCamTransform { get => body; }
		public GameTypes.PlayerMove GetState { get => state; }
		public Vector3 PlayerBodyStartPosition { get => playerBodyStartPosition; }
		public Vector3 PlayerCamStartPosition { get => playerCamStartPosition; }
		public Quaternion PlayerBodyStartRotation { get => playerBodyStartRotation; }
		public Quaternion PlayerCamStartRotation { get => playerCamStartRotation; }

		private void Awake()
		{
			parentStart = body.parent;
			meshRenderers = body.GetComponentsInChildren<SkinnedMeshRenderer>();
			rigidbody = body.GetComponent<Rigidbody>();
		}

		private void Start()
		{
			playerBodyStartPosition = body.position;
			playerBodyStartRotation = body.rotation;
			playerCamStartPosition = cam.position;
			playerCamStartRotation = cam.rotation;
		}

		private void Update()
		{
			if (model.GetStateGame != GameTypes.Game.Play)
			{
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
				return;
			}

			if (state == GameTypes.PlayerMove.Spectator)
			{
				if (rigidbody.velocity.magnitude < 0.5f)
					rigidbody.isKinematic = true;
				else
					rigidbody.isKinematic = false;
				
				rigidbody.velocity = (cam.forward * move.y + cam.right * move.x) * speed / 2;
				
				DefaultUpdateCam();
			}
			else if (state == GameTypes.PlayerMove.HumanFirst)
			{
				if (rigidbody.velocity.magnitude < limitSpeed)
					rigidbody.velocity += (body.forward * move.y + cam.right * move.x) * speed * Time.deltaTime;
				rigidbody.velocity += Physics.gravity * Time.deltaTime;
				DefaultUpdateCam();
			}
			else if (state == GameTypes.PlayerMove.HumanThird)
			{
				if (rigidbody.velocity.magnitude < limitSpeed)
					rigidbody.velocity += (body.forward * move.y + cam.right * move.x) * speed * Time.deltaTime;
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
			cam.transform.eulerAngles = new Vector3(-mouseY, body.transform.localEulerAngles.y, 0);
			body.localEulerAngles = new Vector3(0, mouseX, 0);
		}

		public void UpdateState(GameTypes.PlayerMove state)
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
				body.SetParent(worker.transform);
				body.localPosition = Vector3.zero;
				body.localRotation = Quaternion.identity;
			}
			else
			{
				body.SetParent(parentStart);
			}

			ChangeVisiblePlayer(isVisible);

			cam.position = camPoint.position;
			cam.rotation = camPoint.rotation;

			rotateMinY = restrictionRot.x;
			rotateMaxY = restrictionRot.y;

			rigidbody.isKinematic = !isPhysics;
			rigidbody.useGravity = isPhysics;

			rigidbody.interpolation = isPhysics ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None;
		}

		public void ReceivePlayerMoveActions(Vector2 vec)
		{
			move = vec;
			animator.UpdateAnimation(Mathf.InverseLerp(0, 1, Mathf.Abs(vec.x) + Mathf.Abs(vec.y)), 1);
		}

		public void ReceivePlayerAxisActions(Vector2 axis)
		{
			if (model.GetStateGame != GameTypes.Game.Play)
				return;
			
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
	}
}