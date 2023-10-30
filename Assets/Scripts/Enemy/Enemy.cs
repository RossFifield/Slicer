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
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerDirection = (player.position - transform.position).normalized;
        characterController.SimpleMove(playerDirection * speed);
    }

    private void OnDestroy()
    {
        enemyManager.Unsubscribe(this);
    }
}
