using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveTheDoggDoghead;
namespace SaveTheDoggyBee
{
    public class Bee : MonoBehaviour
    {
        [SerializeField] private List<Doghead> mTargets;
        public Rigidbody2D beeRigigdoby;
        public float speed = 10f;
        public float bouceForce = 10f;
        private bool isFlyingToDog = false;
        public float randomSpeed = 2f;
        public float randomFlyDuration = 3f;
        public float randomFlyTimer;
        public int number;
        public Vector2 randomDirection;



        void Start()
        {
            beeRigigdoby = GetComponent<Rigidbody2D>();
            randomFlyTimer = randomFlyDuration;
            number = Random.Range(0, mTargets.Count);
            randomDirection = Random.insideUnitCircle.normalized;
        }
        void Update()
        {
            if (!isFlyingToDog)
            {
                randomFlyTimer -= Time.deltaTime;
                if (randomFlyTimer <= 0)
                {
                    isFlyingToDog = true;
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
        void FlyRandomly()
        {

            float angle = Mathf.Atan2(randomDirection.y, randomDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            beeRigigdoby.AddForce(randomDirection * randomSpeed * Time.deltaTime);
        }
        void FlyTowardDog()
        {
            Vector2 directiontoDog = (mTargets[number].transform.position - gameObject.transform.position).normalized;
            float angle = Mathf.Atan2(directiontoDog.y, directiontoDog.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            beeRigigdoby.AddForce(directiontoDog * speed * Time.deltaTime);
        }
        void OnCollisionEnter2D(Collision2D other)
        {
            Vector2 bounceDirection = other.contacts[0].normal;
            beeRigigdoby.AddForce(bounceDirection * bouceForce, ForceMode2D.Impulse);
            
        }
        void OnTriggerEnter2D(Collider2D other)
        {
          if (other.gameObject.CompareTag("Water"))
            {
                gameObject.SetActive(false);
            }
        }
        public void Init(List<Doghead> dogheads)
        {
            mTargets = dogheads;

        }
    }
}


