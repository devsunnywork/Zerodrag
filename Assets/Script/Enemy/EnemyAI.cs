using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Transform[] waypoints;
    public float speed = 2f;
    public float chasespeed = 5f;
    private int currentWaypointIndex = 0;
    public float detectionRange = 15f;
    public float attackRange = 3f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

   void Update()
    {
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
        agent.stoppingDistance = 0.5f;

      
        agent.SetDestination(waypoints[currentWaypointIndex].position);

        float distanceToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position);

        if (distanceToWaypoint < 2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void Chase()
    {
        agent.speed = chasespeed;

        agent.SetDestination(player.position);
    }

    void Attack()
    {
        agent.SetDestination(transform.position); 
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
