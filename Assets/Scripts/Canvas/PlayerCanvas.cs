using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Image[] cursors;
    [SerializeField] private TextMeshProUGUI textTime;
    [SerializeField] private TextMeshProUGUI textLevel;
    IAminiGame iaminiGame;

    public enum CursorType
    {
        Default,
        Select,
        Move,
        Scale
    }

    public CursorType cursorType;


    void Start()
    {
        cursorType = CursorType.Default;
        iaminiGame = Player.GetVoice().gameObject.GetComponent<IAminiGame>();
    }

    private void Update()
    {
        textLevel.text = "Level: " + iaminiGame.level;
        textTime.text = iaminiGame.timer.ToString("F2");

        /*Debug.Log(cursorType);*/

        switch (cursorType)
        {
            case CursorType.Default:
                cursors[0].gameObject.SetActive(true);
                cursors[1].gameObject.SetActive(false);
                cursors[2].gameObject.SetActive(false);
                cursors[3].gameObject.SetActive(false); 
                break;
            case CursorType.Select:
                cursors[0].gameObject.SetActive(false);
                cursors[1].gameObject.SetActive(true);
                cursors[2].gameObject.SetActive(false);
                cursors[3].gameObject.SetActive(false);
                break;
            case CursorType.Move:
                cursors[0].gameObject.SetActive(false);
                cursors[1].gameObject.SetActive(false);
                cursors[2].gameObject.SetActive(true);
                cursors[3].gameObject.SetActive(false);
                break;
            case CursorType.Scale:
                cursors[0].gameObject.SetActive(false);
                cursors[1].gameObject.SetActive(false);
                cursors[2].gameObject.SetActive(false);
                cursors[3].gameObject.SetActive(true);
                break;
        }
    }
}
