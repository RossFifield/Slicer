using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillCountControl : MonoBehaviour
{
    public GameObject enemyController;
    private TMP_Text text;
    private EnemyManager enemyManager;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        enemyManager = enemyController.GetComponent<EnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Kills: " + ((int)enemyManager.killCount).ToString();
    }
}
