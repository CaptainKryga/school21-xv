using System;
using UnityEngine;

namespace Project.Scripts.Model
{
	public class Worker : MonoBehaviour
	{
		[SerializeField] private Transform worker;
		[SerializeField] private CustomAnimator animator;
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