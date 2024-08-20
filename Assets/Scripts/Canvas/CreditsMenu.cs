using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsMenu : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject[] panels;


    private void Start()
    {
        buttons[0].onClick.AddListener(() => BackToMainMenu());

        this.gameObject.SetActive(false);
    }


    void BackToMainMenu()
    {
        Debug.Log("Back to Main Menu");
        this.gameObject.SetActive(false);
        panels[0].SetActive(true);
    }
}
