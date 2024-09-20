using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levels;
    public GameObject currentLevel;
    public Transform levelHolder;
    public bool doneDrawing;
    
    void Start()
    {
        if(currentLevel !=null){
            Destroy(currentLevel);
        }
    }
    public void LoadLevel(int levelIndex){
         if (currentLevel != null)
        {
            Destroy(currentLevel);
        }
        currentLevel = Instantiate(levels[levelIndex],levelHolder.position,Quaternion.identity);
        currentLevel.transform.SetParent(levelHolder);
    }
}
