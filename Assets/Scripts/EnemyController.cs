using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] private int currentLife;
    [SerializeField] private int maxLife;
    [SerializeField] private int enemyScorePoint;


    private NavMeshAgent agent;

    private WeaponController weaponController;
    
    //Player
    private Transform playerTransform;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        weaponController = GetComponent<WeaponController>();
    }


    private void Update()
    {
        SearchPlayer();
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
            if (hit.distance <= 10f) { 
                    agent.SetDestination(playerTransform.position);
                    agent.stoppingDistance = 5f;
                    transform.LookAt(playerTransform.position);

                    //shoot Player if distance between them is lower than 7m
                    if (hit.distance <= 7f)
                    {
                        if (weaponController.CanShoot())
                            weaponController.Shoot();
                    }

            }

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
