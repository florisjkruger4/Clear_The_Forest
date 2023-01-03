using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float ParalaxEffect;

    private void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
       
        float dist = (cam.transform.position.x * ParalaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        
    }
}
