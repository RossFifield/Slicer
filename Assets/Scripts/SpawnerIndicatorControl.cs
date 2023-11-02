using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnerIndicatorControl : MonoBehaviour
{
    private TMP_Text text;
    private EnemySpawner enemySpawner;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        enemySpawner = EnemySpawner.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Spawns: " + enemySpawner.GetExhaustedSpawn().Count+"/"+enemySpawner.GetSpawnCount();
    }
}
