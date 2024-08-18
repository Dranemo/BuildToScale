using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAminiGame : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsLevels;
    [SerializeField] private GameObject currentLevel;
    [SerializeField] private GameObject cubesPlayer;

    [SerializeField] private GameObject SpawningPlateform;

    [SerializeField] private float timer = 0.0f;
    [SerializeField] private float timeToChangeLevel = 5.0f;

    [SerializeField] private int points = 0;


    private void Start()
    {
        if(prefabsLevels.Length > 0)
        {
            currentLevel = Instantiate(prefabsLevels[0], SpawningPlateform.transform.position, Quaternion.identity);
        }

        timer = 0.0f;
        points = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeToChangeLevel)
        {
            CheckPoints();


            timer = 0.0f;
            points = 0;


            if (prefabsLevels.Length > 0)
            {
                Destroy(currentLevel);
                currentLevel = Instantiate(prefabsLevels[Random.Range(0, prefabsLevels.Length)], SpawningPlateform.transform.position, Quaternion.identity);
            }
        }
    }



    private void CheckPoints()
    {

    }

    void CheckNumberBlocks()
    {
        int numberBlocksPlayer = cubesPlayer.transform.childCount;
        int numberBlocksLevel = currentLevel.transform.childCount;

        if(numberBlocksPlayer == numberBlocksLevel)
        {
            points++;
        }
    }
}
