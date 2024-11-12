using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] private int currentLife;
    [SerializeField] private int maxLife;
    [SerializeField] private int enemyScorePoint;

    [Header("Patrol")]
    [SerializeField] private GameObject patrolPointsContainer;
    private List<Transform> patrolPoints = new List<Transform>();
    private int destinationPoint = 0; //internal index to next destination
    private bool isChasing = false ; //is Chasing Player

    private NavMeshAgent agent;

    private WeaponController weaponController;
    
    //Player
    private Transform playerTransform;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        weaponController = GetComponent<WeaponController>();

        //Take all the children of patrolPointContainer and add them in the patrolPoints array
        foreach (Transform child in patrolPointsContainer.transform)
            patrolPoints.Add(child);

        //First time go to Next Patrol
        GotoNextPatrolPoint();
    }


    private void Update()
    {
        //search player with Ray Cast
        SearchPlayer();

        //Choose next destination point when the agents get close to the current one
        if (!isChasing && !agent.pathPending && agent.remainingDistance < 3f)
            GotoNextPatrolPoint();
    }

    /// <summary>
    /// Enemy Go to next destinationPoint
    /// </summary>
    private void GotoNextPatrolPoint()
    {
        //Restart the stopping distance to 0 to posibility the Patrol
        agent.stoppingDistance = 0f;

        //set the agent to the currently destination Point
        agent.SetDestination(patrolPoints[destinationPoint].position);

        if (agent.remainingDistance <= 0.5f) { 
            //choose next destinationPoint in the List
            //cycling to the start if necessary
            destinationPoint = (destinationPoint + 1) % patrolPoints.Count;
        }

    }


    /// <summary>
    /// Enemy search and go towards player
    /// </summary>
    private void SearchPlayer()
    {
        NavMeshHit hit;
        //if no obstacles between enemy and player
        if (!agent.Raycast(playerTransform.position, out hit))
        {
            //Go towards Player only if is at 10m or lower
            if (hit.distance <= 10f)
            {
                isChasing = true; //Chase Player
                agent.SetDestination(playerTransform.position);
                agent.stoppingDistance = 3f;
                transform.LookAt(playerTransform.position);
            
                
                //Stop Enemy at 5m 
                if (hit.distance < 5f) 
                {
                    agent.isStopped = true;
                }
                else
                {
                    agent.isStopped = false;
                }

                //shoot Player if distance between them is lower than 7m
                if (hit.distance <= 7f)
                {
                    if (weaponController.CanShoot())
                        weaponController.Shoot();
                }
            }
            //If the player more than 10f distance
            else
            {
                agent.isStopped = false;
                isChasing = false;
            }
        }
        //Player Not in the Ray Cast 
        else
        {
            agent.isStopped = false;
            isChasing = false;
        }

    }

    /// <summary>
    /// Handle when the enemy receive a bullet
    /// </summary>
    /// <param name="quantity">Damage quantity</param>
    public void DamageEnemy(int quantity)
    {
        currentLife -= quantity;
        if (currentLife <= 0)
            Destroy(gameObject);
    }

}
