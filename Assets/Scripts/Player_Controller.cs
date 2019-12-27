using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    [Range(0.1f,5f)]
    float movForce = 0.1f;
    Vector3 dir;

    [Space]

    [SerializeField]
    byte health = 3;

    [Space]

    [Header("Prefabs & Components")]
    [SerializeField]
    GameObject bullet_prefab;
    [SerializeField]
    Transform launchPoint;
    [SerializeField]
    Collider2D col;
    [SerializeField]
    Rigidbody2D rb;

    [Space]

    [Header("Controllers")]
    [SerializeField]
    bool player_InGame;
    [SerializeField]
    Game_Controller gc;
    [SerializeField]
    UI_Controller uic;

    private void Start()
    {
        gc.startDelegate += SetInGame;
        gc.gOverDelegate += SetOffGame;
    }

    private void Update()
    {
        if (player_InGame)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                dir.x = Input.GetAxisRaw("Horizontal") * movForce * Time.deltaTime;
                transform.Translate(dir);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                GameObject tempBullet = Instantiate(bullet_prefab, launchPoint.position, Quaternion.identity);
                tempBullet.GetComponent<Bullet_Contoller>().SetLaunch();
            }
        }        
    }

    public void SetInGame()
    {
        player_InGame = true;
    }

    public void SetOffGame()
    {
        player_InGame = false;
    }

    public void GetHit()
    {
        health -= 1;
        uic.SetHealth(health);
        transform.position = new Vector3(0, transform.position.y);
        if (health <= 0)
        {
            gc.gOverDelegate();
        }
        else
        {
            gc.StopTime();
            uic.StartCountDown(false);
        }
    }

}
