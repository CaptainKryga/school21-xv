using System;
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
		[SerializeField] private bool isPoint1;
		
		private Vector3 workerBodyStartPosition;
		private Quaternion workerBodyStartRotation;
		
		public Transform GetTransform { get => worker; }
		public Vector3 WorkerBodyStartPosition { get => workerBodyStartPosition; }
		public Quaternion WorkerBodyStartRotation { get => workerBodyStartRotation; }

		private void Start()
		{
			workerBodyStartPosition = transform.position;
			workerBodyStartRotation = transform.rotation;
			
			agent.destination = point1.transform.position;
			agent.autoRepath = true;
			agent.autoTraverseOffMeshLink = true;
		}

		private void Update()
		{
			animator.SetAnimatorWalkSpeed(1);

			if (!isPoint1 && Vector3.Distance(agent.transform.position, point1.transform.position) < 1)
			{
				agent.destination = point2.transform.position;
				isPoint1 = true; ;
			}
			else if (isPoint1 && Vector3.Distance(agent.transform.position, point2.transform.position) < 1)
			{
				agent.destination = point1.transform.position;
				isPoint1 = false;
			}
		}

		public Transform GetTransformWorker()
		{
			return worker;
		}
	}
}