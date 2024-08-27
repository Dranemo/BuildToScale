using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    private static Camera mainCamera;
    private static Canvas canvaPlayer;
    private static Voice voice;

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

        if(canvaPlayer == null)
        {
            canvaPlayer = GameObject.Find("Canvas").GetComponent<Canvas>();
        }

        if (voice == null)
        {
            voice = GameObject.Find("IA").GetComponent<Voice>();
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

    public static Canvas GetCanvasPlayer()
    {
        if(canvaPlayer != null)
        {
            return canvaPlayer;
        }
        else
        {
            Debug.LogError("Canvas Player not found!");
            return null;
        }
    }

    public static Voice GetVoice()
    {
        if (voice != null)
        {
            return voice;
        }
        else
        {
            Debug.LogError("Voice not found!");
            return null;
        }
    }

}
