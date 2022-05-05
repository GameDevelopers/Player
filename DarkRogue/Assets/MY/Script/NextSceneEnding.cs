using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneEnding : MonoBehaviour
{
	public Animator anim;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			anim.SetTrigger("FadeIn");
		}

	}


}
