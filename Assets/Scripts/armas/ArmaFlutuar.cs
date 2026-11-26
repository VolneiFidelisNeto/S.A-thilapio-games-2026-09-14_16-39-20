using UnityEngine;

public class ArmaFlutuar : MonoBehaviour
{
    [Header("Animação flutuante")]
    [SerializeField] private bool flutuar = true;
    [SerializeField] private float amplitude = 0.1f;
    [SerializeField] private float velocidade = 2f;
    private Vector3 posicaoInicial;
    private Collider2D col;
    private SpriteRenderer[] sprites;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
        sprites = GetComponentsInChildren<SpriteRenderer>();
        posicaoInicial = transform.position;
    }

    private void Update()
    {
        if (!flutuar) return;
        float y = Mathf.Sin(Time.time * velocidade) * amplitude;
        transform.position = posicaoInicial + new Vector3(0f, y, 0f);
    }

}
