using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IAminiGame : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] private GameObject[] prefabsLevels;
    [SerializeField] private GameObject currentLevel;
    [SerializeField] private GameObject cubesPlayer;
    [SerializeField] private GameObject pince;
    [SerializeField] private GameObject hole;

    [SerializeField] private GameObject SpawningPlateform;
    [SerializeField] public int level = -1;

    bool ascended = true;

    [Header("Timer")]
    [SerializeField] public float timer = 60f;
    [SerializeField] private float timeToChangeLevel = 60f;

    [Header("Points")]
    [SerializeField] private int points = 0;
    int pointsToNextLevel = 0;

    [SerializeField] private int pointsNumberBlocks = 0;
    [SerializeField] private int pointsBlockSize = 0;
    [SerializeField] private int pointsBlockRotation = 0;
    [SerializeField] private int pointsBlockLocalPos = 0;

    [SerializeField] private float differenceSize = 0.2f;
    [SerializeField] private float differenceRotation = 15f;
    [SerializeField] private float differencePosition = 3f;

    private bool levelSpawned = false;
    bool startTimer = false;
    bool pointsChecked = false;
    Voice voice;


    private void Start()
    {
        timer = timeToChangeLevel;
        points = 0;

        pince.SetActive(false);
        hole.SetActive(false);

        voice = Player.GetVoice();
    }



    private void Update()
    {
        if (!voice.tutoFinished)
        {
            if (!levelSpawned)
            {
                levelSpawned = true;
                StartCoroutine(SummonBlockLevel());
            }

            else if (voice.startTimer)
            {
                timer -= Time.deltaTime;
            }
        }

        else
        {
            if (voice.startTimer)
            {
                voice.startTimer = false;
                timer = timeToChangeLevel;
                startTimer = true;
            }

            if (timer <= 0 )
            {
                if (!pointsChecked)
                {
                    pointsChecked = true;
                    CheckPointsToNextLevel();
                    CheckPoints();
                    levelSpawned = false;

                    if (points >= pointsToNextLevel)
                    {
                        if(!levelSpawned)
                        {
                            levelSpawned = true;
                            StartCoroutine(SummonBlockLevel());
                            voice.PlayVoice(Random.Range(11, 12));
                        }
                    }
                    else
                    {
                        level -= 1;
                        StartCoroutine(SummonBlockLevel());
                        voice.PlayVoice(Random.Range(13, 16));
                    }
                }


                
            }
            else if (startTimer)
            {
                timer -= Time.deltaTime;
            }



        }


        }


    void CheckPointsToNextLevel()
    {
        pointsToNextLevel = 4 * currentLevel.transform.childCount;
    }
    private void CheckPoints()
    {
        CheckNumberBlocks();
        CheckBlockSize();
        CheckBlockRotation();
        CheckBlockLocalPos();

        points = pointsNumberBlocks + pointsBlockSize + pointsBlockRotation + pointsBlockLocalPos;
        Debug.Log("Points: " + points);
    }

    void CheckNumberBlocks()
    {
        int numberBlocksPlayer = cubesPlayer.transform.childCount;
        int numberBlocksLevel = currentLevel.transform.childCount;

        if (numberBlocksPlayer == numberBlocksLevel)
        {
            pointsNumberBlocks++;
        }
        else
        {
            pointsNumberBlocks--;
        }
    }

    void CheckBlockSize()
    {
        List<GameObject> blockAIChecked = new List<GameObject>();


        foreach (Transform cubePT in cubesPlayer.transform)
        {
            GameObject cubeP = cubePT.gameObject;

            GameObject cubeCheckedAI = currentLevel.transform.GetChild(0).gameObject;
            float difference_x = Mathf.Abs(cubeP.transform.localScale.x - cubeCheckedAI.transform.localScale.x);
            float difference_y = Mathf.Abs(cubeP.transform.localScale.y - cubeCheckedAI.transform.localScale.y);
            float difference_z = Mathf.Abs(cubeP.transform.localScale.z - cubeCheckedAI.transform.localScale.z);
            float difference = difference_x + difference_y + difference_z;

            foreach (Transform cubeAIT in currentLevel.transform)
            {
                GameObject cubeAI = cubeAIT.gameObject;

                if (!blockAIChecked.Contains(cubeAI))
                {
                    float difference_xAI = Mathf.Abs(cubeP.transform.localScale.x - cubeAI.transform.localScale.x);
                    float difference_yAI = Mathf.Abs(cubeP.transform.localScale.y - cubeAI.transform.localScale.y);
                    float difference_zAI = Mathf.Abs(cubeP.transform.localScale.z - cubeAI.transform.localScale.z);
                    float differenceAI = difference_xAI + difference_yAI + difference_zAI;

                    if (differenceAI < difference)
                    {
                        difference = differenceAI;
                        cubeCheckedAI = cubeAI;
                    }
                }
            }

            if (difference < differenceSize)
            {
                pointsBlockSize++;
                blockAIChecked.Add(cubeCheckedAI);
            }
               
        }
    }

    void CheckBlockRotation()
    {
        List<GameObject> blockAIChecked = new List<GameObject>();

        foreach (Transform cubePT in cubesPlayer.transform)
        {
            GameObject cubeP = cubePT.gameObject;

            GameObject cubeCheckedAI = currentLevel.transform.GetChild(0).gameObject;
            float difference_x = Mathf.Abs(cubeP.transform.rotation.x - cubeCheckedAI.transform.rotation.x);
            float difference_y = Mathf.Abs(cubeP.transform.rotation.y - cubeCheckedAI.transform.rotation.y);
            float difference_z = Mathf.Abs(cubeP.transform.rotation.z - cubeCheckedAI.transform.rotation.z);
            float difference = difference_x + difference_y + difference_z;

            foreach (Transform cubeAIT in currentLevel.transform)
            {
                GameObject cubeAI = cubeAIT.gameObject;

                if (!blockAIChecked.Contains(cubeAI))
                {
                    float difference_xAI = Mathf.Abs(cubeP.transform.rotation.x - cubeAI.transform.rotation.x);
                    float difference_yAI = Mathf.Abs(cubeP.transform.rotation.y - cubeAI.transform.rotation.y);
                    float difference_zAI = Mathf.Abs(cubeP.transform.rotation.z - cubeAI.transform.rotation.z);
                    float differenceAI = difference_xAI + difference_yAI + difference_zAI;

                    if (differenceAI < difference)
                    {
                        difference = differenceAI;
                        cubeCheckedAI = cubeAI;
                    }
                }
            }

            if (difference < differenceRotation)
            {
                pointsBlockRotation++;
                blockAIChecked.Add(cubeCheckedAI);

            }
        }
    }

    void CheckBlockLocalPos()
    {
        List<GameObject> blockAIChecked = new List<GameObject>();

        foreach (Transform cubePT in cubesPlayer.transform)
        {
            GameObject cubeP = cubePT.gameObject;

            GameObject cubeCheckedAI = currentLevel.transform.GetChild(0).gameObject;
            float difference_x = Mathf.Abs(cubeP.transform.localPosition.x - cubeCheckedAI.transform.localPosition.x * cubeCheckedAI.transform.localScale.x);
            float difference_y = Mathf.Abs(cubeP.transform.localPosition.y - cubeCheckedAI.transform.localPosition.y * cubeCheckedAI.transform.localScale.y);
            float difference_z = Mathf.Abs(cubeP.transform.localPosition.z - cubeCheckedAI.transform.localPosition.z * cubeCheckedAI.transform.localScale.z);
            float difference = difference_x + difference_y + difference_z;

            foreach (Transform cubeAIT in currentLevel.transform)
            {
                GameObject cubeAI = cubeAIT.gameObject;

                if (!blockAIChecked.Contains(cubeAI))
                {
                    float difference_xAI = Mathf.Abs(cubeP.transform.localPosition.x - cubeAI.transform.localPosition.x * cubeCheckedAI.transform.localScale.x);
                    float difference_yAI = Mathf.Abs(cubeP.transform.localPosition.y - cubeAI.transform.localPosition.y * cubeCheckedAI.transform.localScale.y);
                    float difference_zAI = Mathf.Abs(cubeP.transform.localPosition.z - cubeAI.transform.localPosition.z * cubeCheckedAI.transform.localScale.z);
                    float differenceAI = difference_xAI + difference_yAI + difference_zAI;

                    if (differenceAI < difference)
                    {
                        difference = differenceAI;
                        cubeCheckedAI = cubeAI;
                    }
                }
            }

            if (difference < differencePosition)
            {
                pointsBlockLocalPos++;
                blockAIChecked.Add(cubeCheckedAI);
            }
        }
    }




    IEnumerator SummonBlockLevel()
    {
        startTimer = false;
        level++;
        pince.SetActive(true);
        hole.SetActive(true);

        if(level != 0)
        {
            ascended = false;
            StartCoroutine(BlocksAscending());
        }

        while (!ascended)
        {
            yield return null;
        }

        if(level >= prefabsLevels.Length)
        {

            pince.SetActive(false);
            hole.SetActive(false);

            SceneManager.LoadScene("GameOver");
        }
        else
        {


            currentLevel = Instantiate(prefabsLevels[level], SpawningPlateform.transform.position + Vector3.up * 8, Quaternion.identity);
            currentLevel.transform.parent = SpawningPlateform.transform;

            pince.transform.position = SpawningPlateform.transform.position + Vector3.up * (8 + currentLevel.transform.localScale.y);

            StartCoroutine(BlocksDescending());

        }
        while(pince.activeSelf)
        {
            yield return null;
        }

        startTimer = true;
        pointsChecked = false;
        timer = timeToChangeLevel * (level + 1);

    }



    IEnumerator BlocksDescending()
    {


        while(currentLevel.transform.position.y > SpawningPlateform.transform.position.y)
        {
            currentLevel.transform.position += Vector3.down * Time.deltaTime;
            pince.transform.position += Vector3.down * Time.deltaTime;
            yield return null;
        }

        while (pince.transform.position.y < SpawningPlateform.transform.position.y + 8)
        {
            pince.transform.position += Vector3.up * Time.deltaTime;
            yield return null;
        }

        pince.SetActive(false);
        hole.SetActive(false);
    }

    IEnumerator BlocksAscending()
    {
        while (pince.transform.position.y > SpawningPlateform.transform.position.y + currentLevel.transform.localScale.y /2)
        {
            pince.transform.position += Vector3.down * Time.deltaTime;
            yield return null;
        }

        while (currentLevel.transform.position.y < SpawningPlateform.transform.position.y + 8)
        {
            currentLevel.transform.position += Vector3.up * Time.deltaTime;
            pince.transform.position += Vector3.up * Time.deltaTime;
            yield return null;
        }

        ascended = true;
        GameObject.Destroy(currentLevel);
        foreach (Transform cube in cubesPlayer.transform)
        {
            GameObject.Destroy(cube.gameObject);
        }

    }
}
