using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy_Individual : MonoBehaviour
{
    [Header("Enemy Type")]
    [SerializeField]
    EnemyType type;
    [SerializeField]
    byte score = 1;
    [SerializeField]
    Enemy_Grid eg;
    

    [Space]

    [Header("Shooting")]
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    Transform shootPoint;

    [Space]

    [Header("UI")]
    [SerializeField]
    TextMeshProUGUI points_Text;

    [Space]

    [Header("Render")]
    [SerializeField]
    GameObject renderObject;
    [SerializeField]
    bool isHit = true;

    bool motherCanGo;

    IEnumerator Start()
    {
        if (type.Equals(EnemyType.Shooter) || type.Equals(EnemyType.Mothership))
        {
            while (true)
            {
                yield return new WaitForSeconds(2f);
                Shoot();
            }
        }
        yield return null;
    }

    public void SetType(EnemyType t)
    {
        type = t;

        switch (type)
        {
            case EnemyType.Default:
                score = 1;
                break;
            case EnemyType.Shooter:
                score = 2;
                break;
            case EnemyType.Mothership:
                score = 10;
                break;
            default:
                break;
        }
    }

    public void Shoot()
    {
        GameObject tempBullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        tempBullet.GetComponent<Bullet_Contoller>().SetLaunch();
    }

    public void GetHit()
    {
        if (isHit)
        {
            isHit = false;

            GetComponent<Collider2D>().enabled = false;

            points_Text.text = score.ToString();
            renderObject.SetActive(false);
            points_Text.gameObject.SetActive(true);
            Game_Controller.instance.UpScore(score);
            eg.DeadEnemy();

            if (type.Equals(EnemyType.Mothership))
            {
                eg.SetIsMother(false);
            }

            Destroy(gameObject, 1f);
        }
        
    }

    public void MoveMothership(Vector3 dir)
    {
        StartCoroutine("MoveMotherShipCoroutine", dir);
    }

    IEnumerator MoveMotherShipCoroutine(Vector3 dir)
    {
        while (true)
        {
            transform.Translate(dir);
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") && !type.Equals(EnemyType.Mothership))
        {
            eg.MoveDown();
        }
        else if (collision.CompareTag("Player"))
        {
            Game_Controller.instance.gOverDelegate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") && type.Equals(EnemyType.Mothership) && motherCanGo == true)
        {
            eg.DeadEnemy();
            Destroy(gameObject);
        }

        motherCanGo = true;
    }

    public void SetEnemyGrid(Enemy_Grid e)
    {
        eg = e;
    }

    public enum EnemyType
    {
        Default,
        Shooter,
        Mothership
    }
}
