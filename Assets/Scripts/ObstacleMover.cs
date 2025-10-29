using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float leftSpeed = 5f;
    void Update()
    {
        transform.Translate(Vector2.left * leftSpeed * Time.deltaTime, Space.World);

        // Destruir quan fora de pantalla (x < -20 per exemple)
        if (transform.position.x < -20f) Destroy(gameObject);
    }

    // Optional: si vols que la velocitat sigui controlada pel GameManager
    public void SetSpeed(float speed)
    {
        leftSpeed = speed;
    }
}

