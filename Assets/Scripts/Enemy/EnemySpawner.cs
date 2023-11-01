using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<Transform> spawnPoints;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private int spawnInterval = 3;

    [SerializeField]
    private float spawningDistanceLimit = 3;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                SpawnPointVariables spawnVariable = spawnPoint.gameObject.GetComponent<SpawnPointVariables>();
                bool watchable = spawnVariable == null? true : spawnVariable.isWatchable;
                if(watchable){
                    float distance = Vector3.Distance(spawnPoint.position,player.transform.position);

                    if(distance >= spawningDistanceLimit){
                        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                    }
                    else{
                        Debug.Log("player is too close!");
                    }
                }
                else{
                    // not watchable code
                    Plane[] planes=GeometryUtility.CalculateFrustumPlanes(Camera.main);
                    Bounds boundary = spawnPoint.gameObject.GetComponent<Collider>().bounds;
                    if(!GeometryUtility.TestPlanesAABB(planes, boundary)){
                        float distance = Vector3.Distance(spawnPoint.position,player.transform.position);
                        if(distance >= spawningDistanceLimit){
                            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                        }
                        else{
                            Debug.Log("player is too close!");
                        }
                    }
                    else{
                        Debug.Log("Oh no! player is looking at me");
                    }
                }
                
                
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
