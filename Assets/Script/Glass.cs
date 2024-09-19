using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
     public Rigidbody2D rigidbody2D;
    void Start()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
     if(Input.GetMouseButtonUp(0)){
        rigidbody2D.gravityScale = 1;
     }   
    }
}
