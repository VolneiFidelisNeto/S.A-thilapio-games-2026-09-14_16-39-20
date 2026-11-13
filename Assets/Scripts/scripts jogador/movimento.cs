using UnityEngine;

public class movimento : MonoBehaviour
{
    public float velocidade = 8f;
    public float forcaPulo = 12f;

    private Rigidbody2D rb;
    private bool estaNoChao;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Movimento Horizontal (A/D ou Setas)
        float horizontal = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(horizontal * velocidade, rb.linearVelocity.y);

        // 2. Virar o personagem para o lado correto
        if (horizontal != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontal), 1, 1);
        }

        // 3. Pulo ao pressionar Espa�o (s� pula se estiver tocando o ch�o)
        if (Input.GetButtonDown("Jump") && estaNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
            estaNoChao = false;
        }
    }

    // Detecta contato com o ch�o usando os colliders
    private void OnCollisionEnter2D(Collision2D colisao)
    {
        estaNoChao = true;
    }

}
