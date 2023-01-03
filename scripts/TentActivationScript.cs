using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentActivationScript : Interactable
{
    public GameObject contextClue;
    public GameObject Player;

    public GameObject TentLight;

    public bool isActivated;
    public Animator anim;

    public Transform respawnPoint;

    public ParticleSystem TentParticles;

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

    IEnumerator delayParticles()
    {
        yield return new WaitForSeconds(0.5f);
        TentParticles.Play();
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
            respawnPoint.transform.position = this.transform.position;
            isActivated = true;
            TentLight.SetActive(true);
            contextClue.GetComponent<ContextClue>().Disable();
            anim.SetBool("Activated", true);
            StartCoroutine(delayParticles());
        }
        anim.SetBool("TentIdle", true);
        
    }
}
