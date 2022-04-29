using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSound : MonoBehaviour
{
    private AudioSource audioSource;
    //public AudioClip hitSound;
    public AudioClip dieSound;

    public void PlayBossSound(string action)
    {
        switch (action)
        {
            case "DIE":
                audioSource.clip = dieSound;
                break;
        }
        audioSource.Play();
    }
}
