using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float minDistance;

    [SerializeField] 
    private float maxDistance;

    private Transform player;
    private EnemyManager enemyManager;
    private CharacterController characterController;
    private Shooter shooter;

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = EnemyManager.GetInstance();
        if (enemyManager == null)
        {
            Debug.Log("NEED ENEMY MANAGER AAAAAAAAAAA");
        }
        enemyManager.Add(this);

        characterController = GetComponent<CharacterController>();
        player = enemyManager.GetPlayerTransform();
        shooter = GetComponent<Shooter>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.position);
        Vector3 playerDirection = player.position - transform.position;
        float distance = playerDirection.magnitude;
        playerDirection.Normalize();
        if (distance < maxDistance)
        {
            shooter.Shoot(playerDirection, "Player");
        }
        if (distance < minDistance - 0.05)
        {
            playerDirection = -playerDirection;
        }
        else if (distance < minDistance + 0.01)
        {
            playerDirection = Vector3.zero;
        }
        characterController.SimpleMove(playerDirection * speed);
 
    }

    private void OnDestroy()
    {
        enemyManager.Unsubscribe(this);
    }
}
