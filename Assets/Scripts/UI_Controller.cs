using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_Controller : MonoBehaviour
{
    [Header("Game Controller")]

    [SerializeField]
    Game_Controller gc;

    [Space]

    [Header("Start Game")]

    [SerializeField]
    GameObject startGame_Canvas;
    [SerializeField]
    TextMeshProUGUI countDown_Text;

    [Space]

    [Header("InGame")]

    [SerializeField]
    Transform lives_Images;
    [SerializeField]
    TextMeshProUGUI score_Text;

    [Space]

    [Header("GameOver")]

    [SerializeField]
    GameObject gameOver_Canvas;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("StartScreen") && PlayerPrefs.GetInt("StartScreen") == 0)
        { 
            UIStartGame();
        }
    }

    private void Start()
    {
        gc.gOverDelegate += ShowGameOverPop;
        gc.startDelegate += UIResumeTime;
    }

    public void UIStartGame()
    {
        PlayerPrefs.SetInt("StartScreen", 1);
        startGame_Canvas.SetActive(false);
        StartCountDown(true);
    }

    public void SetHealth(byte h)
    {
        byte length = (byte)lives_Images.childCount;
        for (int i = length-1; i >= h; i--)
        {
            if (lives_Images.GetChild(i).gameObject.activeSelf == true)
            {
                lives_Images.GetChild(i).gameObject.SetActive(false);
            }           
        }
    }

    public void SetScore(int s)
    {
        score_Text.text = "Score: " + s;
    }

    public void ShowGameOverPop()
    {
        gameOver_Canvas.SetActive(true);
        UIStoptime();
    }

    #region Time management

    public void UIStoptime()
    {
        gc.StopTime();
    }

    public void UIResumeTime()
    {
        gc.ResumeTime();
    }

    #endregion

    #region Game Over

    public void UIGameOver(bool mainMenu)
    {
        if (mainMenu)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            PlayerPrefs.SetInt("StartScreen", 0);
            SceneManager.LoadScene(0);
        }
    }

    #endregion

    #region Count Down

    public void StartCountDown(bool isStart)
    {
        if (isStart)
        {
            UIStoptime();
        }
        StartCoroutine(CountDown(isStart));
    }

    #region Coroutines

    IEnumerator CountDown(bool s)
    {
        byte time = 3;
        
        while (time > 0)
        {
            countDown_Text.text = time.ToString();
            yield return new WaitForSecondsRealtime(1f);
            time--;
        }
        countDown_Text.text = "";
        if (s)
        {
            gc.startDelegate();
        }
        else
        {
            UIResumeTime();
        }
            
    }

    #endregion

    #endregion

}
