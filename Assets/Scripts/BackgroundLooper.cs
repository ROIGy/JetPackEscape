using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Necessari per ordenar llistes

public class BackgroundLooper : MonoBehaviour
{
    [Header("Configuració")]
    public float speedMultiplier = 0.5f; 
    public float pieceWidth = 20f; // Amplada exacta de cada peça de fons (han de ser iguals)

    [Header("Elements de Fons")]
    // Arrossega aquí tots els teus objectes (Fons_1, Fons_2, Fons_3...)
    public List<Transform> backgroundPieces; 

    void Update()
    {
        if (GameManager.isGameOver) return;

        float currentSpeed = GameManager.Instance.gameSpeed * speedMultiplier;
        float moveAmount = currentSpeed * Time.deltaTime;

        // 1. Movem TOTS els objectes cap a l'esquerra
        foreach (Transform piece in backgroundPieces)
        {
            piece.Translate(Vector2.left * moveAmount);
        }

        // 2. Comprovem qui ha sortit de la pantalla
        // Assumim que la càmera està a X=0. El límit és quan passa de -amplada.
        // Afegim un petit marge de seguretat (ex: -pieceWidth - 1f)
        float leftLimit = -pieceWidth; 

        foreach (Transform piece in backgroundPieces)
        {
            if (piece.position.x < leftLimit)
            {
                // Aquest objecte ha sortit! L'hem de posar al final de la cua.
                RepositionPiece(piece);
            }
        }
    }

    void RepositionPiece(Transform piece)
    {
        // 1. Busquem qui és l'objecte que està més a la dreta ara mateix
        // (Ordenem la llista per posició X i agafem l'últim)
        Transform lastPiece = backgroundPieces.OrderByDescending(p => p.position.x).First();

        // 2. Ens posem exactament darrere seu
        // La nova posició és: Posició de l'últim + Amplada
        Vector3 newPos = lastPiece.position;
        newPos.x += pieceWidth;
        // Mantenim la Y i Z originals de la peça
        newPos.y = piece.position.y; 
        newPos.z = piece.position.z;

        piece.position = newPos;
    }
}