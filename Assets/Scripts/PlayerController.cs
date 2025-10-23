using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Jetpack")]
    public float baseThrust = 15f;       // força inicial quan es prem
    public float maxThrust = 37f;    //Força màxima després d'acumular temps
    public float thrustRampUpTime = 0.1f; //temps que triga a arribar a maxThrust
    public float gravity = 23f;          //Gravetat normal
    public float maxY = 4f;
    public float minY = -3.8f;
    public float maxVerticalSpeed = 6f;



    [Header("Grounding")]
    public LayerMask obstacleLayer;

    [Header("Death fall")]
    public float deathGravityMultiplier = 3f;
    public float deathAngularVelocity = 300f;
    public float settleTolerance = 0.05f;

    Rigidbody2D rb;
    public bool isAlive = true;
    float thrustTime = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Congel·lem la rotació inicialment
        rb.gravityScale = gravity / 9.8f; //Ajustar segons "Rigidbody2D" (segons gust)
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // Moviment només si està viu
        if (!isAlive) return;

        // Control del jetpack
        bool up = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetMouseButton(0);

        if (up)
        {
            //Augmenta thrust (força de pujada) a mida que es manté premut
            thrustTime += Time.deltaTime;
            float t = Mathf.Clamp01(thrustTime / thrustRampUpTime);
            float thrust = Mathf.Lerp(baseThrust, maxThrust, t);

            //rb.linearVelocity = new Vector2(rb.linearVelocity.x, thrust);
            rb.linearVelocity = new Vector2(0f, thrust);
        }
        else
        {
            thrustTime = 0f; // reinici quan deixes de prémer
        }

        //Clamp vertical (versió amb thrust exponencial)
        // Limit vertical speed i evitar sortir del rang
        if (rb.position.y > maxY && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(0f, 0f);
        }
        else if (rb.position.y < minY && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(0f, 0f);
        }




        // Clamp vertical
        //if (transform.position.y > maxY)
        //{
        //    transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        //    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        //}
        //if (transform.position.y < minY)
        //{
        //    transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        //    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        //}
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!isAlive) return;

        if (col.CompareTag("Obstacle"))
        {
            StartCoroutine(DieAndFall());
        }
    }

    IEnumerator DieAndFall()
    {
        // Desactivar moviment
        isAlive = false;

        // Permetre rotació i augmenta gravetat per caiguda ràpida
        rb.constraints = RigidbodyConstraints2D.None;
        rb.gravityScale *= deathGravityMultiplier;

        // Manté X fix
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        // Aplica velocitat angular per tombar mentre cau
        rb.angularVelocity = deathAngularVelocity;

        //Baixar una mica el valor de minY per a que el player quedi tombat al terra
        float deathMinY = minY - 0.55f;

        // Esperar fins que arribi al terra (minY)
        while (rb.position.y > deathMinY + settleTolerance)
        {
            yield return null; // esperar frame
        }

        // Atura moviment i rotació
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Fixem rotació final a -90 graus
        rb.rotation = -90f;

        // Congel·la posició i rotació per deixar el player quiet
        rb.constraints = RigidbodyConstraints2D.FreezePositionX |
                         RigidbodyConstraints2D.FreezePositionY |
                         RigidbodyConstraints2D.FreezeRotation;

        // Notifica al GameManager DE MOMENT NO HEM FET GAMEMANAGER

        //if (GameManager.Instance != null)
        //{
        //    GameManager.Instance.PlayerDied();
        //}
    }
}
