using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Jogador : MonoBehaviour , ICuravel

{
    [SerializeField] private float vidaMaxima = 100f;
    private float vidaAtual;

  

    [Header("UI")]
    public Slider barraDeVida;

    [Header("Estado")]
    public bool estaInvencivel = false;
    public float tempoInvencibilidade = 1f;

    void Start()
    {
        vidaAtual = vidaMaxima;

        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMaxima;
            barraDeVida.value = vidaAtual;
        }
    }

    public void ReceberDano(int quantidadeDano)
    {
        if (estaInvencivel) return;

        vidaAtual -= quantidadeDano;
        if (vidaAtual < 0) vidaAtual = 0;

        AtualizarBarra();
        Debug.Log("Jogador recebeu " + quantidadeDano + " de dano. Vida restante: " + vidaAtual);

        if (vidaAtual <= 0)
        {
            Morrer();
        }
        else
        {
            StartCoroutine(AtivarInvencibilidade());
        }
    }

    public void Curar(int quantidadeCura)
    {
        vidaAtual += quantidadeCura;

        if (vidaAtual > vidaMaxima)
        {
            vidaAtual = vidaMaxima;
        }

        AtualizarBarra();
        Debug.Log("Jogador curou " + quantidadeCura + ". Vida atual: " + vidaAtual);
    }

    void AtualizarBarra()
    {
        if (barraDeVida != null)
        {
            barraDeVida.value = vidaAtual;
        }
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
        SceneManager.LoadScene(2); // Cena de Game Over
    }
    private void Awake() => vidaAtual = vidaMaxima;

    public bool Curar(float quantidade)
    {
        if (vidaAtual >= vidaMaxima) return false; // vida cheia: não gasta o kit
        vidaAtual = Mathf.Min(vidaAtual + quantidade, vidaMaxima);
        // atualize a barra de vida aqui
        return true;
    }

  
}