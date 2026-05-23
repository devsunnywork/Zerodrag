using UnityEngine;

public class Mission2PoliceManager : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Aapas mein baat karne ke liye
    public void StartTalking()
    {
        if (anim != null) anim.SetBool("isTalking", true);
    }

    // Rupesh ko dekhne ke liye (Idle)
    public void SetIdle()
    {
        if (anim != null)
        {
            anim.SetBool("isTalking", false);
            anim.SetFloat("Speed", 0f);
        }
    }

    // DHAKKA LAGNE PAR (Sirf Animation Trigger karega)
    public void PlayFallBack(Vector3 pushDirection, float force)
    {
        if (anim != null)
        {
            anim.SetTrigger("FallBack");
        }
    }
}
