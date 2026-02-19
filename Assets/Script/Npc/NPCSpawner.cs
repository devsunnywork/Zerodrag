using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
   public GameObject[] npcPrefabs; 
   public int npcCount = 100;
   public float spawnRadius = 100f;
   public Transform npcParent; 

    void Start()
    {        
        for (int i = 0;  i < npcCount; i++)
        {
            SpawnNPC();
        }
    }

    void SpawnNPC()
    {
        bool spawned = false;
        int attempts = 0;

        while (!spawned && attempts < 10)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(spawnPosition, out hit, 20f, 1 << 0))
            {
                if (npcPrefabs.Length > 0)
                {
                    int randomIndex = Random.Range(0, npcPrefabs.Length);
                    Instantiate(npcPrefabs[randomIndex], hit.position, Quaternion.identity, npcParent);
                    spawned = true; 
                }
            }
            attempts++;
        }
    }
}
