using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    // Ja no necessitem una variable pública de velocitat aquí,
    // perquè la llegirem del GameManager.
    
    void Update()
    {
        // Si el joc s'ha acabat, no ens movem
        if (GameManager.isGameOver) return;

        // Obtenim la velocitat global actual
        float currentSpeed = GameManager.Instance.gameSpeed;

        // Ens movem cap a l'esquerra a la velocitat que dicta el GameManager
        transform.Translate(Vector2.left * currentSpeed * Time.deltaTime, Space.World);

        // Destruir quan fora de pantalla
        if (transform.position.x < -20f) Destroy(gameObject);
    }
    
    // Ja no cal el mètode StopMoving() perquè comprovem isGameOver a cada frame
}

