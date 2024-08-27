using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Start()
    {
        GameObject.Destroy(LevelRestarting.GetInstancee());

        button.onClick.AddListener(() => QuitGame());

        this.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }




    public void QuitGame()
    {
        Debug.Log("Exit Game");
        SceneManager.LoadScene("MainMenu");
    }
}
