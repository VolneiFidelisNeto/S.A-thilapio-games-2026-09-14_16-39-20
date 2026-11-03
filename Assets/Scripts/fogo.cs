using UnityEngine;

public class fogo : MonoBehaviour
{
    public int dano = 20; // Valor do dano que a bala causa

    void Start()
    {
        // Destrói a bala após 3 segundos para não poluir o jogo
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        {
            if (collision.CompareTag("Player")) return;

            // 1. Tenta causar dano diretamente a um Inimigo
            Inimigo inimigo = collision.GetComponent<Inimigo>();
            if (inimigo != null)
            {
                inimigo.ReceberDano(dano);
            }

            // 2. Tenta fazer explodir o Barril
            Barril barril = collision.GetComponent<Barril>(); // Ajusta "Barril" para o nome exato do script da imagem
            if (barril != null)
            {
                barril.TakeDamage();
            }

            Destroy(gameObject);
        }
    }
   
    
}
