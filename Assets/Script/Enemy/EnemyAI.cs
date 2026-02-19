using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    private Transform player;
    private GameObject[] waypoints; 
    public float speed = 2f;
    public float chaseSpeed = 5f;
    public float detectionRange = 15f;
    public float attackRange = 3f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;
    private int currentWaypointIndex = 0;

    void Start()
    {
    
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        agent.speed = speed;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)       
        {
            Attack();
        }
        else if (distanceToPlayer <= detectionRange) 
        {
            Chase();
        }
        else                                      
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        agent.speed = speed;
        agent.SetDestination(waypoints[currentWaypointIndex].transform.position);

        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].transform.position) < 2f)
        {
            currentWaypointIndex = Random.Range(0, waypoints.Length);
        }
    }

    void Chase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    void Attack()
    {
        agent.SetDestination(transform.position); // Ruk jao attack ke liye
        
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
       
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
            lastAttackTime = Time.time;
        }
    }
}
