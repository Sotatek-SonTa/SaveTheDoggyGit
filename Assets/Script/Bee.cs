using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public Transform doghead;
    public GameObject doghead2;
    public Rigidbody2D beeRigigdoby;
    public float speed = 10f;
    public float bouceForce = 10f;
    private bool isFlyingToDog = false;
     public float randomSpeed = 2f;
     public float randomFlyDuration =3f;
     public float randomFlyTimer;
     public int number;
    public GameManager gameManager;
  


    void Start()
    {
        doghead = GameObject.FindWithTag("Dog").transform;
        gameManager =FindObjectOfType<GameManager>();
        beeRigigdoby = GetComponent<Rigidbody2D>();
        randomFlyTimer = randomFlyDuration;
        number = Random.Range(0,2);
        doghead2 = GameObject.Find("Doghead2");
    }
    void Update()
    {
     if(!isFlyingToDog){
        randomFlyTimer -= Time.deltaTime;
        if(randomFlyTimer <=0)
        {
            isFlyingToDog =true;
        }
        else
        {
          FlyRandomly(); 
        }
     }
     else
     {
       FlyTowardDog();
     }
    }
    void FlyRandomly(){
        Vector2 randormDirection = Random.insideUnitCircle.normalized;
        beeRigigdoby.AddForce(randormDirection*randomSpeed);
    }
    void FlyTowardDog(){
     if(doghead2 == null){
     Vector2 directiontoDog = (doghead.transform.position - gameObject.transform.position).normalized;
     float angle = Mathf.Atan2(directiontoDog.y, directiontoDog.x) * Mathf.Rad2Deg;
     transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
     beeRigigdoby.AddForce(directiontoDog*speed);
     }
     else
     {
      if(number ==0)
      {
       Vector2 directiontoDog = (doghead.transform.position - gameObject.transform.position).normalized;
       float angle = Mathf.Atan2(directiontoDog.y, directiontoDog.x) * Mathf.Rad2Deg;
       transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
       beeRigigdoby.AddForce(directiontoDog*speed);
      }else
      {
       Vector2 directiontoDog = (doghead2.transform.position - gameObject.transform.position).normalized;
       float angle = Mathf.Atan2(directiontoDog.y, directiontoDog.x) * Mathf.Rad2Deg;
       transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
       beeRigigdoby.AddForce(directiontoDog*speed);
      }
     }
    }       
     void OnCollisionEnter2D(Collision2D other) {
      if(other.gameObject.CompareTag("Line")){
       Vector2 collisionPoint = other.contacts[0].point;
      Vector2 bounceDirection = (beeRigigdoby.position - collisionPoint).normalized;
      beeRigigdoby.AddForce(bounceDirection*bouceForce,ForceMode2D.Impulse);
      Vector2 directionToDog = (doghead.position - transform.position).normalized;
      beeRigigdoby.velocity = directionToDog* speed;  
      }
     }
}
