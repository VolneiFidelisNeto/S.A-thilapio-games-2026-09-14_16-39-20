using UnityEngine;

public class Barril : MonoBehaviour
{
    [Header("Configurações da Explosão")]
    public float radius = 5.0f;
    public float forçaExplosao = 500.0f; // Intensidade do empurrão
    public int danoExplosao = 50;

    [Header("Efeitos Visuais")]
    public GameObject explosionPrefab;

    public void TakeDamage()
    {
        Explode();
    }

    private void Explode()
    {
        // 1. Instancia o efeito visual
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }

        // 2. Encontra todos os objetos no raio da explosão
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D hit in colliders)
        {
            // Evita que o barril empurre a si mesmo antes de ser destruído
            if (hit.gameObject == gameObject) continue;

            // --- APLICA A FORÇA FÍSICA (2D) ---
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Calcula a direção da explosão até o objeto
                Vector2 direcao = hit.transform.position - transform.position;

                // Quanto mais distante, menor o impacto da força
                float distancia = direcao.magnitude;
                float taxaForca = 1f - Mathf.Clamp01(distancia / radius);

                // Aplica o empurrão (Impulse é ideal para explosões instantâneas)
                rb.AddForce(direcao.normalized * forçaExplosao * taxaForca, ForceMode2D.Impulse);
            }

            // --- APLICA O DANO ---
            Inimigo inimigo = hit.GetComponent<Inimigo>();
            if (inimigo != null)
            {
                inimigo.ReceberDano(danoExplosao);
            }

            Jogador jogador = hit.GetComponent<Jogador>();
            if (jogador != null)
            {
                jogador.ReceberDano(danoExplosao);
            }
        }

        // 3. Destrói o barril
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.orange;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}