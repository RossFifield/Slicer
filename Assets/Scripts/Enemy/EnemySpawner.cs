using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
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
    private float spawningCloseLimit = 3;

    [SerializeField]
    private float spawningFarLimit = 30;



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
                float distance = Vector3.Distance(spawnPoint.position,player.transform.position);
                if(distance >= spawningCloseLimit && distance <= spawningFarLimit){
                    SpawnPointVariables spawnVariable = spawnPoint.gameObject.GetComponent<SpawnPointVariables>();
                    bool watchable = spawnVariable == null? true : spawnVariable.isWatchable;
                    if(watchable){
                        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                    }
                    else{
                        // not watchable code
                        Plane[] planes=GeometryUtility.CalculateFrustumPlanes(Camera.main);
                        Bounds boundary = spawnPoint.gameObject.GetComponent<Collider>().bounds;
                        if(!GeometryUtility.TestPlanesAABB(planes, boundary)){
                            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                        }
                        else{
                            Vector3 direction = spawnPoint.transform.position - Camera.main.transform.position;
                            int layerMask = 1<<0;
                            bool rayResult = Physics.Raycast(Camera.main.transform.position,direction,direction.magnitude,layerMask,QueryTriggerInteraction.Ignore);
                            if(rayResult){
                                Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

                            }
                            else{
                                Debug.Log("Oh no! player is looking at spawn point : "+ spawnPoint.position.ToString());
                            }                          
                        }
                    }
                }
                else{
                    Debug.Log("Player out of range for spawn point : "+ spawnPoint.position.ToString());
                }    
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
