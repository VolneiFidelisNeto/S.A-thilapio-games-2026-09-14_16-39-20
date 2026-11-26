using UnityEngine;
public class arma : MonoBehaviour
{
    public enum TipoArma
    {
        Pistola,
        Espingarda,
        Fuzil,
        Bazuca
    }

    /// <summary>
    /// Configuração de um modo de disparo. Tudo aparece no Inspector.
    /// </summary>
    [System.Serializable]
    public class ModoArma
    {
        public string nome = "Modo";
        public TipoArma tipo = TipoArma.Pistola;

        [Header("Projétil")]
        public GameObject projetilPrefab;          // se vazio, usa o "projetilPadrao" da Arma
        public float velocidadeProjetil = 12f;
        public float tempoDeVidaProjetil = 3f;
        public float escalaProjetil = 1f;

        [Header("Disparo")]
        public float cadenciaTiro = 0.3f;          // segundos entre tiros
        public bool tiroAutomatico = false;        // segurar o botão direito atira continuamente
        public int projeteisPorTiro = 1;           // espingarda = vários
        public float dispersaoGraus = 0f;          // abertura total do cone de tiro
    }

    /// <summary>
    /// Arma 2D com 4 modos: Pistola, Espingarda, Fuzil e Bazuca.
    /// - Fica no mapa até o jogador encostar nela.
    /// - Depois de pega, mira no mouse e atira com o BOTÃO DIREITO.
    /// - Troca de modo com as teclas 1, 2, 3, 4 ou com a rodinha do mouse.
    ///
    /// Como usar:
    /// 1) Coloque este script no objeto da arma que está no mapa.
    /// 2) Esse objeto precisa de um Collider2D com "Is Trigger" marcado.
    /// 3) O jogador precisa da tag "Player" e de um Collider2D + Rigidbody2D.
    /// 4) Arraste o(s) prefab(s) de projétil nos modos (ou no "Projetil Padrao").
    ///    Para a bazuca, use um prefab maior/de míssil (e com explosão, se quiser).
    /// </summary>
    public class Arma : MonoBehaviour
    {
        [Header("Identificação")]
        public string weaponName = "Nova Arma";

        [Header("Coleta")]
        [SerializeField] private string tagDoJogador = "Player";
        [SerializeField] private bool seguirOJogadorDepoisDePegar = true; // vira filho do jogador
        [SerializeField] private bool mostrarArmaNaMao = true;            // false = some ao ser pega

        [Header("Projétil")]
        [SerializeField] private GameObject projetilPadrao;   // usado por modos sem prefab próprio
        [SerializeField] private Transform pontoDeTiro;       // objeto filho na ponta da arma

        [Header("Modos")]
        [SerializeField] private ModoArma[] modos = CriarModosPadrao();
        [SerializeField] private int modoAtual = 0;

        [Header("Mira")]
        [SerializeField] private bool girarParaOMouse = true;

        private bool equipada = false;
        private float proximoTiro;
        private Camera cam;

        private SpriteRenderer[] sprites;
        private Collider2D[] colisores;

        public ModoArma ModoAtual => modos[modoAtual];

        // Valores iniciais de cada modo
        private static ModoArma[] CriarModosPadrao()
        {
            return new ModoArma[]
            {
            new ModoArma { nome = "Pistola",    tipo = TipoArma.Pistola,    velocidadeProjetil = 14f, tempoDeVidaProjetil = 2f, escalaProjetil = 1f,   cadenciaTiro = 0.35f, tiroAutomatico = false, projeteisPorTiro = 1, dispersaoGraus = 1f  },
            new ModoArma { nome = "Espingarda", tipo = TipoArma.Espingarda, velocidadeProjetil = 12f, tempoDeVidaProjetil = 0.8f, escalaProjetil = 0.7f, cadenciaTiro = 0.9f,  tiroAutomatico = false, projeteisPorTiro = 6, dispersaoGraus = 30f },
            new ModoArma { nome = "Fuzil",      tipo = TipoArma.Fuzil,      velocidadeProjetil = 20f, tempoDeVidaProjetil = 3f, escalaProjetil = 0.8f, cadenciaTiro = 0.1f,  tiroAutomatico = true,  projeteisPorTiro = 1, dispersaoGraus = 4f  },
            new ModoArma { nome = "Bazuca",     tipo = TipoArma.Bazuca,     velocidadeProjetil = 7f,  tempoDeVidaProjetil = 4f, escalaProjetil = 1.8f, cadenciaTiro = 1.5f,  tiroAutomatico = false, projeteisPorTiro = 1, dispersaoGraus = 0f  },
            };
        }

