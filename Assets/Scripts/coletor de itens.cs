using UnityEngine;

public class coletordeitens : MonoBehaviour
{
    [Header("Configurações do Item")]
    [SerializeField] private KeyCode teclaColeta = KeyCode.E;

    private bool jogadorPerto = false;

    private void Update()
    {
        // Verifica se o jogador está na área e pressionou a tecla escolhida
        if (jogadorPerto && Input.GetKeyDown(teclaColeta))
        {
            Coletar();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Garanta que o seu jogador tenha a Tag "Player" configurada na Unity
        if (collision.CompareTag("Player"))
        {
            jogadorPerto = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jogadorPerto = false;
        }
    }

    private void Coletar()
    {
        // Adicione aqui a lógica de inventário ou pontuação (ex: Inventario.Adicionar(item))
        Debug.Log("Item coletado!");

        // Destrói o objeto no mapa após a coleta
        Destroy(gameObject);
    }
}
