using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float speed = 3.0f;
    public float detectionRange = 10.0f;

    [Header("Referências")]
    public Transform player;

    // Construtor (opcional — caso pretenda usar, o nome TEM de ser idêntico à classe)
    public EnemyAI()
    {
        // Inicialização de variáveis se necessário
    }

    // Método Unity Start (com o tipo de retorno 'void' obrigatório)
    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    // Método Unity Update (com o tipo de retorno 'void' obrigatório)
    private void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= detectionRange)
            {
                MoverEmDirecaoAoJogador();
            }
        }
    }

    // Exemplo de método customizado (garanta que tem 'void' ou outro tipo de retorno)
    public void MoverEmDirecaoAoJogador()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
}

