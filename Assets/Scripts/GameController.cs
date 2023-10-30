using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float timeLimit=300;
    public int killCountTarget= 30;
    public GameObject character;
    public GameObject gameManager;
    private int killCount=0;

    private float startTime;

    [HideInInspector]
    public float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time;
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
}
