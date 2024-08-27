using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRestarting : MonoBehaviour
{
    // Start is called before the first frame update
    static GameObject instance;
    public int levelInt = 1;


    private void Awake()
    {
        if (instance == null)
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameObject GetInstancee()
    {
        return instance;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
