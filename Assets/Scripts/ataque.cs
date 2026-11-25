using UnityEngine;

/// <summary>
/// Arma 2D que mira no mouse e atira com o BOTÃO DIREITO.
/// Coloque este script no objeto da arma (ou do jogador).
/// </summary>
public class ArmaTiroBotaoDireito : MonoBehaviour
{
    [Header("Projétil")]
    [SerializeField] private GameObject projetilPrefab;   // prefab com Rigidbody2D + Collider2D
    [SerializeField] private Transform pontoDeTiro;       // objeto filho na ponta da arma
    [SerializeField] private float velocidadeProjetil = 12f;
    [SerializeField] private float tempoDeVidaProjetil = 3f;

    [Header("Disparo")]
    [SerializeField] private float cadenciaTiro = 0.3f;   // segundos entre tiros
    [SerializeField] private bool tiroAutomatico = false; // true = segurar o botão direito atira continuamente

    [Header("Mira")]
    [SerializeField] private bool girarParaOMouse = true;

    private float proximoTiro;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (girarParaOMouse)
            MirarNoMouse();

        // Botão direito do mouse = 1 (esquerdo = 0, meio = 2)
        bool apertou = tiroAutomatico ? Input.GetMouseButton(1) : Input.GetMouseButtonDown(1);

        if (apertou && Time.time >= proximoTiro)
        {
            Atirar();
            proximoTiro = Time.time + cadenciaTiro;
        }
    }

    private void MirarNoMouse()
    {
        Vector3 mundo = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direcao = mundo - transform.position;
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angulo);
    }

    private void Atirar()
    {
        if (projetilPrefab == null)
        {
            Debug.LogWarning("Projétil prefab não foi atribuído na arma!");
            return;
        }

        Transform origem = pontoDeTiro != null ? pontoDeTiro : transform;

        GameObject tiro = Instantiate(projetilPrefab, origem.position, origem.rotation);

        Rigidbody2D rb = tiro.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // "right" aponta para a frente da arma (eixo X local)
            // Unity 6: use rb.linearVelocity. Versões antigas: rb.velocity
            rb.linearVelocity = origem.right * velocidadeProjetil;
        }

        Destroy(tiro, tempoDeVidaProjetil);
    }
}