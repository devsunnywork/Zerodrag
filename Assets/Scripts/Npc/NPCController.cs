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

   
    private Transform playerTransform;
  
    private Vector3 lastDestination;
    private bool destinationSet = false;

    [Header("Player Block Detection")]
    public float playerDetectRange = 2.5f;    
    public float playerDetectAngle = 60f;    
    private bool isPlayerBlocking = false;

    void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

     
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) playerTransform = playerObj.transform;
        
        if (waypoints.Length > 0)
        {
            currentWaypointIndex = Random.Range(0, waypoints.Length);
        }

        agent.speed = speed;
        animator = GetComponent<Animator>();

        
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        if (col != null)
        {
            col.height = 2f;
            col.center = new Vector3(0, 1f, 0);
            col.radius = 0.3f;
            col.isTrigger = false;
        }
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

 
        CheckPlayerBlocking();

        if (!isPlayerBlocking)
            Patrol();
    }


    void CheckPlayerBlocking()
    {
        if (playerTransform == null) return;

        Vector3 dirToPlayer = playerTransform.position - transform.position;
        float distToPlayer = dirToPlayer.magnitude;

        
        if (distToPlayer <= playerDetectRange)
        {
            float angle = Vector3.Angle(transform.forward, dirToPlayer);
            if (angle <= playerDetectAngle * 0.5f)
            {
                if (!isPlayerBlocking)
                {
                    isPlayerBlocking = true;
                    agent.isStopped = true;
                    animator.SetBool("isWalking", false);
                }
                return;
            }
        }

        if (isPlayerBlocking)
        {
            isPlayerBlocking = false;
            agent.isStopped = false;
        }
    }

    void Patrol()
    {
        if (isWaiting || waypoints.Length == 0) return;
        animator.SetBool("isWalking", true);

        Vector3 targetPos = waypoints[currentWaypointIndex].transform.position;
        if (!destinationSet || targetPos != lastDestination)
        {
            agent.SetDestination(targetPos);
            lastDestination = targetPos;
            destinationSet = true;
        }

        if (!agent.pathPending && agent.remainingDistance < 2f)
        {
            currentWaypointIndex = Random.Range(0, waypoints.Length);
            destinationSet = false; // Naya waypoint set hoga
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
        if (playerTransform == null) return;

        isFleeing = true;
        isWaiting = false;
        agent.isStopped = false;
        agent.speed = runSpeed;
        animator.SetBool("isRunning", true);
        animator.SetBool("isWalking", false);

        Vector3 fleeDirection = transform.position - playerTransform.position;
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
        Destroy(gameObject);
    }
}
