using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Transform[] waypoints;
    public float speed = 2f;
    private int currentWaypointIndex = 0;
    public float detectionRange = 15f;
    public float attackRange = 3f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        agent.speed = speed;
        agent.stoppingDistance = 0.5f;

        // Go to current waypoint
        agent.SetDestination(waypoints[currentWaypointIndex].position);

        // Check ACTUAL distance between enemy and waypoint (more reliable)
        float distanceToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position);

        if (distanceToWaypoint < 2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
