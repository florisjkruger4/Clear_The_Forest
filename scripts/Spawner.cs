using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnObject;
    public GameObject spawnParent;

    public void Spawn()
    {
        Instantiate(spawnObject, spawnParent.transform.position, transform.rotation);
    }
}
