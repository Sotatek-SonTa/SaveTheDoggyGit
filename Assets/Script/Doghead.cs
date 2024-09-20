using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doghead : MonoBehaviour
{
    public GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2D;
    public Action OnDie;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0;
    }
    void Update()
    {
      if(Input.GetMouseButtonUp(0)){
         rigidbody2D.gravityScale = 1;
      }
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
      if( other.gameObject.CompareTag("Bee")){
         gameManager.buttonGroup.gameObject.SetActive(true);
         gameManager.lostGroup.gameObject.SetActive(true);
         gameManager.isDead = true;
      } 
      
      if(other.gameObject.CompareTag("Spike") ){

         gameManager.buttonGroup.gameObject.SetActive(true);
         gameManager.lostGroup.gameObject.SetActive(true);
         spriteRenderer.enabled = false;
         gameManager.isDead = true;
      }
    }
    private void OnTriggerEnter2D(Collider2D other) {
      if(other.gameObject.CompareTag("ToxicWater")){

         gameManager.buttonGroup.gameObject.SetActive(true);
         gameManager.lostGroup.gameObject.SetActive(true);
         spriteRenderer.enabled = false;
         gameManager.isDead = true;
      }
    }
}
