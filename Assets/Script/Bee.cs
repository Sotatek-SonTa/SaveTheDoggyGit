using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public Transform doghead;
    public Rigidbody2D beeRigigdoby;
    public float speed = 5f;
    public float bouceForce = 10f;

    public GameManager gameManager;


    void Start()
    {
        doghead = GameObject.FindWithTag("Dog").transform;
        gameManager =FindObjectOfType<GameManager>();
        beeRigigdoby = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
     Vector2 directiontoDog = (doghead.transform.position - gameObject.transform.position).normalized;
     float angle = Mathf.Atan2(directiontoDog.y, directiontoDog.x) * Mathf.Rad2Deg;
     transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
     beeRigigdoby.AddForce(directiontoDog*speed,ForceMode2D.Force);
    }       
     void OnCollisionEnter2D(Collision2D other) {
      if(other.gameObject.CompareTag("Line")){
       Vector2 collisionPoint = other.contacts[0].point;
      Vector2 bounceDirection = (beeRigigdoby.position - collisionPoint).normalized;
      beeRigigdoby.AddForce(bounceDirection*bouceForce,ForceMode2D.Impulse);
      Vector2 directionToDog = (doghead.position - transform.position).normalized;
      beeRigigdoby.velocity = directionToDog* speed;  
      }else
      if(other.gameObject.CompareTag("Dog")){
         gameManager.buttonGroup.gameObject.SetActive(true);
         gameManager.tryAgain.gameObject.SetActive(true);
         gameManager.announcer.text = "Try again";
      }
     }
}
