using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPickup : MonoBehaviour
{
    public int worth = 100;

    public ParticleSystem GemPickupCircle;

    public GameObject GemDisplay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            GemPickupCircle.transform.SetParent(null);
            GemPickupCircle.Play();

            Destroy(GemPickupCircle.gameObject, 3f);

            Destroy(gameObject);

            Manager.instance.IncreaseCurrency(worth);

            GemDisplay.GetComponent<Animator>().SetTrigger("GemUIAnim");
        }
    }


}
