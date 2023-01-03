using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPointsBA : MonoBehaviour
{

    void Start()
    {
        Destroy(gameObject, 1f);
        transform.localPosition += new Vector3(0.2f, 0.3f, 0);
    }

    
}
