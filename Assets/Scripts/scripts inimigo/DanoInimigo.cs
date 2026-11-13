using UnityEngine;

public class InimigoDano : MonoBehaviour
{
    public int dano = 10;

    // Use OnCollisionEnter (sem o "2D") se o seu jogo for 3D
    private void OnCollisionEnter2D(Collision2D colisao)
    {
        // Verifica se o objeto que encostou tem a Tag "Player"
        if (colisao.gameObject.CompareTag("Player"))
        {
            // Busca o script de vida do jogador e aplica o dano
            Jogador vida = colisao.gameObject.GetComponent<Jogador>();
            if (vida != null)
            {
                vida.ReceberDano(dano);
            }
        }
    }
}