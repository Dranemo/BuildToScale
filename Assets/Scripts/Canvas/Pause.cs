using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    static public bool paused = false;

    [SerializeField] Button[] buttons;
    [SerializeField] Slider slider;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().planeDistance = -1;
        paused = false;
        Time.timeScale = 1;

        

        buttons[0].onClick.AddListener(() => Resume());
        buttons[1].onClick.AddListener(() => RestartLevel());
        buttons[2].onClick.AddListener(() => LoadScene("MainMenu"));

        slider.onValueChanged.AddListener((float value) => Player.GetMainCamera().GetComponent<CameraController>().mouseSensitivity = value);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Debug.Log("pause");
            if (paused)
            {
                Resume();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void Resume()
    {
        GetComponent<Canvas>().planeDistance = -1;
        Time.timeScale = 1;
        paused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void PauseGame()
    {
        GetComponent<Canvas>().planeDistance = 1;
        Time.timeScale = 0;
        paused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void LoadScene(string scene)
    {
        paused = false;
        SceneManager.LoadScene(scene);
    }

    void RestartLevel()
    {
        LevelRestarting.GetInstancee().GetComponent<LevelRestarting>().levelInt = Player.GetVoice().gameObject.GetComponent<IAminiGame>().level;
        LoadScene("Minigame");
    }
}
