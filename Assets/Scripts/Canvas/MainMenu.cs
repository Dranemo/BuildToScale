using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject[] panels;

    private void Start()
    {
        buttons[0].onClick.AddListener(() => StartGame());
        buttons[1].onClick.AddListener(() => ShowPanel(1));
        buttons[2].onClick.AddListener(() => ExitGame());

        this.gameObject.SetActive(true);
    }

    private void ShowPanel(int index)
    {
        this.gameObject.SetActive(false);
        panels[index].SetActive(true);
    }

    public void StartGame()
    {
        Debug.Log("Start Game");
        SceneManager.LoadScene("Minigame");
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
