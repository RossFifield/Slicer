using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    private HashSet<Enemy> enemySet;
    [SerializeField]
    private int enemyCount = 0;

    [SerializeField]
    private int enemyLimit = 6;

    [SerializeField]
    public int killCount = 0;

    private static EnemyManager instance = null;

    public int getEnemyCount(){
        return enemyCount;
    }
    public static EnemyManager GetInstance()
    {
        return instance;
    }

    public Transform GetPlayerTransform()
    {
        return player;
    }

    public void Add(Enemy enemy)
    {
        if (enemyCount >= enemyLimit)
        {
            Destroy(enemy.gameObject);
            return;
        }
        enemySet.Add(enemy);
        enemyCount = enemySet.Count;

    }

    public void Unsubscribe(Enemy enemy)
    {
        if (enemySet.Contains(enemy))
        {
            enemySet.Remove(enemy);
            killCount++;
        }
        else
        {
            //Debug.Log("CANT UNSUB SINCE ALREADY UNSUBBED");
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("MORE THAN ONE INSTANCE OF ENEMY MANAGER AAAAAAAAAAAAAA");
        }

        enemySet = new HashSet<Enemy>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        enemyCount = enemySet.Count;
    }
}
