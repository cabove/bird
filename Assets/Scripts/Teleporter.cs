using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Spawner spawner;
    [SerializeField] private Transform teleportDestination;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // teleport
            if (teleportDestination != null)
            {
                other.transform.position = teleportDestination.position;
                spawner.SpawnObject();
            }
        }
    }
}
