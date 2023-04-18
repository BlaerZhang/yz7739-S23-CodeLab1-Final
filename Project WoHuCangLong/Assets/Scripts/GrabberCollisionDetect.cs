using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberCollisionDetect : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D colRB2D = null;
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Bamboo"))
        {
            colRB2D = col.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    public Rigidbody2D GetGrabberCollisionRigidbody2D()
    {
        return colRB2D;
    }
}
