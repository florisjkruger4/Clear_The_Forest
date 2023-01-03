using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextClue : MonoBehaviour
{
    public GameObject contextClue;
    public Animator anim;

    public GameObject player;

    public bool isFacingRight;

    private SpriteRenderer flipSpriteX;

    private void Start()
    {
        flipSpriteX = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        isFacingRight = player.GetComponent<Playermovement>().isFacingRight;

        if (isFacingRight == false)
        {
            flipSpriteX.flipX = true;
        }
        else
        {
            flipSpriteX.flipX = false;
        }
    }

    public void Enable()
    {
        contextClue.SetActive(true);
    }


    public void Disable()
    {
        contextClue.SetActive(false);
    }
}
