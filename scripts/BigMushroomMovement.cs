using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMushroomMovement : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            anim.SetTrigger("MoveDown");
        }

    }




    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            anim.SetTrigger("MoveUp");
        }

    }
}
