using UnityEngine;

/// <summary>
/// Coloque no objeto filho do jogador chamado "WeaponHolder".
/// Ele fica a uma distância do jogador e gira para a posição do mouse,
/// levando junto a arma equipada.
/// </summary>
public class WeaponHolder : MonoBehaviour
{
    [Header("Referências")]
    [Tooltip("Normalmente o próprio jogador (pai deste objeto).")]
    [SerializeField] private Transform player;

    [Header("Mira")]
    [SerializeField] private bool aimAtMouse = true;
    [Tooltip("Distância do centro do jogador até a arma.")]
    [SerializeField] private float distanceFromPlayer = 0.6f;
    [Tooltip("Ajuste se o sprite da arma não aponta para a direita (ex.: -90 se aponta para cima).")]
    [SerializeField] private float rotationOffset = 0f;
    [Tooltip("0 = instantâneo. Valores maiores = giro mais suave.")]
    [SerializeField] private float rotationSmoothing = 0f;
    [Tooltip("Inverte o sprite no eixo Y quando mira para a esquerda.")]
    [SerializeField] private bool flipWhenAimingLeft = true;

    private Camera cam;

    /// <summary>Direção atual da mira (normalizada). Útil para o script de tiro.</summary>
    public Vector2 AimDirection { get; private set; } = Vector2.right;

    private void Awake()
    {
        cam = Camera.main;
        if (player == null && transform.parent != null) player = transform.parent;
    }

    private void Update()
    {
        if (!aimAtMouse || player == null || cam == null) return;

        // Posição do mouse no mundo 2D
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector2 direction = (mouseWorld - player.position);
        if (direction.sqrMagnitude < 0.0001f) return;
        direction.Normalize();
        AimDirection = direction;

        // Posição: orbita em volta do jogador
        transform.position = (Vector2)player.position + direction * distanceFromPlayer;

        // Rotação: aponta para o mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;
        Quaternion target = Quaternion.Euler(0f, 0f, angle);

        transform.rotation = rotationSmoothing <= 0f
            ? target
            : Quaternion.Lerp(transform.rotation, target, rotationSmoothing * Time.deltaTime);

        // Evita a arma ficar de cabeça para baixo quando mira para a esquerda
        if (flipWhenAimingLeft)
        {
            Vector3 scale = transform.localScale;
            scale.y = direction.x < 0f ? -Mathf.Abs(scale.y) : Mathf.Abs(scale.y);
            transform.localScale = scale;
        }
    }
}