using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private List<int> spawnLimits;

    [SerializeField]
    private float spawningCloseLimit = 3;

    [SerializeField]
    private float spawningFarLimit = 100;
    private Dictionary<Transform,int> spawnCount = new Dictionary<Transform, int>();
    private Dictionary<Transform,int> spawnLimitation = new Dictionary<Transform, int>();
    private List<Transform> exhaustedSpawn = new List<Transform>();
    private static EnemySpawner instance = null;

    public static EnemySpawner GetInstance()
    {
        return instance;
    }

    public List<Transform> GetExhaustedSpawn(){
        return exhaustedSpawn;
    }
    public int GetSpawnCount(){
        return spawnPoints.Count;
    }

    public int GetTotalEnemies(){
        int sum =0;
        foreach(int i in spawnLimits){
            sum+=i;
        }
        return sum;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("MORE THAN ONE INSTANCE OF ENEMY SPAWNER AAAAAAAAAAAAAA");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        int i =0;
        foreach (Transform spawnPoint in spawnPoints){
            spawnCount.Add(spawnPoint,0);
            spawnLimitation.Add(spawnPoint,spawnLimits[i]);
            i++;
        }
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
                //Debug.Log("Current Spawn Count is: "+ spawnCount[spawnPoint] +" and the limit is "+spawnLimitation[spawnPoint] +" for Spawn point: "+spawnPoints.IndexOf(spawnPoint));
                if(spawnCount[spawnPoint]<spawnLimitation[spawnPoint]){
                    float distance = Vector3.Distance(spawnPoint.position,player.transform.position);
                    if(distance >= spawningCloseLimit && distance <= spawningFarLimit){
                        SpawnEnemy(spawnPoint);                       
                    }
                    else{
                        //Debug.Log("Player out of range for spawn point : "+ spawnPoints.IndexOf(spawnPoint));
                    }    
                }
                else{                   
                    if(!exhaustedSpawn.Contains(spawnPoint)){
                        //Debug.Log("Exhausted Spawn point: "+ spawnPoints.IndexOf(spawnPoint));
                        exhaustedSpawn.Add(spawnPoint);
                    }
                }
                
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }


    private void SpawnEnemy(Transform spawnPoint){
        SpawnPointVariables spawnVariable = spawnPoint.gameObject.GetComponent<SpawnPointVariables>();
        bool watchable = spawnVariable == null? true : spawnVariable.isWatchable;
        if(watchable){
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            spawnCount[spawnPoint] +=1;
        }
        else{
            // not watchable code
            Plane[] planes=GeometryUtility.CalculateFrustumPlanes(Camera.main);
            Bounds boundary = spawnPoint.gameObject.GetComponent<Collider>().bounds;
            if(!GeometryUtility.TestPlanesAABB(planes, boundary)){
                Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                spawnCount[spawnPoint] +=1;
            }
            else{
                Vector3 direction = spawnPoint.transform.position - Camera.main.transform.position;
                int layerMask = 1<<0;
                bool rayResult = Physics.Raycast(Camera.main.transform.position,direction,direction.magnitude,layerMask,QueryTriggerInteraction.Ignore);
                if(rayResult){
                    Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                    spawnCount[spawnPoint] +=1;
                }
                else{
                    //Debug.Log("Oh no! player is looking at spawn point : "+ spawnPoints.IndexOf(spawnPoint));
                }                          
            }
        }
    }
}
