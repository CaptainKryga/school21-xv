using System;
using Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.Model
{
	public class Worker : MonoBehaviour
	{
		[SerializeField] private Transform worker;
		[SerializeField] private CustomAnimator animator;

		[SerializeField] private NavMeshAgent agent;
		[SerializeField] private Transform point1, point2;

		[SerializeField] private GameObject boxSmall, boxMiddle;
		
		private Vector3 workerBodyStartPosition;
		private Quaternion workerBodyStartRotation;
		private float startSpeed, startAcceleration;
		
		public Transform GetTransform { get => worker; }
		public Vector3 WorkerBodyStartPosition { get => workerBodyStartPosition; }
		public Quaternion WorkerBodyStartRotation { get => workerBodyStartRotation; }

		private void Start()
		{
			workerBodyStartPosition = transform.position;
			workerBodyStartRotation = transform.rotation;
			
			agent.autoRepath = true;
			agent.autoTraverseOffMeshLink = true;

			startSpeed = agent.speed;
			startAcceleration = agent.acceleration;
			
			UpdateAnimation(0, 1);
		}

		public bool SetNextPosition(Vector3 position, float distance = 1)
		{
			agent.destination = position;

			if (Vector3.Distance(agent.transform.position, position) < distance)
			{
				return true;
			}
			return false;
		}

		public void UpdateAnimation(float walk, float classic)
		{
			animator.UpdateAnimation(walk, classic);
		}

		public void UpdateSpeed(float speed)
		{
			agent.speed = startSpeed * speed;
			agent.acceleration = startAcceleration * speed * 2;
		}

		public Transform GetTransformWorker()
		{
			return worker;
		}

		public void UpdateVisibleItem(GameTypes.Item item, bool isFlag)
		{
			if (item == GameTypes.Item.BoxSmall)
				boxSmall.SetActive(isFlag);
			else if (item == GameTypes.Item.BoxMiddle)
				boxMiddle.SetActive(isFlag);
		}
	}
}