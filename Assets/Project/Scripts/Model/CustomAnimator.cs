using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAnimator : MonoBehaviour
{
	[SerializeField] private Animator animator;
	private static readonly int Walk = Animator.StringToHash("walk");
	private static readonly int Classic = Animator.StringToHash("classic");

	public void UpdateAnimation(float walk, float classic)
	{
		animator.SetFloat(Walk, walk);
		animator.SetFloat(Classic, classic);
	}	
}
