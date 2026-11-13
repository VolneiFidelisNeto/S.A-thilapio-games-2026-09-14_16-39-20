using UnityEngine;

public class Inimigo : MonoBehaviour
{
    [Header("Configurações de Vida")]
    public int vidaMaxima = 100;
    private int vidaAtual;

    

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

   
    }
