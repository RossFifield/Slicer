using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed;

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
        Vector3 playerDirection = (player.position - transform.position).normalized;
        characterController.SimpleMove(playerDirection * speed);
        transform.LookAt(player.position);
        shooter.Shoot(playerDirection, "Player");
 
    }

    private void OnDestroy()
    {
        enemyManager.Unsubscribe(this);
    }
}
