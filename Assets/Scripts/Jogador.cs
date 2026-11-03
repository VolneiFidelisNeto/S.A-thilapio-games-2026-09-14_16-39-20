using UnityEngine;

public class Jogador : MonoBehaviour
{
    [Header("Configurações de Vida")]
    public int vidaMaxima = 100;
    private int vidaAtual;

    [Header("Estado")]
    public bool estaInvencivel = false;
    public float tempoInvencibilidade = 1f;

    void Start()
    {
        vidaAtual = vidaMaxima;
    }

    // Método chamado pelo script do Inimigo ao colidir com o jogador
    public void ReceberDano(int quantidadeDano)
    {
        // Se estiver num período de invencibilidade após um golpe, ignora o dano
        if (estaInvencivel) return;

        vidaAtual -= quantidadeDano;
        Debug.Log("Jogador recebeu " + quantidadeDano + " de dano. Vida restante: " + vidaAtual);

        if (vidaAtual <= 0)
        {
            Morrer();
        }
        else
        {
            // Ativa um pequeno tempo de invencibilidade para não tomar dano em todos os frames
            StartCoroutine(AtivarInvencibilidade());
        }
    }

    public void Curar(int quantidadeCura)
    {
        vidaAtual += quantidadeCura;

        // Impede que a vida ultrapasse o valor máximo
        if (vidaAtual > vidaMaxima)
        {
            vidaAtual = vidaMaxima;
        }

        Debug.Log("Jogador curou " + quantidadeCura + ". Vida atual: " + vidaAtual);
    }

    private System.Collections.IEnumerator AtivarInvencibilidade()
    {
        estaInvencivel = true;
        yield return new WaitForSeconds(tempoInvencibilidade);
        estaInvencivel = false;
    }

    void Morrer()
    {
        Debug.Log("O jogador morreu!");

        // Exemplo: reinicia a cena atual ou destrói o jogador
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        Destroy(gameObject);
    }
}