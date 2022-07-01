using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckPosition();
    }

    private void CheckPosition()
    {
        if (Math.Abs(transform.position.x) > Constants.CornerX)
        {
            transform.position = (new Vector3(-Constants.CornerX * Math.Sign(transform.position.x), transform.position.y,
                transform.position.z));
        }

        if (Math.Abs(transform.position.y) > Constants.CornerY)
        {
            transform.position = (new Vector3(transform.position.x, -Constants.CornerY * Math.Sign(transform.position.y),
                transform.position.z));
        }
    }
}
