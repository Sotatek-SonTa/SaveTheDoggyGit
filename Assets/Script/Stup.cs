using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveTheDoggyBee;
using SaveTheDoggDoghead;

namespace SaveTheDoggyStup
{
    public class Stup : MonoBehaviour
    {
        [SerializeField] private int beeCount = 0;
        [SerializeField] private Bee bee;
        [SerializeField] private float spawnInterval = 0.2f;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        IEnumerator BeeSpawn(List<Doghead> dogheads)
        {
            while (beeCount < 7)
            {
                var beeSpawmed = Instantiate(bee, gameObject.transform.position, Quaternion.identity);
                beeSpawmed.gameObject.transform.SetParent(gameObject.transform);
                beeCount++;
                beeSpawmed.Init(dogheads);
                yield return new WaitForSeconds(spawnInterval);
            }
        }
        public void SpawnBee(List<Doghead> dogheads)
        {
            StartCoroutine(BeeSpawn(dogheads));
        }
    }

}
