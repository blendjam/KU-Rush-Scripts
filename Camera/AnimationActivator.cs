using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AnimationActivator : MonoBehaviour
{
	[SerializeField] Animator _animator;
	[SerializeField] string animationName;
	[SerializeField] bool destoryOnEnter;

	private int animationHash;

	private void Start()
	{
		animationHash = Animator.StringToHash(animationName);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_animator.Play(animationHash);
			if (destoryOnEnter)
				Object.Destroy(gameObject);
		}
	}
}
