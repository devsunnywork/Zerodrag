using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] waypoints;
    public float speed = 2f;
    public float waitTime = 3f;
    public float runSpeed = 6f;
    public float fleeDistance = 15f;
    private bool isWaiting = false;
    private bool isFleeing = false;
    private int currentWaypointIndex = 0;
    private Animator animator;

    void Start()
    {
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
        if (isWaiting) return;
        animator.SetBool("isWalking", true);
        agent.SetDestination(waypoints[currentWaypointIndex].position);

        float distanceToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position);

        if (distanceToWaypoint < 2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
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
        isFleeing = true;
        isWaiting = false;
        agent.isStopped = false;
        agent.speed = runSpeed;
        animator.SetBool("isRunning", true);
        animator.SetBool("isWalking", false);

        Vector3 fleeDirection = transform.position - GameObject.FindWithTag("Player").transform.position;
        Vector3 fleeTarget = transform.position + fleeDirection.normalized * fleeDistance;
        agent.SetDestination(fleeTarget);
    }
}
