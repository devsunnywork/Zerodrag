using UnityEngine;
using TMPro;

public class missiondistance : MonoBehaviour
{ 
    
    public static missiondistance instance;

    [Header("Setup")]
    public Transform player;
    public TextMeshProUGUI distanceText;

    
    private Transform currentTarget; 

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        CalculateDistance(); 
    }

    
    public void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }

    // Mission khatam hone par target hatane ke liye
    public void ClearTarget()
    {
        currentTarget = null;
        if (distanceText != null) distanceText.text = "";
    }

    void CalculateDistance()
    {
       
        if (currentTarget != null && player != null)
        {
            float distance = Vector3.Distance(player.position, currentTarget.position);
            distanceText.text = Mathf.Round(distance).ToString() + "m";
        }
        else
        {
            if (distanceText != null) distanceText.text = "";
        }
    }
}