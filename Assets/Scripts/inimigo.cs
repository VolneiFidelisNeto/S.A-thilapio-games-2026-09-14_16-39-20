using UnityEngine;

public class Inimigo : MonoBehaviour
{
    [Header("Configurações de Vida")]
    public int vidaMaxima = 100;
    private int vidaAtual;

    [Header("Configurações de Ataque")]
    public int danoAoJogador = 15;

    void Start()
    {
        vidaAtual = vidaMaxima;
    }

    // Método chamado por outros scripts (ex: a bala) para causar dano ao inimigo
    public void ReceberDano(int quantidadeDano)
    {
        vidaAtual -= quantidadeDano;
        Debug.Log("Inimigo recebeu " + quantidadeDano + " de dano. Vida restante: " + vidaAtual);

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        // Adicione aqui efeitos de morte, animação, drop de itens, etc.
        Destroy(gameObject);
    }

    // Causa dano ao Jogador assim que o inimigo encosta nele
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Tenta obter o componente de vida do jogador (ajuste "Jogador" para o nome do seu script de vida)
            // CORRETO:
            Jogador jogador = collision.GetComponent<Jogador>();
            if (jogador != null)
            {
                jogador.ReceberDano(danoAoJogador);
            }
        }
    }
}