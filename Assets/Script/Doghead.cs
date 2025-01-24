using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveTheDoggDoghead
{
    public class Doghead : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        public Rigidbody2D rigidbody2D;
        public event Action OnDie;
        public Animator animator;

        void Start()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 0;
        }
        void Update()
        {

        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Bee"))
            {
                OnDie?.Invoke();
                TriggerAnimationAttacked();
            }

            if (other.gameObject.CompareTag("Spike"))
            {
                spriteRenderer.enabled = false;
                OnDie?.Invoke();
            }
             if (other.gameObject.CompareTag("BorderLine"))
            {
                spriteRenderer.enabled = false;
                OnDie?.Invoke();
            }
            
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("ToxicWater"))
            {
                spriteRenderer.enabled = false;
                OnDie?.Invoke();
            }
        }
        public void SetGravityScale(int i)
        {
            rigidbody2D.gravityScale = i;
        }
        public void TriggerAnimationAttacked()
        {
            animator.SetTrigger("GettingAttacked");
        }
    }


}