        private void Awake()
        {
            cam = Camera.main;
            sprites = GetComponentsInChildren<SpriteRenderer>();
            colisores = GetComponentsInChildren<Collider2D>();

            // Garante que existam modos válidos
            if (modos == null || modos.Length == 0)
                modos = CriarModosPadrao();

            modoAtual = Mathf.Clamp(modoAtual, 0, modos.Length - 1);
        }

        private void Update()
        {
            // Só funciona depois que o jogador pegou a arma
            if (!equipada) return;

            TrocarModo();

            if (girarParaOMouse)
                MirarNoMouse();

            ModoArma modo = ModoAtual;

            // Botão direito do mouse = 1 (esquerdo = 0, meio = 2)
            bool apertou = modo.tiroAutomatico ? Input.GetMouseButton(1) : Input.GetMouseButtonDown(1);

            if (apertou && Time.time >= proximoTiro)
            {
                Use();
                proximoTiro = Time.time + modo.cadenciaTiro;
            }
        }

        // ---------------- Coleta ----------------

        private void OnTriggerEnter2D(Collider2D outro)
        {
            if (equipada) return;
            if (!outro.CompareTag(tagDoJogador)) return;

            Pegar(outro.transform);
        }

        private void Pegar(Transform jogador)
        {
            equipada = true;

            // Desliga o colisor para não ser pega de novo
            foreach (Collider2D c in colisores) c.enabled = false;

            if (!mostrarArmaNaMao)
                foreach (SpriteRenderer s in sprites) s.enabled = false;

            if (seguirOJogadorDepoisDePegar)
            {
                transform.SetParent(jogador);
                transform.localPosition = Vector3.zero;
            }
        }

        // ---------------- Troca de modo ----------------

        private void TrocarModo()
        {
            // Teclas 1 a 4
            for (int i = 0; i < modos.Length && i < 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    DefinirModo(i);
                    return;
                }
            }

            // Rodinha do mouse
            float scroll = Input.mouseScrollDelta.y;
            if (scroll > 0f) DefinirModo((modoAtual + 1) % modos.Length);
            else if (scroll < 0f) DefinirModo((modoAtual - 1 + modos.Length) % modos.Length);
        }

        public void DefinirModo(int indice)
        {
            if (indice < 0 || indice >= modos.Length || indice == modoAtual) return;

            modoAtual = indice;
            proximoTiro = Time.time + 0.15f; // pequeno atraso para evitar tiro acidental ao trocar
            Debug.Log("Modo da arma: " + ModoAtual.nome);
        }

        // ---------------- Ataque ----------------

        // Método chamado quando o jogador usa a arma
        public virtual void Use()
        {
            Atirar();
        }

        private void MirarNoMouse()
        {
            Vector3 mundo = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direcao = mundo - transform.position;
            float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angulo);
        }

        private void Atirar()
        {
            ModoArma modo = ModoAtual;
            GameObject prefab = modo.projetilPrefab != null ? modo.projetilPrefab : projetilPadrao;

            if (prefab == null)
            {
                Debug.LogWarning("Projétil prefab não foi atribuído no modo " + modo.nome + "!");
                return;
            }

            Transform origem = pontoDeTiro != null ? pontoDeTiro : transform;
            int quantidade = Mathf.Max(1, modo.projeteisPorTiro);

            for (int i = 0; i < quantidade; i++)
            {
                // Desvio aleatório dentro do cone de dispersão
                float desvio = Random.Range(-modo.dispersaoGraus * 0.5f, modo.dispersaoGraus * 0.5f);
                Quaternion rotacao = origem.rotation * Quaternion.Euler(0f, 0f, desvio);

                GameObject tiro = Instantiate(prefab, origem.position, rotacao);
                tiro.transform.localScale *= modo.escalaProjetil;

                Rigidbody2D rb = tiro.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Unity 6: use rb.linearVelocity. Versões antigas: rb.velocity
                    rb.linearVelocity = rotacao * Vector3.right * modo.velocidadeProjetil;
                }

                Destroy(tiro, modo.tempoDeVidaProjetil);
            }
        }
    }
}