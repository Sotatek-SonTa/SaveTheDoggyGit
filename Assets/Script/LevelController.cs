using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveTheDoggyStup;
using SaveTheDoggDoghead;

namespace SaveTheDoggyLevelController
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private List<Doghead> dogHead = new List<Doghead>();
        [SerializeField] private List<Stup> stups = new List<Stup>();
        public event Action DogDeadAction;
        private void OnValidate()
        {

            dogHead = new List<Doghead>(GetComponentsInChildren<Doghead>());
            stups = new List<Stup>(GetComponentsInChildren<Stup>());
        }
        void Start()
        {
            foreach (Doghead item in dogHead)
            {
                item.OnDie += OnDogDead;
            }
        }
        void OnDogDead()
        {
            DogDeadAction?.Invoke();
        }
        public void ActiveLevelStart()
        {
            OnValidate();
            foreach (var item in dogHead)
            {
                item.SetGravityScale(1);
            }
            foreach (var item in stups)
            {
                item.SpawnBee(dogHead);
            }
        }

    }
}

