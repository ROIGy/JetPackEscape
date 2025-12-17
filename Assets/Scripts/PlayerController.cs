using System.Collections;
using UnityEngine;
using static GameManager;


public class PlayerController : MonoBehaviour
{
    [Header("Jetpack")]
    public float baseThrust = 0.05f;       // força inicial quan es prem
    public float maxThrust = 10f;    //Força màxima després d'acumular temps
    public float thrustRampUpTime = 0.005f; //temps que triga a arribar a maxThrust
    public float gravity = 1000f;          //Gravetat normal
    //public float maxY = 4f;
    public float minY = -3.8f;

    //public float maxVerticalSpeed = 6f;

    [Header("Grounding")]
    public LayerMask obstacleLayer;

    [Header("Death fall")]
    public float deathGravityMultiplier = 3f;
    public float deathAngularVelocity = 300f;
    public float settleTolerance = 0.05f;

    [Header("Audio")]
    public AudioClip deathSound; // <--- Arrossega aquí el so d'explosió/xoc
    public AudioSource jetpackSource;

// --- VARIABLES PER A LA FÍSICA DINÀMICA ---
    private float initialGravity;
    private float initialBaseThrust;
    private float initialMaxThrust;
    private float startSpeed; // Per saber quina era la velocitat original
    // ------------------------------------------------

    Rigidbody2D rb;
    public bool isAlive = true;
    float thrustTime = 0f;
    
    [Header("VFX")]
    public ParticleSystem jetpackParticles;

    public GameManager gaMa;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Congel·lem la rotació inicialment
        //rb.gravityScale = gravity / 8f; //Ajustar segons "Rigidbody2D" (segons gust)
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Start()
    {
        gaMa = GameManager.Instance; // Accedim al Singleton que vam crear abans

        // 1. GUARDEM ELS VALORS ORIGINALS (ELS QUE HAS POSAT A L'INSPECTOR)
        initialGravity = rb.gravityScale;
        initialBaseThrust = baseThrust;
        initialMaxThrust = maxThrust;
        
        // Guardem la velocitat inicial del joc per poder calcular la proporció
        if (gaMa != null)
        {
            startSpeed =gaMa.gameSpeed; 
        }
        else
        {
            // Valor de seguretat si no troba el GM
            startSpeed = 5f; 
        }
    }

    void FixedUpdate()
    {
        var emission = jetpackParticles.emission;

        // Moviment i emissió de partícules només si està viu
        if (!isAlive)
        {
            emission.enabled = false;
            if (jetpackSource.isPlaying) jetpackSource.Stop();
            return;
        } 
        
        // --- CÀLCUL DEL MULTIPLICADOR DE FÍSICA ---
        float physicsMultiplier = 1f;

        if (gaMa != null && startSpeed > 0)
        {
            // Exemple: Si gameSpeed és 10 i startSpeed era 5, el multiplicador és 2.
            physicsMultiplier = gaMa.gameSpeed / startSpeed;
        }

        // --- APLICAR ELS NOUS VALORS ---
        
        // 1. Escalem la gravetat
        rb.gravityScale = initialGravity * physicsMultiplier;

        // 2. Escalem la força del motor
        float currentBaseThrust = initialBaseThrust * physicsMultiplier;
        float currentMaxThrust = initialMaxThrust * physicsMultiplier;

        // 3. NOU: Escalem el temps de resposta (Més petit = Més ràpid)
        // Dividim pel multiplicador: Si anem al doble de velocitat, triguem la meitat a carregar.
        float currentRampUpTime = thrustRampUpTime / physicsMultiplier;

        // -------------------------------

        // Control del jetpack
        bool up = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetMouseButton(0);

        if (up)
        {
            //Augmenta thrust (força de pujada) a mida que es manté premut
            thrustTime += Time.deltaTime;

            float t = Mathf.Clamp01(thrustTime / currentRampUpTime);
            
            // Usem els valors calculats (current) 
            float thrust = Mathf.Lerp(currentBaseThrust, currentMaxThrust, t);

            rb.AddForce(Vector2.up * thrust, ForceMode2D.Force);

            //Activem emissió de partícules
            emission.enabled = true;

            if (!jetpackSource.isPlaying)
            {
                jetpackSource.Play();
            }
        }
        else
        {
            thrustTime = 0f; // reinici quan deixes de prémer
            emission.enabled = false;

            // Si deixem de prémer i sona, parem
            if (jetpackSource.isPlaying)
            {
                jetpackSource.Stop();
            }
        }

        

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
        //Passem variable de GameManager a true per a que deixin d'spawnejar nous obstacles
        isGameOver = true;
        
        // Desactivar moviment
        isAlive = false;

        // --- GESTIÓ D'ÀUDIO ---

        // 1. ATURAR MÚSICA DE FONS
        // Busquem l'objecte que hem anomenat "GameMusic"
        GameObject bgMusic = GameObject.Find("GameMusic");
        if (bgMusic != null)
        {
            bgMusic.GetComponent<AudioSource>().Stop();
        }

        // 2. REPRODUIR SO DE MORT
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, 1.0f);
        }

        // Permetre rotació i augmenta gravetat per caiguda ràpida
        rb.constraints = RigidbodyConstraints2D.None;
        rb.gravityScale = initialGravity * deathGravityMultiplier;

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


        // Notifica al GameManager
        gaMa.PlayerDied();
        
    }
}
