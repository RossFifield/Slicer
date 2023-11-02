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
    public GameObject gameManager;    

    private float startTime;

    [HideInInspector]
    public float currentTime;
    public int killCount=0;

    private static GameController instance = null;

    public static GameController GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        killCountTarget = EnemySpawner.GetInstance().GetTotalEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time- startTime;
        // end game when kill count reaches target
        if(killCount >= killCountTarget){
            gameManager.GetComponent<Startup>().LoadNextLevel();
        }
        // end game when character health = 0
        if(character.GetComponent<PlayerController>().health<=0){
            gameManager.GetComponent<Startup>().LoadNextLevel();
        }
        // end the game when time is up
        if(currentTime-startTime >= timeLimit){
            gameManager.GetComponent<Startup>().LoadNextLevel();
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
