using UnityEngine;

public class NPCAnimationManager : MonoBehaviour
{
    private Animator animator;

    public Transform player;
    private bool isTalking = false;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("NPCAnimationManager: Animator not found on " + gameObject.name + " or its children!");
        }

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (isTalking && player != null)
        {
            Vector3 lookDirection = player.position - transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }

    public void StartTalking()
    {
        isTalking = true;
        Debug.Log("NPCAnimationManager: StartTalking called on " + gameObject.name);
        
        // Stop movement if NavMeshAgent is present
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            if (animator != null) {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }
        }

        if (animator != null) animator.SetBool("isTalking", true);
    }

    public void StopTalking()
    {
        isTalking = false;
        
        // Resume movement if NavMeshAgent is present
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.isStopped = false;

        if (animator != null) animator.SetBool("isTalking", false);
    }
}
