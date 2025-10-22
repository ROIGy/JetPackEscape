using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Jetpack")]
    public float thrust = 2.5f;             // força vertical mentre es prem
    public float maxY = 4f;
    public float minY = -3.8f;


    public float maxVerticalSpeed = 6f;   // límit de velocitat vertical
    public bool isAlive = true;

    [Header("Grounding")]
    public LayerMask obstacleLayer;

    Rigidbody2D rb;
    Vector2 velocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        bool up = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetMouseButton(0);

        if (up)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, thrust);
        }

        if (transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
        if (transform.position.y < minY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }

    }

    void OnCollisionEnter2D(Collision2D col)
{
    if (col.gameObject.CompareTag("Obstacle"))
    {
        Debug.Log("Game Over!");
        // Més endavant cridarem GameManager
    }
}

}
