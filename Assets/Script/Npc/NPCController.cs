using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public NavMeshAgent agent;
    private GameObject[] waypoints; 
    public float speed = 1f;
    public float waitTime = 3f;
    public float runSpeed = 10f;
    public float fleeDistance = 50f;
    public float health = 100f;
    private bool isWaiting = false;
    private bool isFleeing = false;
    private int currentWaypointIndex = 0;
    private Animator animator;

    void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        
        if (waypoints.Length > 0)
        {
            currentWaypointIndex = Random.Range(0, waypoints.Length);
        }

        agent.speed = speed;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isFleeing)
        {
            if (!agent.pathPending && agent.remainingDistance < 2f)
            {
                isFleeing = false;
                agent.speed = speed;
                animator.SetBool("isRunning", false);
            }
            return;
        }

        Patrol();
    }

    void Patrol()
    {
        if (isWaiting || waypoints.Length == 0) return;
        animator.SetBool("isWalking", true);

        agent.SetDestination(waypoints[currentWaypointIndex].transform.position);

        float distanceToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypointIndex].transform.position);

        if (distanceToWaypoint < 2f)
        {
            currentWaypointIndex = Random.Range(0, waypoints.Length);
            StartCoroutine(WaitAtWaypoint());
        }
    }

    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        agent.isStopped = true;
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("isWalking", true);
        agent.isStopped = false;
        isWaiting = false;
    }

    public void StartFleeing()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj == null) return;

        isFleeing = true;
        isWaiting = false;
        agent.isStopped = false;
        agent.speed = runSpeed;
        animator.SetBool("isRunning", true);
        animator.SetBool("isWalking", false);

        Vector3 fleeDirection = transform.position - playerObj.transform.position;
        Vector3 fleeTarget = transform.position + fleeDirection.normalized * fleeDistance;
        agent.SetDestination(fleeTarget);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Die();
        }

    }

    void Die()
    {
    }
}
