using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    private NavMeshAgent agent;
    private Animator animator;


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
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.stoppingDistance = minDistance;
    }

    void handleAnimation()
    {
        bool isRetreating = animator.GetBool("isRetreating");
        bool isIdle = animator.GetBool("isStill");
        bool isAdvancing = animator.GetBool("isAdvancing");

        if (isIdle)
        {
            animator.SetBool("isStill", true);
        }
        else if (isRetreating)
        {
            animator.SetBool("isRetreating", true);
        }
        else if (isAdvancing)
        {
            animator.SetBool("isAdvancing", true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookPos);
        Vector3 playerDirection = player.position - transform.position;
        float distance = playerDirection.magnitude;
        playerDirection.Normalize();
        if (distance < maxDistance)
        {
            shooter.Shoot(playerDirection, "Player");
            //handleAnimation();
        }
        agent.destination = player.position;
    }


    private void OnDestroy()
    {
        enemyManager.Unsubscribe(this);
    }
}
