using UnityEngine;

public class LineEndTrigger : MonoBehaviour
{
    public LineManager lineManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            lineManager.AdvanceLine();
        }
    }
}
