using UnityEngine;

public class ataque : MonoBehaviour
{
    [Header("Configurações das Armas")]
    public GameObject[] prefabsProjeteis; // Lista com os prefabs de cada arma
    public float[] velocidadesProjeteis;   // Lista com as velocidades correspondentes
    private int indiceArmaAtual = 0;       // Índice da arma em uso

    [Header("Referências")]
    public Transform pontoDeDisparo;

    void Update()
    {
        // Troca de arma usando as teclas numéricas 1, 2, 3...
        for (int i = 0; i < prefabsProjeteis.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelecionarArma(i);
            }
        }

        // Troca de arma pelo Scroll do mouse
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            SelecionarArma((indiceArmaAtual + 1) % prefabsProjeteis.Length);
        }
        else if (scroll < 0f)
        {
            int novoIndice = indiceArmaAtual - 1;
            if (novoIndice < 0) novoIndice = prefabsProjeteis.Length - 1;
            SelecionarArma(novoIndice);
        }

        // Disparo
        if (Input.GetButtonDown("Fire1"))
        {
            Atirar();
        }
    }

    public void SelecionarArma(int indice)
    {
        if (indice >= 0 && indice < prefabsProjeteis.Length)
        {
            indiceArmaAtual = indice;
        }
    }

    void Atirar()
    {
        if (prefabsProjeteis.Length == 0 || pontoDeDisparo == null) return;

        GameObject prefabAtual = prefabsProjeteis[indiceArmaAtual];
        float velocidadeAtual = (velocidadesProjeteis.Length > indiceArmaAtual)
                                ? velocidadesProjeteis[indiceArmaAtual]
                                : 10f;

        // Instancia o projétil
        GameObject projetil = Instantiate(prefabAtual, pontoDeDisparo.position, Quaternion.identity);

        // Direção baseada na escala do personagem
        float direcao = transform.localScale.x > 0 ? 1f : -1f;

        // Ajusta a rotação/escala do projétil
        Vector3 escala = projetil.transform.localScale;
        escala.x = Mathf.Abs(escala.x) * direcao;
        projetil.transform.localScale = escala;

        // Movimenta o projétil
        Rigidbody2D rb = projetil.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(velocidadeAtual * direcao, 0);
        }
    }
}