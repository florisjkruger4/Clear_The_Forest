using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootChest : Interactable
{
    public GameObject contextClue;
    public GameObject Player;
    public GameObject TresureLights;

    public bool isOpen;
    public Animator anim;

    public Spawner spawner;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpen)
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

    IEnumerator delayLights()
    {
        yield return new WaitForSeconds(0.25f);
        TresureLights.SetActive(true);
        FindObjectOfType<AudioManager>().Play("Open Chest");
    }

    IEnumerator delaySpawn()
    {
        yield return new WaitForSeconds(0.5f);
        spawner.Spawn();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            contextClue.GetComponent<ContextClue>().anim.SetTrigger("ContextExit");
            StartCoroutine(delayClose());
        }
    }

    public void OpenChest()
    {
        if (!isOpen)
        {
            isOpen = true;
            StartCoroutine(delayLights());
            contextClue.GetComponent<ContextClue>().Disable();
            anim.SetBool("OpenChest", true);

            StartCoroutine(delaySpawn());
        }
    }
}
