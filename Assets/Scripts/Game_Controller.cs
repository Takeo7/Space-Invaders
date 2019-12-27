using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Controller : MonoBehaviour
{
    #region Singleton

    public static Game_Controller instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    [Header("Global Stats")]
    [SerializeField]
    int score;
    public bool InGame = false;

    [Space]

    [Header("Controllers")]
    [SerializeField]
    UI_Controller uic;

    #region Delegates

    public delegate void startGame_Delegate();
    public startGame_Delegate startDelegate;

    public delegate void gameOver_Delegate();
    public gameOver_Delegate gOverDelegate;

    #endregion

    private void Start()
    {
        gOverDelegate += StopTime;
        StopTime();
        
    }

    public void UpScore(byte s)
    {
        score += s;
        uic.SetScore(score);
    }

    #region Time Management

    public void StopTime()
    {
        Time.timeScale = 0;
    }
    public void ResumeTime()
    {
        Time.timeScale = 1;
    }

    #endregion

    #region Enums
    public enum Character
    {
        Player,
        Enemy
    }
    #endregion
}
