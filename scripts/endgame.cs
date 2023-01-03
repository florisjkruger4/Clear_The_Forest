using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endgame : Interactable
{
    public GameObject contextClue;
    public GameObject Player;
    public bool isActivated;
    public Animator anim;

    public GameObject EndGameTent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            isInRange = true;
            contextClue.GetComponent<ContextClue>().Enable();
            contextClue.GetComponent<ContextClue>().anim.SetTrigger("ContextOpen");
        }
    }

    IEnumerator delayClose()
    {
        yield return new WaitForSeconds(0.5f);
        contextClue.GetComponent<ContextClue>().Disable();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            contextClue.GetComponent<ContextClue>().anim.SetTrigger("ContextExit");
            StartCoroutine(delayClose());
        }
    }

    public void ActivateTent()
    {
        if (!isActivated)
        {
            FindObjectOfType<AudioManager>().Play("Open Tent");
            isActivated = true;
            contextClue.GetComponent<ContextClue>().Disable();
            anim.SetBool("Activated", true);
        }

        if (EndGameTent)
        {
            SceneManager.LoadScene("Main Menu");
        }

        anim.SetBool("TentIdle", true);

    }
}
