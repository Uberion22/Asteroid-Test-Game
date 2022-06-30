using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    [SerializeField] private float xBound;

    [SerializeField] private float yBound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(transform.position.x) > xBound)
        {
            transform.position = (new Vector3(- xBound * Math.Sign(transform.position.x), transform.position.y,
                transform.position.z));
        }

        if (Math.Abs(transform.position.y) > yBound)
        {
            transform.position = (new Vector3(transform.position.x, - yBound * Math.Sign(transform.position.y),
                transform.position.z));
        }
    }
}
