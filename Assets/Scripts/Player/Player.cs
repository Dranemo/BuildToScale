using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    private static Camera mainCamera;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (mainCamera == null)
        {
            mainCamera = gameObject.transform.Find("Main Camera").GetComponent<Camera>();
        }
    }

    public static GameObject GetPlayer()
    {
        if (instance != null)
        {
            return instance.gameObject;
        }
        else
        {
            Debug.LogError("Player instance not found!");
            return null;
        }
    }

    public static Camera GetMainCamera()
    {
        if (mainCamera != null)
        {
            return mainCamera;
        }
        else
        {
            Debug.LogError("Main Camera not found!");
            return null;
        }
    }

}
