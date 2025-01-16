using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using SaveTheDoggyLevelController;

namespace SaveTheDoggyLevelManager
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<LevelController> levels;
        public LevelController currentLevel;
        public Transform levelHolder;
        public bool doneDrawing;
        public event Action DogDead;
        void Start()
        {
            if (currentLevel != null)
            {
                Destroy(currentLevel.gameObject);
                Destroy(currentLevel);
            }
            LoadAllLevel();
        } 
        public void LoadLevel(int levelIndex)
        {
            if (currentLevel != null)
            {
                Destroy(currentLevel.gameObject);
                Destroy(currentLevel);
            }
            currentLevel = Instantiate(levels[levelIndex], levelHolder.position, Quaternion.identity);
            currentLevel.transform.SetParent(levelHolder);
            currentLevel.DogDeadAction += ExecuteDogDead;
        }

        private void ExecuteDogDead()
        {
            DogDead?.Invoke();
        }

        private void LoadAllLevel()
        {
            levels = new List<LevelController>(Resources.LoadAll<LevelController>("Levels"));
            levels.Sort((a, b) =>
            {
                int aNumber = ExtractLevelNumber(a.name);
                int bNumber = ExtractLevelNumber(b.name);
                return aNumber.CompareTo(bNumber);
            });
        }
        private int ExtractLevelNumber(string levelName)
        {
            string numberString = System.Text.RegularExpressions.Regex.Replace(levelName, "[^0-9]", "");
            int levelNumber = 0;

            if (!string.IsNullOrEmpty(numberString))
            {
                int.TryParse(numberString, out levelNumber);
            }

            return levelNumber;
        }
        public void StartLevel()
        {
            currentLevel.ActiveLevelStart();
        }

    }
}

