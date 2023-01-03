using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public int heal = 50;

    public Collider2D col;

    public ParticleSystem healthPickupPS;

    IEnumerator delayPickup()
    {
        yield return new WaitForSeconds(0.8f);
        col.enabled = true;
    }

    private void Start()
    {
        col.enabled = false;
        StartCoroutine(delayPickup());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            healthPickupPS.transform.SetParent(null);
            healthPickupPS.Play();

            Destroy(healthPickupPS.gameObject, 3f);

            Debug.Log("HelloHealth");
            Destroy(gameObject);

            Manager.instance.IncreaseHealth(heal);
            
        }

    }
}
