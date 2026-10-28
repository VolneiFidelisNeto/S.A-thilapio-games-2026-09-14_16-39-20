using UnityEngine;

public class camera : MonoBehaviour
{
    [Header("Alvo e Offset")]
    public Transform target;          // Arraste o jogador para cá
    public Vector3 offset = new Vector3(0f, 0f, -10f); // -10 no Z para manter a câmera visível

    [Header("Configurações de Suavização")]
    public float smoothSpeed = 5f;    // Velocidade de interpolação

    void LateUpdate()
    {
        if (target == null) return;

        // Posição desejada somando a posição do jogador com o offset
        Vector3 targetPosition = target.position + offset;

        // Suaviza o movimento usando Lerp
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Atualiza a posição da câmera
        transform.position = smoothedPosition;
    }

}
