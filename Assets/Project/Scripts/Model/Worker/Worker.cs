using System;
using UnityEngine;

namespace Project.Scripts.Model
{
	public class Worker : MonoBehaviour
	{
		[SerializeField] private Transform worker;
		[SerializeField] private CustomAnimator animator;
		
		private Vector3 workerBodyStartPosition;
		private Quaternion workerBodyStartRotation;
		
		public Transform GetTransform { get => worker; }
		public Vector3 WorkerBodyStartPosition { get => workerBodyStartPosition; }
		public Quaternion WorkerBodyStartRotation { get => workerBodyStartRotation; }

		private void Start()
		{
			workerBodyStartPosition = transform.position;
			workerBodyStartRotation = transform.rotation;
		}

		private void Update()
		{
			animator.SetAnimatorWalkSpeed(0);
		}

		public Transform GetTransformWorker()
		{
			return worker;
		}
	}
}