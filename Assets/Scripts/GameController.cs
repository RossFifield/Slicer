using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public float timeLimit=300;
    private int killCountTarget= 30;
    public GameObject character;   
    private float startTime;

    [HideInInspector]
    public float currentTime;
    public int killCount=0;

    private static GameController instance = null;
    private PlayerController player;
    public int currentHP;

    public static GameController GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        killCountTarget = EnemySpawner.GetInstance().GetTotalEnemies();
        player = character.GetComponent<PlayerController>();
        currentHP = player.health;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time- startTime;
        // end game when kill count reaches target
        if(killCount >= killCountTarget){
            Startup.GetInstance().LoadNextLevel();
        }
        // end game when character health = 0
        if(player.health<=0){
            Startup.GetInstance().LoadNextLevel();
        }
        // end the game when time is up
        if(currentTime-startTime >= timeLimit){
            Startup.GetInstance().LoadNextLevel();
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
            Debug.Log("MORE THAN ONE INSTANCE OF GAME MANAGER AAAAAAAAAAAAAA");
        }

    }
}
