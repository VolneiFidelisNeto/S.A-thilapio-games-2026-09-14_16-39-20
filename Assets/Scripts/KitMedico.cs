using UnityEngine;

/// <summary>
/// Implemente esta interface no script de vida do jogador (ou de qualquer
/// outro objeto que possa ser curado).
/// </summary>
public interface ICuravel
{
    /// <summary>Recebe a cura. Retorna true se a vida realmente aumentou.</summary>
    bool Curar(float quantidade);
}

/// <summary>
/// Kit médico (2D). Coloque em um objeto com Collider2D (Is Trigger ligado).
/// Quando o jogador encosta, ele recebe cura e o kit some (ou reaparece depois).
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class KitMedico : MonoBehaviour
{
    [Header("Cura")]
    [SerializeField] private float quantidadeCura = 25f;
    [SerializeField] private string tagJogador = "Player";

    [Header("Respawn (opcional)")]
    [SerializeField] private bool reaparece = false;
    [SerializeField] private float tempoRespawn = 15f;

    [Header("Efeitos (opcionais)")]
    [SerializeField] private AudioClip somColeta;
    [SerializeField, Range(0f, 1f)] private float volumeSom = 1f;
    [SerializeField] private GameObject efeitoColeta; // prefab de partículas

    [Header("Animação flutuante")]
    [SerializeField] private bool flutuar = true;
    [SerializeField] private float amplitude = 0.1f;
    [SerializeField] private float velocidade = 2f;

    private Vector3 posicaoInicial;
    private Collider2D col;
    private SpriteRenderer[] sprites;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
        sprites = GetComponentsInChildren<SpriteRenderer>();
        posicaoInicial = transform.position;
    }

    private void Update()
    {
        if (!flutuar) return;
        float y = Mathf.Sin(Time.time * velocidade) * amplitude;
        transform.position = posicaoInicial + new Vector3(0f, y, 0f);
    }

    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (!outro.CompareTag(tagJogador)) return;

        ICuravel alvo = outro.GetComponentInParent<ICuravel>();
        if (alvo == null) return;

        // Só consome o kit se o jogador realmente precisava de cura.
        if (!alvo.Curar(quantidadeCura))
        {
            Debug.Log("Jogador com a vida cheia, o kit médico não foi usado.");
            return;
        }

        Debug.Log($"Jogador foi curado! +{quantidadeCura} de vida.");
        Coletar();
    }

    private void Coletar()
    {
        if (somColeta != null)
            AudioSource.PlayClipAtPoint(somColeta, transform.position, volumeSom);

        if (efeitoColeta != null)
            Instantiate(efeitoColeta, transform.position, Quaternion.identity);

        if (reaparece)
        {
            SetAtivo(false);
            Invoke(nameof(Reaparecer), tempoRespawn);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Reaparecer() => SetAtivo(true);

    private void SetAtivo(bool ativo)
    {
        col.enabled = ativo;
        foreach (var s in sprites) s.enabled = ativo;
    }
}

/* ------------------------------------------------------------------
   EXEMPLO: como ligar ao script de vida do jogador

   public class VidaJogador : MonoBehaviour, ICuravel
   {
       [SerializeField] private float vidaMaxima = 100f;
       private float vidaAtual;

       private void Awake() => vidaAtual = vidaMaxima;

       public bool Curar(float quantidade)
       {
           if (vidaAtual >= vidaMaxima) return false; // vida cheia: não gasta o kit
           vidaAtual = Mathf.Min(vidaAtual + quantidade, vidaMaxima);
           // atualize a barra de vida aqui
           return true;
       }
   }
------------------------------------------------------------------ */