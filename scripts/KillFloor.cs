using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KillFloor : MonoBehaviour
{
    public GameObject player;
    public GameObject RespawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.CompareTag("Player") && RespawnPoint.activeSelf)
        {
            player.GetComponent<Playermovement>().currentHealth = 0;
            Manager.instance.Respawn();

        }

        
    }
}
