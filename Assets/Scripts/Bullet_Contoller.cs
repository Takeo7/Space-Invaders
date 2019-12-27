using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Contoller : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    [Range(0.1f, 5f)]
    float movForce = 0.1f;
    [SerializeField]
    [Range(0.1f, 5f)]
    float refreshTime = 0.1f;

    Vector3 dir;

    [Space]

    [SerializeField]
    Game_Controller.Character launcher;

    [Space]

    [SerializeField]
    bool launch;



    IEnumerator Start()
    {
        if (launcher == Game_Controller.Character.Player)
        {
            dir = Vector3.up * movForce;
        }
        else
        {
            dir = Vector3.down * movForce;
        }

        while (launch)
        {
            transform.Translate(dir);
            yield return new WaitForSeconds(refreshTime);
        }
    }

    void DestroyMyself()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            DestroyMyself();
        }
        else if (launcher.Equals(Game_Controller.Character.Player) && collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy_Individual>().GetHit();
            DestroyMyself();
        }
        else if (launcher.Equals(Game_Controller.Character.Enemy) && collision.CompareTag("Player"))
        {
            collision.GetComponent<Player_Controller>().GetHit();
            DestroyMyself();
        }
    }

    public void SetLaunch()
    {
        launch = true;
    }

}
