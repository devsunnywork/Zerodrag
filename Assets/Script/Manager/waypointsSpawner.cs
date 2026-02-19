using UnityEngine;
using UnityEngine.AI; 

public class waypointsSpawner : MonoBehaviour
{
   public GameObject waypointPrefab;
   public int waypointCount = 20;
   public float waypointRadius = 50f;
   public float minDistanceBetween = 5f; 
   public Transform waypointParent;     

    void Awake() 
    {
        for(int i = 0; i < waypointCount; i++)
        {
            SpawnAtRandomPosition();
        }
    }

    void SpawnAtRandomPosition()
    {
        bool spawned = false;
        int attempts = 0;

        while (!spawned && attempts < 10)
        {
            Vector3 randomPos = transform.position + Random.insideUnitSphere * waypointRadius;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPos, out hit, 20f, 1 << 0))
            {
                GameObject newWaypoint = Instantiate(waypointPrefab, hit.position, Quaternion.identity, waypointParent);
                newWaypoint.tag = "Waypoint"; 
                spawned = true;
            }
            attempts++;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, waypointRadius);
    }
}