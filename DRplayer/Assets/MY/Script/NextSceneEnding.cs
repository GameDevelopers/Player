using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneEnding : MonoBehaviour
{
	public Animator anim;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			StartCoroutine(FadeIn());
		}

	}

	IEnumerator FadeIn()
	{
		anim.SetTrigger("FadeIn");
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene("Menu");
	}
}
