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
		
		public Transform GetTransform { get => worker; }
		public Vector3 WorkerBodyStartPosition { get => workerBodyStartPosition; }
		public Quaternion WorkerBodyStartRotation { get => workerBodyStartRotation; }

		private void Start()
		{
			workerBodyStartPosition = transform.position;
			workerBodyStartRotation = transform.rotation;
			
			agent.autoRepath = true;
			agent.autoTraverseOffMeshLink = true;
			
			UpdateAnimation(0, 1);
		}

		public bool SetNextPosition(Vector3 position)
		{
			agent.destination = position;

			if (Vector3.Distance(agent.transform.position, position) < 1)
			{
				return true;
			}
			return false;
		}

		public void UpdateAnimation(float walk, float classic)
		{
			animator.UpdateAnimation(walk, classic);
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