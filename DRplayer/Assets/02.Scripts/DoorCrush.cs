using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCrush : MonoBehaviour
{
    public int doorhp = 2;
    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player" && doorhp < 0)
    //    {
    //        --doorhp;
    //    }

    //    else if (doorhp <= 0)
    //    {

    //        animator.SetTrigger("Crash");

    //    }
    //}

    public void HitDoor(int damage)
    {
        doorhp -= damage;
        if (doorhp <= 0)
        {
            StartCoroutine("DoorAnimation");
        }
        
    }

    public IEnumerator DoorAnimation()
    {
        animator.SetTrigger("Crash");
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
