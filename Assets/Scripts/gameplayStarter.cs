using UnityEngine;

public class GameplayStarter : MonoBehaviour
{
    public Transform player;
    public Vector3 playerStartPosition = new Vector3(-7f, 0f, 0f);

    public Rigidbody2D playerRb;

    public void StartLevel()
    {
        Time.timeScale = 1f;

        if (player != null)
        {
            player.position = playerStartPosition;
        }

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
        }
    }
}
