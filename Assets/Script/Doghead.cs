using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doghead : MonoBehaviour
{
    public GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2D;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0;
    }
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
      if( other.gameObject.CompareTag("Bee")){
         gameManager.buttonGroup.gameObject.SetActive(true);
         gameManager.tryAgain.gameObject.SetActive(true);
         gameManager.announcer.text = "Try again";
      } 
      
      if(other.gameObject.CompareTag("Spike") ){

         gameManager.buttonGroup.gameObject.SetActive(true);
         gameManager.tryAgain.gameObject.SetActive(true);
         gameManager.announcer.text = "Try again";
         spriteRenderer.enabled = false;
      }
    }
    private void OnTriggerEnter2D(Collider2D other) {
      if(other.gameObject.CompareTag("ToxicWater") || other.gameObject.CompareTag("Spike") ){

         gameManager.buttonGroup.gameObject.SetActive(true);
         gameManager.tryAgain.gameObject.SetActive(true);
         gameManager.announcer.text = "Try again";
         spriteRenderer.enabled = false;
      }
    }
}
