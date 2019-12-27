using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Grid : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField]
    Vector2 enemyGrid = new Vector2(6f,6f);
    [SerializeField]
    Vector3 spacing;
    [SerializeField]
    int enemy_Count;

    [Space]

    [Header("Movement")]
    [SerializeField]
    Vector3 direction;
    [SerializeField]
    [Range(0.1f,3f)]
    float velocity = 1;
    [SerializeField]
    [Range(0.1f, 3f)]
    float refreshTime = 1;
    [SerializeField]
    bool downTrigger;

    [Space]

    [Header("Mothership")]
    [SerializeField]
    GameObject mothership_Prefab;
    [SerializeField]
    Transform[] spawnposition_Mothership;
    [SerializeField]
    bool isMother;


    private void Start()
    {
        CreateEnemys(enemyGrid);
        Game_Controller.instance.startDelegate += StartMoving;
    }

    public void CreateEnemys(Vector2 numEnemys)
    {
        int lengthX = (int)numEnemys.x;
        int lengthY = (int)numEnemys.y;

        float firstX = ((numEnemys.x / 2f) * spacing.x * -1) + (spacing.x/2);

        GameObject tempEnemy;
        Vector3 enemyPosition = new Vector3(firstX,0);

        for (int i = 0; i < lengthX; i++)
        {
            for (int j = 0; j < lengthY; j++)
            {
                tempEnemy = Instantiate(enemyPrefab, transform);
                tempEnemy.GetComponent<Enemy_Individual>().SetEnemyGrid(this);
                NewEnemy();
                tempEnemy.transform.localPosition = enemyPosition;
                if (j == 1 || j == 3)
                {
                    if (i == 0 || i == lengthX-1)
                    {
                        tempEnemy.GetComponent<Enemy_Individual>().SetType(Enemy_Individual.EnemyType.Shooter);
                    }
                }
                enemyPosition.y -= spacing.y;
            }
            enemyPosition.x += spacing.x;
            enemyPosition.y = 0;

        }
    }

    #region EnemyCount
    public void DeadEnemy()
    {
        enemy_Count--;
        if (enemy_Count.Equals(0))
        {
            Game_Controller.instance.gOverDelegate();
        }
    }

    public void NewEnemy()
    {
        enemy_Count++;
    }
    #endregion

    public void StartMoving()
    {
        StartCoroutine("Move");
        StartCoroutine("SpawnMothershipCoroutine");
    }

    public void MoveDown()
    {
        if (downTrigger)
        {
            downTrigger = false;
            direction.x = direction.x * -1.1f;

            transform.Translate(Vector3.down);
        }
        
    }

    IEnumerator Move()
    {
        while (true)
        {
            transform.Translate(direction * velocity);
            yield return new WaitForSeconds(refreshTime);
            downTrigger = true;
        }
    }

    public void SetIsMother(bool b)
    {
        isMother = b;
    }

    void SpawnMotherShip()
    {
        int rand = Random.Range(0, 2);
        if (rand == 2)
        {
            rand = 0;
        }

        GameObject mothTemp = Instantiate(mothership_Prefab, spawnposition_Mothership[rand].position, Quaternion.identity);
        mothTemp.GetComponent<Enemy_Individual>().SetEnemyGrid(this);
        mothTemp.GetComponent<Enemy_Individual>().SetType(Enemy_Individual.EnemyType.Mothership);
        NewEnemy();

        if (rand == 0)
        {
            mothTemp.GetComponent<Enemy_Individual>().MoveMothership(Vector3.right);
        }
        else
        {
            mothTemp.GetComponent<Enemy_Individual>().MoveMothership(Vector3.left);
        }
        
    }

    IEnumerator SpawnMothershipCoroutine()
    {
        float rand;

        while (true)
        {
            if (isMother == false)
            {
                rand = Random.Range(0f, 11f);
                if (rand == 11)
                {
                    rand = 0;
                }
                if (rand < 5)
                {
                    SpawnMotherShip();
                    isMother = true;
                }
            }
            yield return new WaitForSeconds(4f);          
        }
    }
}
