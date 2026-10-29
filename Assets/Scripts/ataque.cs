using UnityEngine;

public class ataque : MonoBehaviour
{
    public GameObject prefabProjetil;
    public Transform pontoDeDisparo;
    public float velocidadeProjetil = 10f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Atirar();
        }
    }

    void Atirar()
    {
        if (prefabProjetil == null || pontoDeDisparo == null) return;

        // Instancia o projétil
        GameObject projetil = Instantiate(prefabProjetil, pontoDeDisparo.position, Quaternion.identity);

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
            rb.linearVelocity = new Vector2(velocidadeProjetil * direcao, 0);
        }
    }
}