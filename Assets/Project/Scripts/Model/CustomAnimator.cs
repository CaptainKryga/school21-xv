using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAnimator : MonoBehaviour
{
	[SerializeField] private Animator animator;
	private static readonly int Walk = Animator.StringToHash("walk");

	public void SetAnimatorWalkSpeed(float speed)
	{
		animator.SetFloat(Walk, speed);
	}
}
